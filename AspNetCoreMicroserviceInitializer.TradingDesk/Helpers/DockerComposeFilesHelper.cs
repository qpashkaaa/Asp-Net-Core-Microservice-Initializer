using System.Text.RegularExpressions;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Helpers;

/// <summary>
/// Хелпер для работы с файлами docker-compose.
/// </summary>
public static class DockerComposeFilesHelper
{
    /// <summary>
    /// Базовая папка с файлами DockerTemplates.
    /// </summary>
    private const string TemplatesFolder = "DockerTemplates";

    /// <summary>
    /// Метод создания docker-compose.yml со стандартными параметрами из файла-шаблона docker-compose.template.
    /// </summary>
    /// <param name="fileModules">Модули, которые должны быть добавлены в файл docker-compose.</param>
    public static void CreateDockerComposeFileContent(List<DockerComposeFileModules> fileModules)
    {
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TemplatesFolder}/docker-compose.template");
        var content = File.ReadAllText(filePath);
        const string pattern = @"<(\w+)>(.*?)<\/\1>";
        var regex = new Regex(pattern, RegexOptions.Singleline);
        var contentWithFilteredModules = regex.Replace(content, match => ReplaceDockerComposeFileBlock(fileModules, match));
        
        const string newLinesPattern = @"(\n\s*\n)+";
        const string replacement = "\n\n";
        var result = Regex.Replace(contentWithFilteredModules, newLinesPattern, replacement);
        
        File.WriteAllText(filePath.Replace(".template", ".yml"), result);
        File.Delete(filePath);
    }

    /// <summary>
    /// Метод создания develop.env со стандартными параметрами из файла-шаблона develop.template.
    /// </summary>
    /// <param name="configPath">Путь до конфига appsettings.json.</param>
    public static void CreateEnvironmentFileContent(string configPath)
    {
        var envFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TemplatesFolder}/develop.template");
        
        var configContent = File.ReadAllText(configPath);
        var environmentFileContent = File.ReadAllLines(envFilePath).ToList();
        environmentFileContent.Add(string.Empty);

        var configContentDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(configContent);

        if (configContentDictionary != null)
        {
            foreach (var kvp in configContentDictionary)
            {
                var parsedBranch = ParseKvp($"{kvp.Key}__", kvp);
                environmentFileContent.AddRange(parsedBranch);
                environmentFileContent.Add(string.Empty);
            }

            File.WriteAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TemplatesFolder}/develop.env"), environmentFileContent);
            File.Delete(envFilePath);
        }
    }

    /// <summary>
    /// Метод парсинга дерева json в строку используя <see cref="KeyValuePair"/>.
    /// </summary>
    /// <param name="prefix">Префикс верхнего уровня.</param>
    /// <param name="kvp"><see cref="KeyValuePair"/> элемент.</param>
    /// <returns>Массив строк, полученный при обработке дерева json.</returns>
    private static List<string> ParseKvp(string prefix, KeyValuePair<string, object> kvp)
    {
        var result = new List<string>();

        if (kvp.Value is JObject jObjects)
        {
            foreach (var jObject in jObjects)
            {
                var parsedString = ParseKvp($"{prefix}{jObject.Key}__", new KeyValuePair<string, object>(jObject.Key, jObject.Value));
                result.AddRange(parsedString);
            }
        }

        if (kvp.Value is JArray jArray)
        {
            if (jArray.IsNullOrEmpty())
            {
                result.Add($"{prefix}0=");
            }

            for (int i = 0; i < jArray.Count; i++)
            {
                var parsedString = ParseKvp($"{prefix}{i}__", new KeyValuePair<string, object>(i.ToString(), jArray[i]));
                result.AddRange(parsedString);
            }
        }

        if (kvp.Value is JValue jValue)
        {
            var value = jValue.Value ?? "null";
            result.Add($"{prefix[..^2]}={value}");
        }
        
        return result;
    }

    /// <summary>
    /// Метод замены блоков файла docker-compose.yml в зависимости от переданных модулей (переданные модули остаются, остальные удаляются).
    /// </summary>
    /// <param name="fileModules">Модули, которые должны быть добавлены в файл docker-compose.</param>
    /// <param name="match">Модель, которая представляет результаты одного сопоставления с регулярным выражением.</param>
    /// <returns>Отфильтрованную строку.</returns>
    /// <exception cref="Exception">Пробрасывается ошибка в случае, если не удается преобразовать Enum.</exception>
    private static string ReplaceDockerComposeFileBlock(
        List<DockerComposeFileModules> fileModules,
        Match match)
    {
        var moduleName = match.Groups[1].Value;
        var blockContent = match.Groups[2].Value;

        if (Enum.TryParse(moduleName, out DockerComposeFileModules module))
        {
            return fileModules.Contains(module) ? blockContent : string.Empty;
        }

        throw new Exception($"Не удалось преобразовать модуль {moduleName} в Enum {nameof(DockerComposeFileModules)}");
    }
}
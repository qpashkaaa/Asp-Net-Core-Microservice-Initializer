using System.Text;
using System.Text.RegularExpressions;
using AspNetCoreMicroserviceInitializer.TradingDesk.Enums;
using IdentityModel.OidcClient;
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
        var bytes = Convert.FromBase64String("dmVyc2lvbjogIjMuOSINCg0KbmV0d29ya3M6DQogIGFwcC1uZXR3b3JrOg0KDQpzZXJ2aWNlczoNCjxTZXJ2ZXI+DQogIHNlcnZlcjoNCiAgICBidWlsZDoNCiAgICAgICMg0JzQvtC20L3QviDQu9C40LHQviDQvtCx0YDQsNGC0LjRgtGM0YHRjyDQuiDQutC+0L3QutGA0LXRgtC90L7QvNGDIGltYWdlLCDQu9C40LHQviDQuiBEb2NrZXJmaWxlLg0KICAgICAgI2ltYWdlOg0KICAgICAgI2NvbnRleHQ6IA0KICAgICAgI2RvY2tlcmZpbGU6IA0KICAgIGNvbnRhaW5lcl9uYW1lOiBzZXJ2ZXINCiAgICBlbnZpcm9ubWVudDoNCiAgICAgIFRaOiAke1RJTUVfWk9ORX0NCiAgICBwb3J0czoNCiAgICAgIC0gIjgwMDA6ODAwMCINCiAgICBlbnZfZmlsZTogJHtFTlZfRklMRX0NCiAgICBuZXR3b3JrczoNCiAgICAgIC0gYXBwLW5ldHdvcmsNCjwvU2VydmVyPg0KDQo8Q2xpZW50Pg0KICBjbGllbnQ6DQogICAgYnVpbGQ6DQogICAgICAjINCc0L7QttC90L4g0LvQuNCx0L4g0L7QsdGA0LDRgtC40YLRjNGB0Y8g0Log0LrQvtC90LrRgNC10YLQvdC+0LzRgyBpbWFnZSwg0LvQuNCx0L4g0LogRG9ja2VyZmlsZS4NCiAgICAgICNpbWFnZToNCiAgICAgICNjb250ZXh0OiANCiAgICAgICNkb2NrZXJmaWxlOiANCiAgICBjb250YWluZXJfbmFtZTogY2xpZW50DQogICAgZGVwZW5kc19vbjoNCiAgICAgIHNlcnZlcjoNCiAgICAgICAgY29uZGl0aW9uOiBzZXJ2aWNlX3N0YXJ0ZWQNCiAgICBlbnZpcm9ubWVudDoNCiAgICAgIFRaOiAke1RJTUVfWk9ORX0NCiAgICBwb3J0czoNCiAgICAgIC0gIjgwMDE6ODAwMSINCiAgICBlbnZfZmlsZTogJHtFTlZfRklMRX0NCiAgICBuZXR3b3JrczoNCiAgICAgIC0gYXBwLW5ldHdvcmsNCjwvQ2xpZW50Pg0KDQo8QWRtaW5lcj4NCiAgYWRtaW5lcjoNCiAgICBpbWFnZTogYWRtaW5lcjpsYXRlc3QNCiAgICBjb250YWluZXJfbmFtZTogYWRtaW5lcg0KICAgIHBvcnRzOg0KICAgICAgLSAiODAwMjo4MDAyIg0KICAgIGVudmlyb25tZW50Og0KICAgICAgVFo6ICR7VElNRV9aT05FfQ0KICAgIG5ldHdvcmtzOg0KICAgICAgLSBhcHAtbmV0d29yaw0KPC9BZG1pbmVyPg0KDQo8TW9uZ29EYj4NCiAgbW9uZ29kYjoNCiAgICBpbWFnZTogbW9uZ286bGF0ZXN0DQogICAgY29udGFpbmVyX25hbWU6IG1vbmdvZGINCiAgICByZXN0YXJ0OiBhbHdheXMNCiAgICBlbnZpcm9ubWVudDoNCiAgICAgIE1PTkdPX0lOSVREQl9ST09UX1VTRVJOQU1FOiB5b3VyX3VzZXJuYW1lDQogICAgICBNT05HT19JTklUREJfUk9PVF9QQVNTV09SRDogeW91cl9wYXNzd29yZA0KICAgICAgVFo6ICR7VElNRV9aT05FfQ0KICAgIHBvcnRzOg0KICAgICAgLSAiMjcwMTc6MjcwMTciDQogICAgbmV0d29ya3M6DQogICAgICAtIGFwcC1uZXR3b3JrDQo8L01vbmdvRGI+DQoNCjxNb25nb0V4cHJlc3M+DQogIG1vbmdvLWV4cHJlc3M6DQogICAgaW1hZ2U6IG1vbmdvLWV4cHJlc3M6bGF0ZXN0DQogICAgY29udGFpbmVyX25hbWU6IG1vbmdvLWV4cHJlc3MNCiAgICByZXN0YXJ0OiBhbHdheXMNCiAgICBkZXBlbmRzX29uOg0KICAgICAgLSBtb25nb2RiDQogICAgcG9ydHM6DQogICAgICAtICI4MDgxOjgwODEiDQogICAgZW52aXJvbm1lbnQ6DQogICAgICBNRV9DT05GSUdfTU9OR09EQl9BRE1JTlVTRVJOQU1FOiByb290DQogICAgICBNRV9DT05GSUdfTU9OR09EQl9BRE1JTlBBU1NXT1JEOiBleGFtcGxlcGFzcw0KICAgICAgTUVfQ09ORklHX01PTkdPREJfU0VSVkVSOiBtb25nb2RiDQogICAgICBUWjogJHtUSU1FX1pPTkV9DQogICAgbmV0d29ya3M6DQogICAgICAtIGFwcC1uZXR3b3JrDQo8L01vbmdvRXhwcmVzcz4NCg0KPENsaWNrSG91c2U+DQogIGNsaWNraG91c2U6DQogICAgaW1hZ2U6IHlhbmRleC9jbGlja2hvdXNlLXNlcnZlcjpsYXRlc3QNCiAgICBjb250YWluZXJfbmFtZTogY2xpY2tob3VzZQ0KICAgIGVudmlyb25tZW50Og0KICAgICAgVFo6ICR7VElNRV9aT05FfQ0KICAgIHBvcnRzOg0KICAgICAgLSAiODEyMzo4MTIzIg0KICAgICAgLSAiOTAwMDo5MDAwIg0KICAgIG5ldHdvcmtzOg0KICAgICAgLSBhcHAtbmV0d29yaw0KPC9DbGlja0hvdXNlPg0KDQo8TXlTcWw+DQogIG15c3FsOg0KICAgIGltYWdlOiBteXNxbDpsYXRlc3QNCiAgICBjb250YWluZXJfbmFtZTogbXlzcWwNCiAgICByZXN0YXJ0OiBhbHdheXMNCiAgICBlbnZpcm9ubWVudDoNCiAgICAgIE1ZU1FMX1JPT1RfUEFTU1dPUkQ6IGV4YW1wbGUNCiAgICAgIE1ZU1FMX0RBVEFCQVNFOiBleGFtcGxlZGINCiAgICAgIE1ZU1FMX1VTRVI6IGV4YW1wbGV1c2VyDQogICAgICBNWVNRTF9QQVNTV09SRDogZXhhbXBsZXBhc3MNCiAgICAgIFRaOiAke1RJTUVfWk9ORX0NCiAgICBwb3J0czoNCiAgICAgIC0gIjMzMDY6MzMwNiINCiAgICBuZXR3b3JrczoNCiAgICAgIC0gYXBwLW5ldHdvcmsNCjwvTXlTcWw+DQoNCjxSZWRpcz4NCiAgcmVkaXM6DQogICAgaW1hZ2U6IHJlZGlzOmxhdGVzdA0KICAgIGNvbnRhaW5lcl9uYW1lOiByZWRpcw0KICAgIHJlc3RhcnQ6IGFsd2F5cw0KICAgIGVudmlyb25tZW50Og0KICAgICAgVFo6ICR7VElNRV9aT05FfQ0KICAgIHBvcnRzOg0KICAgICAgLSAiNjM3OTo2Mzc5Ig0KICAgIG5ldHdvcmtzOg0KICAgICAgLSBhcHAtbmV0d29yaw0KPC9SZWRpcz4NCg0KPEVsYXN0aWNzZWFyY2g+DQogIGVsYXN0aWNzZWFyY2g6DQogICAgaW1hZ2U6IGRvY2tlci5lbGFzdGljLmNvL2VsYXN0aWNzZWFyY2gvZWxhc3RpY3NlYXJjaDpsYXRlc3QNCiAgICBjb250YWluZXJfbmFtZTogZWxhc3RpY3NlYXJjaA0KICAgIGVudmlyb25tZW50Og0KICAgICAgLSBkaXNjb3ZlcnkudHlwZT1zaW5nbGUtbm9kZQ0KICAgICAgLSBFU19KQVZBX09QVFM9LVhtczUxMm0gLVhteDUxMm0NCiAgICAgIFRaOiAke1RJTUVfWk9ORX0NCiAgICB1bGltaXRzOg0KICAgICAgbWVtbG9jazoNCiAgICAgICAgc29mdDogLTENCiAgICAgICAgaGFyZDogLTENCiAgICBtZW1fbGltaXQ6IDFnDQogICAgcG9ydHM6DQogICAgICAtICI5MjAwOjkyMDAiDQogICAgICAtICI5MzAwOjkzMDAiDQogICAgbmV0d29ya3M6DQogICAgICAtIGFwcC1uZXR3b3JrDQo8L0VsYXN0aWNzZWFyY2g+DQoNCjxLaWJhbmE+DQogIGtpYmFuYToNCiAgICBpbWFnZTogZG9ja2VyLmVsYXN0aWMuY28va2liYW5hL2tpYmFuYTpsYXRlc3QNCiAgICBjb250YWluZXJfbmFtZToga2liYW5hDQogICAgcG9ydHM6DQogICAgICAtICI1NjAxOjU2MDEiDQogICAgZGVwZW5kc19vbjoNCiAgICAgIC0gZWxhc3RpY3NlYXJjaA0KICAgIGVudmlyb25tZW50Og0KICAgICAgRUxBU1RJQ1NFQVJDSF9VUkw6IGh0dHA6Ly9lbGFzdGljc2VhcmNoOjkyMDANCiAgICAgIFRaOiAke1RJTUVfWk9ORX0NCiAgICBuZXR3b3JrczoNCiAgICAgIC0gYXBwLW5ldHdvcmsNCjwvS2liYW5hPg0KDQo8Q2Fzc2FuZHJhPg0KICBjYXNzYW5kcmE6DQogICAgaW1hZ2U6IGNhc3NhbmRyYTpsYXRlc3QNCiAgICBjb250YWluZXJfbmFtZTogY2Fzc2FuZHJhDQogICAgcG9ydHM6DQogICAgICAtICI5MDQyOjkwNDIiDQogICAgZW52aXJvbm1lbnQ6DQogICAgICAtIENBU1NBTkRSQV9DTFVTVEVSX05BTUU9bXlfY2x1c3Rlcg0KICAgICAgLSBDQVNTQU5EUkFfREM9REMxDQogICAgICAtIENBU1NBTkRSQV9SQUNLPVJBQzENCiAgICAgIC0gQ0FTU0FORFJBX0VORFBPSU5UX1NOSVRDSD1Hb3NzaXBpbmdQcm9wZXJ0eUZpbGVTbml0Y2gNCiAgICAgIFRaOiAke1RJTUVfWk9ORX0NCiAgICBuZXR3b3JrczoNCiAgICAgIC0gYXBwLW5ldHdvcmsNCjwvQ2Fzc2FuZHJhPg0KDQo8UmFiYml0TXE+DQogIHJhYmJpdG1xOg0KICAgIGltYWdlOiByYWJiaXRtcTptYW5hZ2VtZW50DQogICAgY29udGFpbmVyX25hbWU6IHJhYmJpdG1xDQogICAgcG9ydHM6DQogICAgICAtICI1NjcyOjU2NzIiICAgICMg0L/QvtGA0YIg0LTQu9GPIEFNUVANCiAgICAgIC0gIjE1NjcyOjE1NjcyIiAgIyDQv9C+0YDRgiDQtNC70Y8g0LLQtdCxLdC40L3RgtC10YDRhNC10LnRgdCwINGD0L/RgNCw0LLQu9C10L3QuNGPDQogICAgZW52aXJvbm1lbnQ6DQogICAgICBSQUJCSVRNUV9ERUZBVUxUX1VTRVI6IHVzZXINCiAgICAgIFJBQkJJVE1RX0RFRkFVTFRfUEFTUzogcGFzc3dvcmQNCiAgICAgIFRaOiAke1RJTUVfWk9ORX0NCiAgICBuZXR3b3JrczoNCiAgICAgIC0gYXBwLW5ldHdvcmsNCjwvUmFiYml0TXE+DQoNCjxQcm9tZXRoZXVzPg0KICBwcm9tZXRoZXVzOg0KICAgIGltYWdlOiBwcm9tL3Byb21ldGhldXM6bGF0ZXN0DQogICAgZW52aXJvbm1lbnQ6DQogICAgICBUWjogJHtUSU1FX1pPTkV9DQogICAgcG9ydHM6DQogICAgICAtICI5MDkwOjkwOTAiDQogICAgbmV0d29ya3M6DQogICAgICAtIGFwcC1uZXR3b3JrDQo8L1Byb21ldGhldXM+DQoNCjxHcmFmYW5hPg0KICBncmFmYW5hOg0KICAgIGltYWdlOiBncmFmYW5hL2dyYWZhbmE6bGF0ZXN0DQogICAgZW52aXJvbm1lbnQ6DQogICAgICBUWjogJHtUSU1FX1pPTkV9DQogICAgcG9ydHM6DQogICAgICAtICIzMDAwOjMwMDAiDQogICAgbmV0d29ya3M6DQogICAgICAtIGFwcC1uZXR3b3JrDQo8L0dyYWZhbmE+DQoNCjxOZ2lueD4NCiAgbmdpbng6DQogICAgaW1hZ2U6IG5naW54OmxhdGVzdA0KICAgIHBvcnRzOg0KICAgICAgLSAiODA6ODAiDQogICAgbmV0d29ya3M6DQogICAgICAtIGFwcC1uZXR3b3JrDQo8L05naW54Pg0KDQo8UG9zdGdyZVNxbD4NCiAgcG9zdGdyZXM6DQogICAgaW1hZ2U6IHBvc3RncmVzOmxhdGVzdA0KICAgIHJlc3RhcnQ6IGFsd2F5cw0KICAgIGVudmlyb25tZW50Og0KICAgICAgUE9TVEdSRVNfVVNFUjogcG9zdGdyZXMNCiAgICAgIFBPU1RHUkVTX0RCOiBwb3N0Z3Jlcw0KICAgICAgVFo6ICR7VElNRV9aT05FfQ0KICAgIHBvcnRzOg0KICAgICAgLSAiNTQzMjo1NDMyIg0KICAgIGhlYWx0aGNoZWNrOg0KICAgICAgdGVzdDogWyJDTUQtU0hFTEwiLCAicGdfaXNyZWFkeSAtVSAkJFBPU1RHUkVTX1VTRVIgLWQgJCRQT1NUR1JFU19EQiJdDQogICAgICBpbnRlcnZhbDogNXMNCiAgICAgIHRpbWVvdXQ6IDVzDQogICAgICByZXRyaWVzOiA1DQogICAgZW52X2ZpbGU6ICR7RU5WX0ZJTEV9DQogICAgbmV0d29ya3M6DQogICAgICAtIGFwcC1uZXR3b3JrDQo8L1Bvc3RncmVTcWw+");

        var content = Encoding.UTF8.GetString(bytes);
        const string pattern = @"<(\w+)>(.*?)<\/\1>";
        var regex = new Regex(pattern, RegexOptions.Singleline);
        var contentWithFilteredModules = regex.Replace(content, match => ReplaceDockerComposeFileBlock(fileModules, match));
        
        const string newLinesPattern = @"(\n\s*\n)+";
        const string replacement = "\n\n";
        var result = Regex.Replace(contentWithFilteredModules, newLinesPattern, replacement);

        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TemplatesFolder}/docker-compose.yml");
        
        if (TryCreateDirectory(path))
        {
            File.WriteAllText(path, result);
        }
    }

    /// <summary>
    /// Метод создания develop.env со стандартными параметрами из файла-шаблона develop.template.
    /// </summary>
    /// <param name="configPath">Путь до конфига appsettings.json.</param>
    public static void CreateEnvironmentFileContent(string configPath)
    {
        var bytes = Convert.FromBase64String("IyDQn9GD0YLRjCDQtNC+IC5lbnYg0YTQsNC50LvQsCDQvtGC0L3QvtGB0LjRgtC10LvRjNC90L4gZG9ja2VyLWNvbXBvc2UgKNC40YHQv9C+0LvRjNC30YPQtdGC0YHRjyDQtNC70Y8g0L/RgNC+0L/QuNGB0YvQstCw0L3QuNGPIC5lbnYg0YTQsNC50LvQvtCyINCyINGB0LXRgNCy0LjRgdGLIGRvY2tlci1jb21wb3NlKS4NCkVOVl9GSUxFPWRldmVsb3AuZW52DQoNCiMg0JLRgNC10LzQtdC90L3QsNGPINC30L7QvdCwINC/0YDQuNC70L7QttC10L3QuNC5Lg0KVElNRV9aT05FPUV1cm9wZS9Nb3Njb3c=");
        
        var configContent = File.ReadAllText(configPath);

        var environmentFileContentString = Encoding.UTF8.GetString(bytes);
        var environmentFileContent = environmentFileContentString
            .Split(new[ ] { "\r\n", "\n" }, StringSplitOptions.None)
            .ToList();
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

            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{TemplatesFolder}/develop.env");
            
            if (TryCreateDirectory(path))
            {
                File.WriteAllLines(path, environmentFileContent);
            }
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

    /// <summary>
    /// Метод создания директории.
    /// </summary>
    /// <param name="path">Полный путь до файла.</param>
    /// <returns><see langword="true"/>, если директория успешно создана или существует, <see langword="false"/>, если не удалось создать директорию</returns>
    private static bool TryCreateDirectory(string path)
    {
        var directory = Path.GetDirectoryName(path);

        if (path == null ||
            directory == null)
        {
            return false;
        }

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);

            return true;
        }

        return true;
    }
}
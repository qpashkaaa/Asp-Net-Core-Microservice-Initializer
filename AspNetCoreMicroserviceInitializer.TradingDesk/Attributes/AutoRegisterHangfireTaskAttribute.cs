using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

namespace AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;

/// <summary>
/// Атрибут для регистрации фоновых задач Hangfire.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class AutoRegisterHangfireTaskAttribute : Attribute
{
    /// <summary>
    /// Тип модели настроек фоновой задачи Hangfire.
    ///
    /// Параметр должен быть наследником типа <see cref="HangfireTaskSettingsBase"/>.
    /// </summary>
    public readonly Type SettingsType;

    /// <summary>
    /// Конструктор атрибута.
    /// </summary>
    /// <param name="settingsType">
    /// <see langword="typeof"/> модели настроек фоновой задачи Hangfire.
    /// Параметр должен быть наследником типа <see cref="HangfireTaskSettingsBase"/>.
    /// </param>
    public AutoRegisterHangfireTaskAttribute(Type settingsType)
    {
        if (!settingsType.IsSubclassOf(typeof(HangfireTaskSettingsBase)))
        {
            throw new ArgumentException($"{settingsType.FullName} {nameof(settingsType)} должен быть наследником типа {nameof(HangfireTaskSettingsBase)}");
        }
        
        SettingsType = settingsType;
    }
}
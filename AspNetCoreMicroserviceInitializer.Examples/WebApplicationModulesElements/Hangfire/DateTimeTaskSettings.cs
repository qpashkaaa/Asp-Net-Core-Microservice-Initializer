﻿using AspNetCoreMicroserviceInitializer.TradingDesk.Attributes;
using AspNetCoreMicroserviceInitializer.TradingDesk.Settings.Abstract;

namespace AspNetCoreMicroserviceInitializer.Examples.WebApplicationModulesElements.Hangfire;

[AutoRegisterConfigSettings]
public class DateTimeTaskSettings : HangfireTaskSettingsBase
{
}

using FIXProtocolAdapter.Common.Models.Settings.View;
using FIXProtocolAdapter.Common.Enums;
using System.Collections.Generic;
using System.Web.Hosting;
using System.IO;
using System.Linq;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FIXProtocolAdapter.Common.Classes
{
    public static class SettingsManager
    {
        private static Dictionary<ExchangeProvider, SettingsViewModel> _settings;

        public static string Get(ExchangeProvider provider, string key)
        {
            if (_settings == null)
                ReadConfigFromJSON();

            try
            {
                if (_settings == null) return null;

                var settings = _settings[provider];
                var value = settings.GetType().GetProperty(key)?.GetValue(settings, null);
                return value?.ToString();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public static string GetLicensePatch()
        {
            var webRoot = GetRootPatch();
            return $"{webRoot}App_Data\\License";
        }

        public static string GetLogPatch()
        {
            var webRoot = GetRootPatch();
            return $"{webRoot}App_Data\\Log";
        }

        public static string GetProvidersDataPatch()
        {
            var webRoot = GetProvidersPatch();
            return $"{webRoot}\\Data";
        }

        private static string GetRootPatch()
        {
            return HostingEnvironment.ApplicationPhysicalPath;
        }

        private static string GetProvidersPatch()
        {
            var webRoot = GetRootPatch();
            return $"{webRoot}App_Data\\Providers";
        }


        private static void ReadConfigFromJSON()
        {
            var providersPatch = GetProvidersPatch();
            _settings = new Dictionary<ExchangeProvider, SettingsViewModel>();

            try
            {
                using (var file = File.OpenText($"{providersPatch}\\Config.json"))
                using (var reader = new JsonTextReader(file))
                {
                    var json = JToken.ReadFrom(reader);
                    var bodyProviders = json["Providers"];
                    var settings = JsonConvert.DeserializeObject<List<SettingsViewModel>>(bodyProviders.ToString());

                    if (!settings.Any()) return;

                    foreach (var setting in settings)
                    {
                        var provider = (ExchangeProvider)Enum.Parse(typeof(ExchangeProvider), setting.Name);
                        _settings.Add(provider, setting);
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

    }
}
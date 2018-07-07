using Newtonsoft.Json;

namespace FIXProtocolAdapter.Common.Models.Settings.View
{
    public class SettingsViewModel
    {
        [JsonProperty("Name", Required = Required.Always)]
        public string Name { get; set; }

        [JsonProperty("Protocol", Required = Required.Always)]
        public string Protocol { get; set; }

        [JsonProperty("Passphrase", Required = Required.Always)]
        public string Passphrase { get; set; }

        [JsonProperty("APIKey", Required = Required.Always)]
        public string APIKey { get; set; }

        [JsonProperty("APISecret", Required = Required.Always)]
        public string APISecret { get; set; }

        [JsonProperty("SslCertificateFile", Required = Required.Always)]
        public string SslCertificateFile { get; set; }

        [JsonProperty("FIXServerUrl", Required = Required.Always)]
        public string FIXServerUrl { get; set; }

        [JsonProperty("FIXServerPort", Required = Required.Always)]
        public string FIXServerPort { get; set; }

        [JsonProperty("FIXServerAcceptPort", Required = Required.Always)]
        public string FIXServerAcceptPort { get; set; }

        [JsonProperty("HeartBtInt", Required = Required.Always)]
        public string HeartBtInt { get; set; }
    }
}
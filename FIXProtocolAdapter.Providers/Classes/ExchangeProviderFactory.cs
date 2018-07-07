using System;
using System.Linq;
using System.Reflection;
using FIXProtocolAdapter.Common.Classes;
using FIXProtocolAdapter.Common.Enums;
using FIXProtocolAdapter.Common.Interfaces;
using FIXProtocolAdapter.Providers.GDAX;

namespace FIXProtocolAdapter.Providers.Classes
{
    public class ExchangeProviderFactory
    {
        public IExchangeProvider GetExchangeProvider(ExchangeProvider name)
        {
            switch (name)
            {
                case ExchangeProvider.GDAX:
                    var protoсol = SettingsManager.Get(name, "Protocol");

                    return CreateInstance(ExchangeProvider.GDAX,
                        (ExchangeProtocol) Enum.Parse(typeof(ExchangeProtocol), protoсol));
                default:
                    throw new Exception("Unknown provider type");
            }
        }

        private IExchangeProvider CreateInstance(ExchangeProvider provider, ExchangeProtocol protokol)
        {
            try
            {
                var className = $"FIXProtocolAdapter.Providers.{ExchangeProvider.GDAX.ToString()}"
                                + $".{protokol.ToString()}ExchangeProvider";
                
                // ReSharper disable once AssignNullToNotNullAttribute
                return (IExchangeProvider)Activator.CreateInstance(null, className).Unwrap();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
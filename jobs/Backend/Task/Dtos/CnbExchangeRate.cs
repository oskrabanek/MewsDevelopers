using System;
using Newtonsoft.Json;

namespace ExchangeRateUpdater.Dtos
{
    public class CnbExchangeRate
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("order")]
        public string Order { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("validFor")]
        public DateTimeOffset ValidFor { get; set; }
    }
}

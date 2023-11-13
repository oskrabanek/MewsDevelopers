using System.Collections.Generic;

namespace ExchangeRateUpdater.Dtos
{
    public class CnbExchangeRates
    {
        [Newtonsoft.Json.JsonProperty("rates")]
        public IEnumerable<CnbExchangeRate> Rates { get; set; }
    }
}

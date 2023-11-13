using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRateUpdater.Dtos;
using Newtonsoft.Json;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private const string SourceUrl = "https://api.cnb.cz/cnbapi/exrates/daily?date={0}";
        private static Currency SourceCurrency = new Currency("CZK");
        private readonly HttpClient _client;

        public ExchangeRateProvider()
        {
            // TODO : Use IHttpFactory and use Polly policy for retrying
            _client = new HttpClient();
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // TODO: Make this method async
            var cnbExchangeRates = GetExchangeRatesFromCnbAsync(DateTime.UtcNow).Result;
            foreach (var currency in currencies)
            {
                var cnbExchangeRate = cnbExchangeRates.Rates.FirstOrDefault(r => string.Compare(r.CurrencyCode, currency.Code, true) == 0);
                if (cnbExchangeRate != null)
                {
                    var value = cnbExchangeRate.Rate / (decimal)cnbExchangeRate.Amount;
                    yield return new ExchangeRate(SourceCurrency, currency, value);
                }
            }
        }

        /// <summary>
        /// Get data from ČNB
        /// </summary>
        /// <returns></returns>
        private async Task<CnbExchangeRates> GetExchangeRatesFromCnbAsync(DateTimeOffset date)
        {
            var url = string.Format(SourceUrl, date.ToString("yyyy-MM-dd"));
            var cnbExchangeRatesResponse = await _client.GetAsync(url);
            var cnbExchangeRatesJson = await cnbExchangeRatesResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CnbExchangeRates>(cnbExchangeRatesJson);
        }
    }
}

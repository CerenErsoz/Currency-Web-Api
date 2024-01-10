using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Currency_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly string _apiUrl = "https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies";


        //Desteklenen currency tiplerini listeler
        [HttpGet]
        public async Task<String> GetAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var newApiUrl = _apiUrl + ".json";
                HttpResponseMessage response = await client.GetAsync(_apiUrl);
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }


        //Istediğiniz bir tane currency'nin karşılıklarını getirir
        [HttpGet("{baseCurrency}")]
        public async Task<String> Get(string baseCurrency)
        {
            using (HttpClient client = new HttpClient())
            {
                var newApiUrl = _apiUrl + "/" + baseCurrency + ".json";
                HttpResponseMessage response = await client.GetAsync(newApiUrl);
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }


        //Source currency'nin target currency karşılığını getirir
        [HttpGet("exchange/{sourceCurrency}/{targetCurrency}")]
        public async Task<string> Get(string sourceCurrency, string targetCurrency)
        {
            using (HttpClient client = new HttpClient())
            {
                sourceCurrency = sourceCurrency.ToLower();
                targetCurrency = targetCurrency.ToLower();
                var newApiUrl = $"https://cdn.jsdelivr.net/gh/fawazahmed0/currency-api@1/latest/currencies/{sourceCurrency}/{targetCurrency}.json";
                HttpResponseMessage response = await client.GetAsync(newApiUrl);
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
        }


        //Girirlen tutar için A türünden B türüne conversion yapar
        [HttpGet("convert/{sourceCurrency}/{targetCurrency}")]
        public async Task<decimal> ConvertCurrency(string sourceCurrency, string targetCurrency, decimal value)
        {
            sourceCurrency = sourceCurrency.ToLower();
            targetCurrency = targetCurrency.ToLower();
            var newApiUrl = _apiUrl + "/" + $"{sourceCurrency}/{targetCurrency}.json";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(newApiUrl);
                string content = await response.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);
                json.TryGetValue(targetCurrency, out var currencyRate);
                decimal convertedValue = value * ((decimal)currencyRate);
                return convertedValue;
            }
        }
    }
}
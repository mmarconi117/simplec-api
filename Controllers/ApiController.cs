using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace theApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("bitcoin-prices")]
        public async Task<IActionResult> GetBitcoinPrices()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseString);

                var bitcoinPrices = new
                {
                    USD = json["bpi"]?["USD"]?["rate"]?.ToString(),
                    GBP = json["bpi"]?["GBP"]?["rate"]?.ToString(),
                    EUR = json["bpi"]?["EUR"]?["rate"]?.ToString(),
                    UpdatedTime = json["time"]?["updated"]?.ToString()
                };

                return Ok(bitcoinPrices);
            }
            catch (HttpRequestException e)
            {
                return StatusCode(500, "Error fetching Bitcoin prices from external API.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] dynamic data)
        {
            return Ok(new { received = data });
        }
    }
}

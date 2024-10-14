using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace theApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet("bitcoin-price")]
        public async Task<IActionResult> GetBitcoinPrice()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseString);

                // Extract Bitcoin prices
                var bitcoinPrices = new
                {
                    USD = json["bpi"]?["USD"]?["rate"]?.ToString() ?? "N/A",
                    GBP = json["bpi"]?["GBP"]?["rate"]?.ToString() ?? "N/A",
                    EUR = json["bpi"]?["EUR"]?["rate"]?.ToString() ?? "N/A",
                    UpdatedTime = json["time"]?["updated"]?.ToString() ?? "N/A"
                };

                return Ok(bitcoinPrices); // Return the Bitcoin prices as JSON
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error fetching data from CoinDesk API");
                return StatusCode(500, "Unable to fetch data from CoinDesk API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        // This method can be retained if you still need it for something else
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return Ok("Privacy Policy"); // Or whatever you want to return
        }
    }
}

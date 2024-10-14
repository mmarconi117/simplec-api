using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace theApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public ApiController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("bitcoin-prices")]
        public async Task<IActionResult> GetBitcoinPrices()
        {
            var response = await _httpClient.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Error fetching Bitcoin prices.");
            }

            var jsonData = await response.Content.ReadAsStringAsync();
            return Ok(jsonData);
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "Hello from API" });
        }

        [HttpPost]
        public IActionResult Post([FromBody] dynamic data)
        {
            return Ok(new { received = data });
        }
    }
}

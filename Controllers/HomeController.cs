using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using theApi.Models;
using Newtonsoft.Json.Linq;

namespace theApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync("https://api.coindesk.com/v1/bpi/currentprice.json");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseString);

                // Use null checks to avoid dereferencing null values
                ViewData["BitcoinPriceUSD"] = json["bpi"]?["USD"]?["rate"]?.ToString() ?? "N/A";
                ViewData["BitcoinPriceGBP"] = json["bpi"]?["GBP"]?["rate"]?.ToString() ?? "N/A";
                ViewData["BitcoinPriceEUR"] = json["bpi"]?["EUR"]?["rate"]?.ToString() ?? "N/A";
                ViewData["UpdatedTime"] = json["time"]?["updated"]?.ToString() ?? "N/A";
            }
            catch (HttpRequestException e)
            {
                _logger.LogError(e, "Error fetching data from CoinDesk API"); // Log the error for debugging
                ViewData["Error"] = "Unable to fetch data from CoinDesk API.";
            }
            catch (Exception ex) // Catch other exceptions as well
            {
                _logger.LogError(ex, "An unexpected error occurred");
                ViewData["Error"] = "An unexpected error occurred.";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

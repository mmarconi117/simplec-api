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
                response.EnsureSuccessStatusCode(); // Throws an exception if the status code is not success
                var responseString = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseString);

                ViewData["BitcoinPriceUSD"] = json["bpi"]["USD"]["rate"].ToString();
                ViewData["BitcoinPriceGBP"] = json["bpi"]["GBP"]["rate"].ToString();
                ViewData["BitcoinPriceEUR"] = json["bpi"]["EUR"]["rate"].ToString();
                ViewData["UpdatedTime"] = json["time"]["updated"].ToString();
            }
            catch (HttpRequestException e)
            {
                // Log and handle the error as needed
                ViewData["Error"] = "Unable to fetch data from CoinDesk API.";
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

using Microsoft.AspNetCore.Mvc;
using WeatherPredictor.Services;

namespace WeatherPredictor.Controllers
{
    public class ForecastController : Controller
    {
        private readonly ForecastAggregatorService _aggregatorService;
        private readonly ForecastHistoryService _historyService;

        public ForecastController(
            ForecastAggregatorService aggregatorService,
            ForecastHistoryService historyService)
        {
            _aggregatorService = aggregatorService;
            _historyService = historyService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string city, DateTime date)
        {
            try
            {
                var (apiForecast, mlForecast) = await _aggregatorService.GetAndSaveForecastAsync(city, date);

                ViewBag.City = city;
                ViewBag.Date = date.ToString("yyyy-MM-dd");
                ViewBag.ApiForecast = apiForecast;
                ViewBag.MlForecast = mlForecast;
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View();
        }

        [HttpGet]
        public IActionResult History()
        {
            var history = _historyService.GetAll()
                .OrderByDescending(h => h.RequestDate);
            return View(history);
        }
    }
}

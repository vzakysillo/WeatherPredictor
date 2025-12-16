using System.Threading.Tasks;

namespace WeatherPredictor.Services
{
    public class ForecastAggregatorService
    {
        private readonly OpenMateoService _openMateoService;
        private readonly MlForecastService _mlForecastService;
        private readonly ForecastHistoryService _historyService;

        public ForecastAggregatorService(
            OpenMateoService openMateoService,
            MlForecastService mlForecastService,
            ForecastHistoryService historyService)
        {
            _openMateoService = openMateoService;
            _mlForecastService = mlForecastService;
            _historyService = historyService;
        }

        public async Task<(string apiForecast, string mlForecast)> GetAndSaveForecastAsync(string city, DateTime date)
        {
            var apiForecast = await _openMateoService.GetForecastAsync(city, date);

            var mlForecast = await _mlForecastService.GetForecastAsync(
                city,
                date,
                apiForecast.Precipitation,
                apiForecast.TempMin, 
                0,
                "sunny" 
            );

            _historyService.SaveForecast(
                city,
                apiForecast.TempMax.ToString("F1"),
                mlForecast
            );

            return (apiForecast.TempMax.ToString("F1"), mlForecast);
        }
    }
}

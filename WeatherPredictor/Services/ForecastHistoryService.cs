using WeatherPredictor.Models.DbModels;
using WeatherPredictor.Repository;

namespace WeatherPredictor.Services
{
    public class ForecastHistoryService
    {
        private readonly IRepository<ForecastHistory> _repository;

        public ForecastHistoryService(IRepository<ForecastHistory> repository)
        {
            _repository = repository;
        }

        public IEnumerable<ForecastHistory> GetAll() => _repository.GetAll();

        public void SaveForecast(string city, string apiForecast, string mlForecast)
        {
            var history = new ForecastHistory
            {
                RequestDate = DateTime.Now,
                City = city,
                ApiForecast = apiForecast,
                MlForecast = mlForecast
            };

            _repository.Create(history);
        }
    }
}

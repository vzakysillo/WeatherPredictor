namespace WeatherPredictor.Models.DbModels
{
    public class ForecastHistory
    {
        public int Id { get; set; }

        public DateTime RequestDate { get; set; } 

        public string City { get; set; }            // місто

        public string ApiForecast { get; set; }     // прогноз від зовнішнього API

        public string MlForecast { get; set; }      // прогноз від ML-моделі
    }
}

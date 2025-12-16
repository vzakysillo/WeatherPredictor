using System.Text.Json.Serialization;

namespace WeatherPredictor.Models
{
    public class CityResult
    {
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class GeoResponse
    {
        public CityResult[] Results { get; set; }
    }

    public class ForecastResponse
    {
        public DailyData Daily { get; set; }
    }

    public class DailyData
    {
        public List<string> Time { get; set; }
        public List<double> Temperature_2m_max { get; set; }
        public List<double> Temperature_2m_min { get; set; }
        public List<double> Precipitation_sum { get; set; }
    }

    public class DayForecast
    {
        public string Date { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }
        public double Precipitation { get; set; }
    }
}

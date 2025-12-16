using System;
using System.Globalization;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherPredictor.Models;

namespace WeatherPredictor.Services
{
    public class OpenMateoService
    {
        private string CoordinatesApiUrl(string city)
        {
            return $"https://geocoding-api.open-meteo.com/v1/search?name={city}";
        }

        private string ForecastApiUrl(string latitude, string longitude)
        {
            return $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&daily=temperature_2m_max,temperature_2m_min,precipitation_sum&timezone=auto";
        }

        public CityResult GetCityCoordinates(string city)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = client.GetAsync(CoordinatesApiUrl(city)).Result;
                response.EnsureSuccessStatusCode();

                string json = response.Content.ReadAsStringAsync().Result;

                var result = JsonSerializer.Deserialize<GeoResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Results != null && result.Results.Length > 0)
                {
                    var cityData = result.Results[0];
                    return new CityResult()
                    {
                        Name = cityData.Name,
                        Latitude = cityData.Latitude,
                        Longitude = cityData.Longitude
                    };
                }
                else
                {
                    throw new Exception("City not found");
                }
            }
        }

        public async Task<DayForecast> GetForecastAsync(string city, DateTime date)
        {
            CityResult resultCity = GetCityCoordinates(city);

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(
                    ForecastApiUrl(
                        resultCity.Latitude.ToString(CultureInfo.InvariantCulture),
                        resultCity.Longitude.ToString(CultureInfo.InvariantCulture)
                    )
                );
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ForecastResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Daily?.Time != null)
                {
                    int index = result.Daily.Time.FindIndex(d => d == date.ToString("yyyy-MM-dd"));
                    if (index >= 0)
                    {
                        return new DayForecast
                        {
                            Date = result.Daily.Time[index],
                            TempMax = result.Daily.Temperature_2m_max[index],
                            TempMin = result.Daily.Temperature_2m_min[index],
                            Precipitation = result.Daily.Precipitation_sum[index]
                        };
                    }
                }

                throw new Exception("No forecast for this date");
            }
        }
    }
}

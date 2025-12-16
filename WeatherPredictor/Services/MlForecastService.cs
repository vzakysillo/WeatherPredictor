using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace WeatherPredictor.Services
{
    public class MlForecastService
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;

        public MlForecastService(IConfiguration configuration)
        {
            _apiKey = configuration["ML:ApiKey"];
            string endpointUrl = configuration["ML:EndpointUrl"];

            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback =
                    (req, cert, chain, errors) => true
            };

            _client = new HttpClient(handler)
            {
                BaseAddress = new Uri(endpointUrl)
            };
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<string> GetForecastAsync(
            string city,
            DateTime date,
            double precipitation,
            double tempMin,
            double wind,
            string weather)
        {
            var payload = new
            {
                input_data = new
                {
                    columns = new[] { "date", "precipitation", "temp_min", "wind", "weather" },
                    index = new[] { 0 },
                    data = new object[][]
                    {
                new object[]
                {
                    date.ToString("yyyy-MM-dd"),
                    precipitation,
                    tempMin,
                    wind,
                    weather
                }
                    }
                }
            };

            string json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("", content);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();

            return $"Error ML: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}";
        }
    }
}

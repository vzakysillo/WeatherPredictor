using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace WeatherPredictor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ForecastApiController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ForecastApiController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // Метод API для получения прогноза через ML
        [HttpPost]
        public async Task<IActionResult> GetForecast([FromBody] ForecastRequest request)
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
                            request.Date,
                            request.Precipitation,
                            request.TempMin,
                            request.Wind,
                            request.Weather
                        }
                    }
                }
            };

            string json = JsonSerializer.Serialize(payload);
            string mlUrl = _configuration["ML:EndpointUrl"];
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(mlUrl, content);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());

            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }
    }

    public class ForecastRequest
    {
        public string Date { get; set; }
        public double Precipitation { get; set; }
        public double TempMin { get; set; }
        public double Wind { get; set; }
        public string Weather { get; set; }
    }
}

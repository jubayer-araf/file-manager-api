using FileManagement.Models;
using FileManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ICustomAuthorizeService _customAuthorizeService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ICustomAuthorizeService customAuthorizeService)
        {
            _logger = logger;
            _customAuthorizeService = customAuthorizeService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet(Name = "getUser")]
        [Route("getUser")]
        public async Task<ApplicationUser> GetUser()
        {
            return await _customAuthorizeService.GetUserAsync(ControllerContext);
        }
    }
}
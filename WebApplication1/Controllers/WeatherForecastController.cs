using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly List<string> Summaries = new List<string>
        {
            "Fucking11", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly DbContext _context;

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,DbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            Console.WriteLine($"{Summaries.Count}");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Count)]
            })
            .ToArray();
        }

        [HttpGet]
        public object Fuck()
        {
            string content = "fuck this shit bitch";
            return _context.Database.ExecuteSqlRaw("UPDATE goods SET name = 123").ToString();
            Console.WriteLine(content);
            return Summaries;
        }
    }
}

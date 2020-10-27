using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ApplicationContext applicationContext;

        public WeatherForecastController(ApplicationContext _applicationContext)
        {
            applicationContext = _applicationContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var values = await applicationContext.Values.ToListAsync();
            return Ok(values);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var value = await applicationContext.Values.FirstOrDefaultAsync(x=>x.Id==id);
            if(value is Value)
            return Ok(value);
            else
            return NoContent();
        }
    }
}

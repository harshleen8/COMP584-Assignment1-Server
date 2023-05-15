using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {
        private readonly DataContext dc;
        public CityController(DataContext dc)
        {
            this.dc = dc;
        }

        public DataContext Dc { get; }


        //GET method: api/city
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCities()
        {
            var cities = await dc.Cities.ToListAsync();
            return Ok(cities);
        }

        //Post method: api/city/add?cityname=Miami
        [HttpPost("add")]
        public async Task<IActionResult> AddCity(string cityName, string country)
        {
            City city = new City();
            city.Name = cityName;
            city.Country = country;
            await dc.Cities.AddAsync(city);
            await dc.SaveChangesAsync();
            return Ok(city);
        }
    }
}

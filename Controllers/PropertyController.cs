using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PropertyController : Controller
    {
        private readonly DataContext dc;
        public PropertyController(DataContext dc)
        {
            this.dc = dc;
        }

        public DataContext Dc { get; }
        public IActionResult Index()
        {
            return View();
        }

        //property/detail/1
        [HttpGet("detail/{id}")]
        public async Task<IActionResult> GetPropertyDetail(int id)
        {
            
            return Ok();
        }

        [HttpGet]
        [Route("getProperties")]
        public async Task<IActionResult> GetProperties()
        {
            var properties = await dc.Properties.ToListAsync();
            return Ok(properties);
        }

        //property/add
        [HttpPost("add")]
        public async Task<IActionResult> AddProperty(string propertyName)
        {
            Property property = new Property();
            property.Name = propertyName;
            return StatusCode(201);
        }
    }
}

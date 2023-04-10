using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PropertyController : Controller
    {
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

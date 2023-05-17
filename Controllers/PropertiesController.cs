using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly DataContext dc;
        public PropertiesController(DataContext dc)
        {
            this.dc = dc;
        }

        // GET: api/Properties
        [HttpGet]
        [Route("getProperties")]
        public async Task<ActionResult<IEnumerable<Property>>> GetProperties()
        {
            List<Property> properties = await dc.Properties.ToListAsync();
            return properties;
        }

        // GET: api/Properties/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(int id)
        {
            Property? property = await dc.Properties.FindAsync(id);
            return property is null ? NotFound() : property;
        }
        private bool PropertyExists(int id) => dc.Properties.Any(e => e.Id == id);

        // property/add
        [HttpPost("/api/property/add")]
        public async Task<IActionResult> AddProperty([FromBody] PropertyInputModel inputModel)
        {

            var property = new Property
            {
                SellRent = inputModel.SellRent,
                Name = inputModel.Name,
                PropertyTypeId = inputModel.PType,
                BHK = inputModel.BHK,
                FurnishingTypeId = inputModel.FType,
                Price = inputModel.Price,
                Address = inputModel.Address,
                CityId = inputModel.City
            };

            dc.Properties.Add(property);
            await dc.SaveChangesAsync();

            return CreatedAtAction("GetProperty", new { id = property.Id }, property);
        }



        // DELETE: api/Properties/3
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProperty(int id)
        {
            Property? property = await dc.Properties.FindAsync(id);
            if (property == null)
            {
                return NotFound();
            }

            dc.Properties.Remove(property);
            await dc.SaveChangesAsync();

            return NoContent();
        }
        
    }
}

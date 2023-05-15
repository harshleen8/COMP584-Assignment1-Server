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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProperty(int id, Property updatedProperty)
        {
            var existingProperty = await dc.Properties.FindAsync(id);

            if (existingProperty == null)
            {
                return NotFound();
            }

            // Update only the provided fields
            existingProperty.Name = updatedProperty.Name ?? existingProperty.Name;
            existingProperty.CityId = updatedProperty.CityId != 0 ? updatedProperty.CityId : existingProperty.CityId;
            existingProperty.PropertyTypeId = updatedProperty.PropertyTypeId ?? existingProperty.PropertyTypeId;
            existingProperty.FurnishingTypeId = updatedProperty.FurnishingTypeId ?? existingProperty.FurnishingTypeId;

            dc.Entry(existingProperty).State = EntityState.Modified;

            try
            {
                await dc.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PropertyExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }




        //property/add
        [HttpPost("add")]
        public async Task<IActionResult> AddProperty(Property property)
        {
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

using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Atalnta", "New York","Los Angeles","Seattle" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Atalnta";
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

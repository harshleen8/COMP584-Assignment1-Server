using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly UserManager<HousingUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly string _pathName;

        public SeedController(UserManager<HousingUser> userManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, DataContext context, IHostEnvironment environment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _pathName = Path.Combine(environment.ContentRootPath, "Data/properties.csv");
        }

        // POST: api/Seed
        [HttpPost("Properties")]
        public async Task<IActionResult> ImportProperties()
        {
            Dictionary<string, Property> propertiesByName = _context.Properties
                .AsNoTracking().ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);

            IEnumerable<PropertiesCsv>? records = csv.GetRecords<PropertiesCsv>();
            foreach (PropertiesCsv record in records)
            {
                if (propertiesByName.ContainsKey(record.Name))
                {
                    continue;
                }

                Property property = new Property();

                // Handle null values for Name
                if (!string.IsNullOrEmpty(record.Name))
                {
                    property.Name = record.Name;
                }

                // Handle null values for CityId
                if (record.CityId != null)
                {
                    property.CityId = record.CityId;
                }

                // Handle null values for PropertyTypeId
                if (record.PropertyTypeId != null)
                {
                    property.PropertyTypeId = record.PropertyTypeId;
                }

                // Handle null values for FurnishingTypeId
                if (record.FurnishingTypeId != null)
                {
                    property.FurnishingTypeId = record.FurnishingTypeId;
                }

                // Handle null values for Price
                if (record.Price != null)
                {
                    property.Price = record.Price;
                }

                // Handle null values for BHK
                if (record.BHK != null)
                {
                    property.BHK = record.BHK;
                }

                // Handle null values for Address
                if (!string.IsNullOrEmpty(record.Address))
                {
                    property.Address = record.Address;
                }

                await _context.Properties.AddAsync(property);
                propertiesByName.Add(record.Name, property);
            }

            await _context.SaveChangesAsync();

            return new JsonResult(propertiesByName.Count);
        }



        [HttpPost("Users")]
        public async Task<IActionResult> ImportUsers()
        {
            const string roleUser = "RegisteredUser";
            const string roleAdmin = "Administrator";

            if (await _roleManager.FindByNameAsync(roleUser) is null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleUser));
            }
            if (await _roleManager.FindByNameAsync(roleAdmin) is null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleAdmin));
            }

            List<HousingUser> addedUserList = new();
            (string name, string email) = ("admin", "admin@email.com");

            if (await _userManager.FindByNameAsync(name) is null)
            {
                HousingUser userAdmin = new()
                {
                    UserName = name,
                    Email = email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _userManager.CreateAsync(userAdmin, _configuration["DefaultPasswords:Administrator"]
                    ?? throw new InvalidOperationException());
                await _userManager.AddToRolesAsync(userAdmin, new[] { roleUser, roleAdmin });
                userAdmin.EmailConfirmed = true;
                userAdmin.LockoutEnabled = false;
                addedUserList.Add(userAdmin);
            }

            (string name, string email) registered = ("user", "user@email.com");

            if (await _userManager.FindByNameAsync(registered.name) is null)
            {
                HousingUser user = new()
                {
                    UserName = registered.name,
                    Email = registered.email,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await _userManager.CreateAsync(user, _configuration["DefaultPasswords:RegisteredUser"]
                    ?? throw new InvalidOperationException());
                await _userManager.AddToRoleAsync(user, roleUser);
                user.EmailConfirmed = true;
                user.LockoutEnabled = false;
                addedUserList.Add(user);
            }

            if (addedUserList.Count > 0)
            {
                await _context.SaveChangesAsync();
            }

            return new JsonResult(new
            {
                addedUserList.Count,
                Users = addedUserList
            });
        }
    }
}

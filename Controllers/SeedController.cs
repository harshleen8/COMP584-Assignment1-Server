using CsvHelper;
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

        [HttpPost("Properties")]
        public async Task<IActionResult> ImportProperties()
        {
            try
            {
                using (var reader = new StreamReader(_pathName))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<Property>().ToList();

                    // Update existing records
                    var existingProperties = _context.Properties.ToList();
                    foreach (var existingProperty in existingProperties)
                    {
                        var updatedProperty = records.FirstOrDefault(p => p.Id == existingProperty.Id);
                        if (updatedProperty != null)
                        {
                            _context.Entry(existingProperty).CurrentValues.SetValues(updatedProperty);
                        }
                    }

                    var newProperties = records.Where(p => !existingProperties.Any(ep => ep.Id == p.Id)).ToList();
                    foreach (var property in newProperties)
                    {
                        _context.Entry(property).State = EntityState.Modified;
                        await _context.SaveChangesAsync();

                    }
                    await _context.SaveChangesAsync();




                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
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

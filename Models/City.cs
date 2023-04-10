using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class City
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(50)]
        public string Country { get; set; }
    }
}

using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class City
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string? Name { get; set; }

        public virtual ICollection<Property> Properties { get; set; }
    }
}

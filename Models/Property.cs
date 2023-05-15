using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        public int SellRent { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        public string PropertyTypeId { get; set; } = null!;

        public int BHK { get; set; }

        [StringLength(100)]
        public string FurnishingTypeId { get; set; } = null!;

        public int Price { get; set; }

        [StringLength(30)]
        public string Address { get; set; } = null!;


        [ForeignKey("City")]
        public int CityId { get; set; } 

        public virtual City City { get; set; } = null!;
    }
}

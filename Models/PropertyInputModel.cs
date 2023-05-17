using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class PropertyInputModel
    {
        public int SellRent { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = null!;

        [StringLength(100)]
        public string PType { get; set; } = null!;

        public int BHK { get; set; }

        [StringLength(100)]
        public string FType { get; set; } = null!;

        public int Price { get; set; }

        [StringLength(30)]
        public string Address { get; set; } = null!;

        public int City { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class PropertyInputModel
    {
        public int SellRent { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string PType { get; set; }

        public int BHK { get; set; }

        [StringLength(100)]
        public string FType { get; set; }

        public int Price { get; set; }

        [StringLength(30)]
        public string Address { get; set; }

        public int City { get; set; }
    }
}

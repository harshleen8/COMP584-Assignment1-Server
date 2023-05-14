using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class Property
    {
        [Key]
        public int Id { get; set; }

        public int SellRent { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public string PropertyTypeId { get; set; }

        public int BHK { get; set; }

        public string FurnishingTypeId { get; set; }

        public int Price { get; set; }

        public int BuiltArea { get; set; }

        public int CarpetArea { get; set; }

        [StringLength(30)]
        public string Address { get; set; }

        [StringLength(30)]
        public string Address2 { get; set; }

        public string CityId { get; set; }

        //public virtual City City { get; set; } = null;

        public int FloorNo { get; set; }

        public int TotalFloors { get; set; }

        public int ReadyToMove { get; set; }

        public string MainEntrance { get; set; }

        public string Security { get; set; }

        public int Gated { get; set; }

        public int Maintenance { get; set; }

        public string EstPossessionOn { get; set; }

        public string Age { get; set; }

        public string Description { get; set; }

        public string PostedOn { get; set; }

        public string PostedBy { get; set; }
    }
}

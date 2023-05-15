namespace WebAPI.Data
{
    public class PropertiesCsv
    {
        public int Id { get; set; }
        public int SellRent { get; set; }
        public string Name { get; set; } = null!;
        public string PropertyTypeId { get; set; } = null!;
        public int BHK { get; set; }
        public string FurnishingTypeId { get; set; } = null!;
        public int Price { get; set; }
        public string Address { get; set; } = null!;
        public int CityId { get; set; }

    }
}

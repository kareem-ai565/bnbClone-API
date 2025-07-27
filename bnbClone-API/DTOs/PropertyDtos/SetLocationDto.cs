namespace bnbClone_API.DTOs.PropertyDtos
{
    public class SetLocationDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Address { get; set; } = string.Empty;
    }
}

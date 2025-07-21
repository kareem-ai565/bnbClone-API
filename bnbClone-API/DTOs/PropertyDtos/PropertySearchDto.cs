namespace bnbClone_API.DTOs.PropertyDtos
{
    public class PropertySearchDto
    {
        public string? Location { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Guests { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? PropertyType { get; set; }
        public List<int>? AmenityIds { get; set; }
        public int? MinRating { get; set; }
        public bool? InstantBook { get; set; }
        public bool? WithImagesOnly { get; set; }
        public string? Category { get; set; }
        public string? SortBy { get; set; } // "price", "rating", "newest"
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

}

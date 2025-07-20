namespace bnbClone_API.DTOs.FavouritesDTOs
{
    public class FavouritePropertiesDTO
    {
            public int PropertyId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Country { get; set; }
            public string City { get; set; }
            public decimal PricePerNight { get; set; }
            public string Currency { get; set; }
            public string ImageUrl { get; set; } // first image
            public DateTime FavouritedAt { get; set; }

    }
}

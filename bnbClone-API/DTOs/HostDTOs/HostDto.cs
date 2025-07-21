namespace bnbClone_API.DTOs.HostDTOs
{
    public class HostDto
    {
            public int Id { get; set; }
            public int UserId { get; set; }
            public string? AboutMe { get; set; }
            public string? Work { get; set; }
            public decimal Rating { get; set; }
            public int TotalReviews { get; set; }
            public string? Education { get; set; }
            public string? Languages { get; set; }
            public bool IsVerified { get; set; }
            public decimal TotalEarnings { get; set; }
            public decimal AvailableBalance { get; set; }
            public string? StripeAccountId { get; set; }
            public string? DefaultPayoutMethod { get; set; }
            public string? PayoutAccountDetails { get; set; }
            public string? LivesIn { get; set; }
            public string? DreamDestination { get; set; }
            public string? FunFact { get; set; }
            public string? Pets { get; set; }
            public string? ObsessedWith { get; set; }
            public string? SpecialAbout { get; set; }
    }
    
}

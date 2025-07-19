using bnbClone_API.DTOs;
using bnbClone_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Services.Interfaces
{
    public interface IAmenityService
    {
        Task<IEnumerable<Amenity>> GetAmenities();
        Task<Amenity> GetAmenityById(int id);
        Task<Amenity> AddAmenity([FromForm] AmenityDTO amenity);
        Task<Amenity> EditAmenity(int id, [FromForm] AmenityDTO amenity);
        Task<Amenity> DeleteAmenity(int id);
    }
}

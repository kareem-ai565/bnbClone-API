using bnbClone_API.Data;
using bnbClone_API.DTOs;
using bnbClone_API.DTOs.PropertyDtos;
using bnbClone_API.Models;
using bnbClone_API.Repositories.Impelementations;
using bnbClone_API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bnbClone_API.Repositories.Implementations
{
    public class PropertyRepo : GenericRepo<Property>, IPropertyRepo
    {
        private readonly ApplicationDbContext _context;

        public PropertyRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<List<Property>> SearchAsync(PropertySearchDto dto)
        {
            var query = _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.Availabilities)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Bookings)
                .AsQueryable();

            if (!string.IsNullOrEmpty(dto.Location))
            {
                query = query.Where(p =>
                    p.City.Contains(dto.Location) ||
                    p.Address.Contains(dto.Location) ||
                    p.Country.Contains(dto.Location) ||
                    p.Title.Contains(dto.Location));
            }

            //if (dto.StartDate.HasValue && dto.EndDate.HasValue)
            //{
            //    var today = DateTime.Today;

            //    if (dto.StartDate.Value.Date >= today)
            //    {
            //        query = query.Where(p =>
            //            p.Bookings.Any(b =>
            //                (dto.StartDate < b.EndDate && dto.EndDate > b.StartDate)
            //            )
            //        );
            //    }
            //}

            if (dto.StartDate.HasValue && dto.EndDate.HasValue)
            {
                var start = dto.StartDate.Value.Date;
                var end = dto.EndDate.Value.Date;
                var totalDays = (end - start).Days;

                query = query.Where(p =>
                    p.Availabilities.Count(a =>
                        a.Date >= start &&
                        a.Date < end &&
                        a.IsAvailable
                    ) == totalDays
                );
            }


            if (dto.Guests.HasValue)
            {
                query = query.Where(p => p.MaxGuests >= dto.Guests);
            }

            if (dto.MinPrice.HasValue)
            {
                query = query.Where(p => p.PricePerNight >= dto.MinPrice);
            }

            if (dto.MaxPrice.HasValue)
            {
                query = query.Where(p => p.PricePerNight <= dto.MaxPrice);
            }

            if (!string.IsNullOrEmpty(dto.PropertyType))
            {
                query = query.Where(p => p.PropertyType == dto.PropertyType);
            }

            if (dto.AmenityIds?.Any() == true)
            {
                query = query.Where(p =>
                    p.PropertyAmenities.Any(pa => dto.AmenityIds.Contains(pa.AmenityId)));
            }

            if (dto.InstantBook.HasValue)
            {
                query = query.Where(p => p.InstantBook == dto.InstantBook);
            }

            if (dto.WithImagesOnly == true)
            {
                query = query.Where(p => p.PropertyImages.Any());
            }

            if (!string.IsNullOrEmpty(dto.Category))
            {
                query = query.Where(p => p.Category != null && p.Category.Name == dto.Category);
            }

            // Sorting
            if (!string.IsNullOrEmpty(dto.SortBy))
            {
                switch (dto.SortBy.ToLower())
                {
                    case "price":
                        query = query.OrderBy(p => p.PricePerNight);
                        break;
                    case "newest":
                        query = query.OrderByDescending(p => p.CreatedAt);
                        break;
                }
            }

            // Pagination
            //int skip = (dto.Page - 1) * dto.PageSize;
            //return await query.Skip(skip).Take(dto.PageSize).ToListAsync();
            return await query.ToListAsync();
        }
        public async Task<Property?> GetDetailsByIdAsync(int id)
        {
            return await _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Host)
                    .ThenInclude(h => h.User) 
                .Include(p => p.Category)
                .Include(p => p.Bookings)
                    .ThenInclude(b=>b.Review)
                        .ThenInclude(r => r.Reviewer)
                 .Include(p => p.Availabilities)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public override async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyAmenities)
                    .ThenInclude(pa => pa.Amenity)
                .Include(p => p.Availabilities)
                .Include(p => p.Host)
                    .ThenInclude(h => h.User) 
                .Include(p => p.Category)
                .Include(p => p.Bookings)
                    .ThenInclude(b => b.Review)
                        .ThenInclude(r => r.Reviewer)
                .Include(p => p.Availabilities)
                .ToListAsync();
        }

        public override async Task<Property> AddAsync(Property property)
        {
            await _context.Properties.AddAsync(property);

            return property;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var property = await _context.Properties
                .Include(p => p.PropertyImages)
                .Include(p => p.PropertyAmenities)
                .Include(p => p.Bookings)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return false;

            _context.PropertyImages.RemoveRange(property.PropertyImages);
            _context.PropertyAmenities.RemoveRange(property.PropertyAmenities);
            _context.Bookings.RemoveRange(property.Bookings); 

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();

            return true;
        }



    }
}

using bnbClone_API.DTOs.Admin;
using bnbClone_API.Services.Interfaces;
using bnbClone_API.UnitOfWork;

namespace bnbClone_API.Services.Implementations
{
    public class AdminPropertyService : IAdminPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminPropertyService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<AdminPropertyListDto>> GetAllPropertiesAsync()
        {
            var properties = await _unitOfWork.Properties.GetAllWithHostAsync();
            return properties.Select(p => new AdminPropertyListDto
            {
                Id = p.Id,
                Title = p.Title,
                PropertyType = p.PropertyType,
                City = p.City,
                Country = p.Country,
                PricePerNight = p.PricePerNight,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                HostName = $"{p.Host.User.FirstName} {p.Host.User.LastName}",
                HostEmail = p.Host.User.Email
            });
        }

        public async Task<AdminPropertyResponseDto> GetPropertyByIdAsync(int propertyId)
        {
            var property = await _unitOfWork.Properties.GetByIdWithHostAsync(propertyId);
            if (property == null) return null;

            return new AdminPropertyResponseDto
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                PropertyType = property.PropertyType,
                Country = property.Country,
                City = property.City,
                Address = property.Address,
                PricePerNight = property.PricePerNight,
                Currency = property.Currency,
                Status = property.Status,
                CreatedAt = property.CreatedAt,
                UpdatedAt = property.UpdatedAt,
                Host = new AdminHostSummaryDto
                {
                    Id = property.Host.Id,
                    FirstName = property.Host.User.FirstName,
                    LastName = property.Host.User.LastName,
                    Email = property.Host.User.Email
                }
            };
        }

        public async Task<bool> UpdatePropertyStatusAsync(int propertyId, PropertyStatusUpdateDto request)
        {
            await _unitOfWork.Properties.UpdateStatusAsync(propertyId, request.Status, request.AdminNotes);
            var result = await _unitOfWork.SaveAsync();
            return result > 0;
        }

        public async Task<bool> DeletePropertyAsync(int propertyId)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(propertyId);
            if (property == null) return false;

            _unitOfWork.Properties.Remove(property);
            var result = await _unitOfWork.SaveAsync();
            return result > 0;
        }

        public async Task<IEnumerable<AdminPropertyListDto>> GetPropertiesByStatusAsync(string status)
        {
            var properties = await _unitOfWork.Properties.GetByStatusAsync(status);
            return properties.Select(p => new AdminPropertyListDto
            {
                Id = p.Id,
                Title = p.Title,
                PropertyType = p.PropertyType,
                City = p.City,
                Country = p.Country,
                PricePerNight = p.PricePerNight,
                Status = p.Status,
                CreatedAt = p.CreatedAt,
                HostName = $"{p.Host.User.FirstName} {p.Host.User.LastName}",
                HostEmail = p.Host.User.Email
            });
        }
        // ADD THIS NEW METHOD
        public async Task<bool> SoftDeletePropertyAsync(int propertyId, string adminNotes = null)
        {
            var result = await _unitOfWork.Properties.SoftDeleteAsync(propertyId, adminNotes);
            if (!result) return false;

            var saveResult = await _unitOfWork.SaveAsync();
            return saveResult > 0;
        }


    }
}
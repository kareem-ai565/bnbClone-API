using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces.admin
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        Task<IEnumerable<Property>> GetAllWithHostAsync();
        Task<Property> GetByIdWithHostAsync(int id);
        Task<IEnumerable<Property>> GetByHostIdAsync(int hostId);
        Task<IEnumerable<Property>> GetByStatusAsync(string status);
        Task UpdateStatusAsync(int id, string status, string adminNotes = null);
    }
}

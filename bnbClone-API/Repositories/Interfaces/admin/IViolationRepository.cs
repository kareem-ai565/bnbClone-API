using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces.admin
{
    public interface IViolationRepository : IGenericRepository<Violation>
    {
        Task<IEnumerable<Violation>> GetAllWithDetailsAsync();
        Task<Violation> GetByIdWithDetailsAsync(int id);
        Task<IEnumerable<Violation>> GetByStatusAsync(string status);
        Task UpdateStatusAsync(int id, string status, string adminNotes);
    }
}

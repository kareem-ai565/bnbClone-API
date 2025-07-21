using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IViolationRepo : IGenericRepo<Violation>
    {
        Task<IEnumerable<Violation>> GetViolationsByReporterAsync(int userId);
        Task<IEnumerable<Violation>> GetViolationsByPropertyAsync(int propertyId);
        Task<IEnumerable<Violation>> GetViolationsByStatusAsync(string status);
        Task<IEnumerable<Violation>> GetViolationsWithDetailsAsync();
        Task<Violation?> GetViolationByIdWithDetailsAsync(int violationId);
    }

}

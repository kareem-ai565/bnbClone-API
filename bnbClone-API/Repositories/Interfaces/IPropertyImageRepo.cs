
using bnbClone_API.Models;

namespace bnbClone_API.Repositories.Interfaces
{
    public interface IPropertyImageRepo :IGenericRepo<PropertyImage>
    {
        Task<IEnumerable<PropertyImage>> GetImagesByPropertyIdAsync(int propertyId);


    }
}

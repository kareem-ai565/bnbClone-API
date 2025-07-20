using bnbClone_API.DTOs;
using bnbClone_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace bnbClone_API.Services.Interfaces
{
    public interface IPropertyCategoryService
    {
        Task<IEnumerable<PropertyCategory>> GetAllPropertyCategories();
        Task<PropertyCategory> GetPropertyCategoryById(int id);
        Task<PropertyCategory> DeletePropertyCategory(int id);
        Task<PropertyCategory> EditPropertyCategory(int id, [FromForm] CategoryDTO category);
        Task<PropertyCategory> AddPropertyCategory([FromForm] CategoryDTO category);
    }
}

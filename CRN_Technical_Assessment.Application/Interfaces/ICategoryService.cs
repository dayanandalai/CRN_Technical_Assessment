using CRN_Technical_Assessment.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedResponseDto<CategoryDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10); 
        Task<CategoryDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCategoryDto dto); 
        Task UpdateAsync(int id, UpdateCategoryDto dto);
        Task DeleteAsync(int id); 
        Task RestoreAsync(int id);
    }
}

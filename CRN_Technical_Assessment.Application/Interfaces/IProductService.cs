using CRN_Technical_Assessment.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Application.Interfaces
{
    public interface IProductService
    {
        Task<PagedResponseDto<ProductDto>> GetAllAsync(int pageNumber, int pageSize); 
        Task<ProductDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateProductDto dto); 
        Task UpdateAsync(int id, UpdateProductDto dto);
        Task DeleteAsync(int id); 
        Task RestoreAsync(int id);
    }
}

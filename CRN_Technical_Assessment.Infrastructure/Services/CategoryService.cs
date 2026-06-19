using AutoMapper;
using CRN_Technical_Assessment.Application.DTOs;
using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Entities;
using CRN_Technical_Assessment.Domain.Interfaces;

namespace CRN_Technical_Assessment.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResponseDto<CategoryDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var categories = await _unitOfWork.Categories.GetPagedAsync(pageNumber, pageSize);
            var allCategories = await _unitOfWork.Categories.GetAllAsync();

            var categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return new PagedResponseDto<CategoryDto>
            {
                Data = categoryDtos,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = allCategories.Count(),
                TotalPages = (int)Math.Ceiling(allCategories.Count() / (double)pageSize)
            };
        }

        public async Task<CategoryDto?> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<int> CreateAsync(CreateCategoryDto dto)
        {
            var category = new Categories
            {
                Name = dto.Name,
                CreatedBy = _currentUserService.UserName ?? "System",
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id {id} not found");

            category.Name = dto.Name;
            category.UpdatedBy = _currentUserService.UserName ?? "System";
            category.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id {id} not found");

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task RestoreAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with id {id} not found");

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

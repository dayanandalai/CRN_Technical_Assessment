using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Entities;
using CRN_Technical_Assessment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Infrastructure.Data.Repositories
{
    public class CategoryRepository
    : GenericRepository<Categories>,
      ICategoryRepository
    {
        public CategoryRepository(
            ApplicationDbContext context,
            ICurrentUserService currentUser)
            : base(context, currentUser)
        {
        }

        public async Task<Categories?> GetByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}

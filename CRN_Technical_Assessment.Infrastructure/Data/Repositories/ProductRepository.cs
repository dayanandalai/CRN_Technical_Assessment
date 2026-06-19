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
    public class ProductRepository
    : GenericRepository<Products>,
      IProductRepository
    {
        public ProductRepository(
            ApplicationDbContext context,
            ICurrentUserService currentUser)
            : base(context, currentUser)
        {
        }

        public async Task<IEnumerable<Products>>
            GetByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(x => x.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Products>>
            SearchByNameAsync(string name)
        {
            return await _context.Products
                .Where(x => x.Name.Contains(name))
                .ToListAsync();
        }
    }
}

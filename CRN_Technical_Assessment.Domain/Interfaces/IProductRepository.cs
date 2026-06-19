using CRN_Technical_Assessment.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRN_Technical_Assessment.Domain.Interfaces
{
    public interface IProductRepository
    : IGenericRepository<Products>
    {
        Task<IEnumerable<Products>> GetByCategoryAsync(int categoryId);

        Task<IEnumerable<Products>> SearchByNameAsync(string name);
    }
}

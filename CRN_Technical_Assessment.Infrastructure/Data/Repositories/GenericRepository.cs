using CRN_Technical_Assessment.Application.Interfaces;
using CRN_Technical_Assessment.Domain.Common;
using CRN_Technical_Assessment.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRN_Technical_Assessment.Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly ICurrentUserService _currentUser;

        public GenericRepository(
            ApplicationDbContext context,
            ICurrentUserService currentUser)
        {
            _context = context;
            _dbSet = context.Set<T>();
            _currentUser = currentUser;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e =>
                    EF.Property<int>(e, "Id") == id &&
                    !EF.Property<bool>(e, "IsDeleted"));
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => !EF.Property<bool>(e, "IsDeleted"))
                .ToListAsync();
        }

        public virtual async Task<IEnumerable<T>> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => !EF.Property<bool>(e, "IsDeleted"))
                .OrderBy(e => EF.Property<int>(e, "Id"))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity is BaseEntities baseEntity)
            {
                baseEntity.IsDeleted = true;
                baseEntity.DeletedDate = DateTime.UtcNow;
                baseEntity.DeletedBy = _currentUser.UserName;
            }

            _dbSet.Update(entity);
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.AnyAsync(e =>
                EF.Property<int>(e, "Id") == id &&
                !EF.Property<bool>(e, "IsDeleted"));
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
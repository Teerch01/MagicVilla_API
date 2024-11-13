using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository;

public class Repository<T>(ApplicationDbContext db) : IRepository<T> where T : class
{
    internal DbSet<T> dbSet = db.Set<T>();

    public async Task CreateAsync(T entity)
    {
        await dbSet.AddAsync(entity);
        await SaveAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0,
        int pageNumber = 1)
    {
        IQueryable<T> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (pageSize > 0)
        {
            if (pageSize > 100)
            {
                pageSize = 100;
            }
            query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            char[] separator = [','];
            foreach (var property in includeProperties
                .Split(separator: separator, options: StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;

        if (!tracked)
        {
            query = query.AsNoTracking();
        }
        if (filter != null)
        {
            query = query.Where(filter);
        }

        if (!string.IsNullOrEmpty(includeProperties))
        {
            char[] separator = [','];
            foreach (var property in includeProperties
                .Split(separator: separator, options: StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(property);
            }
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(T entity)
    {
        dbSet.Remove(entity);
        await SaveAsync();
    }

    public async Task SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}

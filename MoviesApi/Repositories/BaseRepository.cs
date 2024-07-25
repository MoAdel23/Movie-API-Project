using Microsoft.EntityFrameworkCore;
using MoviesApi.Interfaces;
using MoviesApi.Models.Data;
using System.Linq.Expressions;

namespace MoviesApi.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    private readonly ApplicationDbContext _context;
    private DbSet<T> _dbset;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbset = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? criteria = null,
        string[]? includes = null
        )
    {
        IQueryable<T> query = _dbset;

        if (criteria != null)
            query = query.Where(criteria);

        if (includes != null)
        {
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
        }

        return await query.ToListAsync();


    }
    public  async Task<IEnumerable<T>> GetAllAsync<TKey>(
         Expression<Func<T, TKey>>? orderBy = null, 
        bool ascending = true,
        Expression<Func<T, bool>>? criteria = null,
        string[]? includes = null)
    {
        IQueryable<T> query = _dbset;

        if (criteria != null)
            query = query.Where(criteria);

        if (includes != null)
        {
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
        }

        if(orderBy != null)
            query = (ascending) ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        

        return await query.ToListAsync();
    }



    public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(
      Expression<Func<T, TResult>> selector,
      Expression<Func<T, bool>>? criteria = null,
      string[]? includes = null)
    {
        IQueryable<T> query = _dbset;

        if (criteria != null)
            query = query.Where(criteria);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return await query.Select(selector).ToListAsync();
    }


    public async Task<IEnumerable<TResult>> GetAllAsync<TResult, TKey>(
        Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? criteria = null,
        Expression<Func<T, TKey>>? orderBy = null,
        bool ascending = true,
        string[]? includes = null)
    {
        IQueryable<T> query = _dbset;

        if (criteria != null)
            query = query.Where(criteria);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (orderBy != null)
        {
            query = ascending ? query.OrderBy(orderBy) : query.OrderByDescending(orderBy);
        }


        return await query.Select(selector).ToListAsync();
    }
  

 



    public async Task<T?> GetByAsync(
        Expression<Func<T, bool>> criteria,
        string[]? includes = null)
    {
        IQueryable<T> query = _dbset.Where(criteria);
        if(includes != null)
        {
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
        }

        return await query.FirstOrDefaultAsync();
    }


    public async Task<TResult?> GetByAsync<TResult>(
        Expression<Func<T, bool>> criteria,
        Expression<Func<T, TResult>> selector,
        string[]? includes = null)
    {
        IQueryable<T> query = _dbset.Where(criteria);

        if (includes != null)
        {
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
        }

        return await query.Select(selector).FirstOrDefaultAsync();
    }


    public async Task AddAsync(T entity)
    {
        await _dbset.AddAsync(entity);
    }

    public  void Remove(T entity)
    {
         _dbset.Remove(entity);
    }

    public async Task<bool> AnyAsync(Expression<Func<T, bool>> criteria)
    {
       return await _dbset.AnyAsync(criteria);
    }

    public async Task<T?> FindAsync<TParam>(TParam id)
    {
        return await _dbset.FindAsync(id);
    }
}

using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoviesApi.Interfaces;

public interface IBaseRepository<T> where T:class
{

    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? criteria = null,
        string[]? includes = null
        );

    Task<IEnumerable<T>> GetAllAsync<TKey>(
        Expression<Func<T, TKey>>? orderBy = null, 
        bool ascending = true, 
        Expression<Func<T, bool>>? criteria = null,
        string[]? includes = null);

    Task<IEnumerable<TResult>> GetAllAsync<TResult>(
       Expression<Func<T, TResult>> selector,
        Expression<Func<T, bool>>? criteria = null,
        string[]? includes = null);

    Task<IEnumerable<TResult>> GetAllAsync<TResult, TKey>(
       Expression<Func<T, TResult>> selector ,
        Expression<Func<T, bool>>? criteria = null,
        Expression<Func<T, TKey>>? orderBy=null, 
        bool ascending = true, 
        string[]? includes = null);


    Task<T?> GetByAsync(
        Expression<Func<T,bool>> criteria ,
        string[]? includes = null);
    Task<TResult?> GetByAsync<TResult>(
        Expression<Func<T, bool>> criteria, 
        Expression<Func<T, TResult>> selector, 
        string[]? includes = null);

    Task<T?> FindAsync<TParam>(TParam id);


    Task<bool> AnyAsync(Expression<Func<T, bool>> criteria);

    Task AddAsync(T entity);

    void Remove(T entity);
}

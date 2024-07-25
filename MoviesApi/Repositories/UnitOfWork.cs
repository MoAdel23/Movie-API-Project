using MoviesApi.Interfaces;
using MoviesApi.Models;
using MoviesApi.Models.Data;

namespace MoviesApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    public IGenreRepository Genre { get; private set; }
    public IMovieRepository Movie { get; private set; }
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        Genre = new GenreRepository(context);
        Movie = new MovieRepository(context);
    }

     

    public int Compelete()
    {
        return _context.SaveChanges();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
}

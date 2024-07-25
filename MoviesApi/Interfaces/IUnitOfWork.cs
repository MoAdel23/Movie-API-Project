namespace MoviesApi.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenreRepository Genre { get; }
    IMovieRepository  Movie { get; }

    int Compelete();
}

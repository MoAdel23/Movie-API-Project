using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Interfaces
{
    public interface IMovieRepository :IBaseRepository<Movie> 
    {
         void Update(Movie movie );
    }
}

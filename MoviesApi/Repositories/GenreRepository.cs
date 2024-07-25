using MoviesApi.Interfaces;
using MoviesApi.Models;
using MoviesApi.Models.Data;

namespace MoviesApi.Repositories
{
    public class GenreRepository : BaseRepository<Genre> , IGenreRepository
    {
      

        public GenreRepository(ApplicationDbContext context) : base(context)
        {
           
        }

       
    }
}

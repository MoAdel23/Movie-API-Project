using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Interfaces;
using MoviesApi.Models;
using MoviesApi.Models.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MoviesApi.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _context;
      

        public MovieRepository(ApplicationDbContext context ) : base(context)
        {
            _context = context;
        }


        public void Update(Movie movie)
        {
            _context.Movies.Update(movie);
        }
    }
}

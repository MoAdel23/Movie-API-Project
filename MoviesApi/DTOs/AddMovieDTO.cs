namespace MoviesApi.DTOs
{
    public class AddMovieDTO : MovieDTO
    {
        public IFormFile Poster { get; set; }
    }
}

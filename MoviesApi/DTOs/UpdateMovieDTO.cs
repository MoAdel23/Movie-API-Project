namespace MoviesApi.DTOs;

public class UpdateMovieDTO: MovieDTO
{
    public IFormFile? Poster { get; set; }
}



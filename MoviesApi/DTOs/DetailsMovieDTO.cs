

namespace MoviesApi.DTOs;

public class DetailsMovieDTO : MovieDTO
{
    public int Id { get; set; }
   
    public byte[]? Poster { get; set; }
   
    public string? GenreName { get; set; }
}

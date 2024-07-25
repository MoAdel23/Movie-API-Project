using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs;

public class MovieDTO
{
    public string Title { get; set; }
    public int Year { get; set; }
    public double Rate { get; set; }

    [MaxLength(2500)]
    public string StoryLine { get; set; }
  
    public byte GenreId { get; set; }
}

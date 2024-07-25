using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs;

public class GenreDTO
{
    [MaxLength(100)]
    public string Name { get; set; }
}

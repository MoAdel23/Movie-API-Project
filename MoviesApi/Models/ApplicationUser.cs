using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models;

public class ApplicationUser :IdentityUser
{
    [MaxLength(75)]
    public string FirstName { get; set; }
    [MaxLength(75)]
    public string LastName { get; set; }
}

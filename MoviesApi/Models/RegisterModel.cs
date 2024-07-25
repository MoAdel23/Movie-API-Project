using System.ComponentModel.DataAnnotations;

namespace MoviesApi.Models;

public class RegisterModel
{
    [StringLength(100)]
    public string FirstName { get; set; }
    
    [StringLength(100)]
    public string LastName { get; set; }
    
    [StringLength(50)]
    public string UserName { get; set; }
    
    [StringLength(128)]
    public string Email { get; set; }
    
    [StringLength(50)]
    public string Password { get; set; }

}

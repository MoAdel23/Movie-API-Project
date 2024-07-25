using MoviesApi.Models;

namespace MoviesApi.Interfaces;

public interface IAuthServices
{
    Task<AuthModel> RegisterAsync(RegisterModel model);
    Task<AuthModel> GetTokenAsync(LoginModel model);
    Task<string> AddroleAsync(AddRoleModel model);

}



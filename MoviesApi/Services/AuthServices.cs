using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoviesApi.Helpers;
using MoviesApi.Interfaces;
using MoviesApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MoviesApi.Services;

public class AuthServices : IAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;
    private readonly JwtOptions _jwt;

    public AuthServices(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager  ,IMapper mapper , IOptions<JwtOptions> jwt)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _jwt = jwt.Value;
    }
    public async Task<AuthModel> RegisterAsync(RegisterModel model)
    {
        if (await _userManager.FindByEmailAsync(model.Email) is not null)
            return new AuthModel { Message = "Email is already registered!"};

        if (await _userManager.FindByNameAsync(model.UserName) != null)
            return new AuthModel { Message = "UserName is already registered!" };

        // AutoMapper
        var user = _mapper.Map<ApplicationUser>(model);


        var result = await _userManager.CreateAsync(user, model.Password);

        if(!result.Succeeded)
        {
            var errors = string.Empty;

            foreach (var error in result.Errors)
            {
                errors += $"{error.Description} ,";
            }
            return new AuthModel { Message = errors};
        }

        // Add Role
        await _userManager.AddToRoleAsync(user, RolesSettings.Basic);

        JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

        return new AuthModel
        {
            UserName = user.UserName,
            Email = user.Email,
            Roles = new List<string> { RolesSettings.Basic },

            IsAuhtenticated = true,
            ExpiresOn = jwtSecurityToken.ValidTo,
            Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
        };


    }


    public async Task<AuthModel> GetTokenAsync(LoginModel model)
    {
        AuthModel authModel = new();

        var user = await _userManager.FindByEmailAsync(model.Email);

        if(user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
        {
            authModel.Message = "Email or password is incorrec!";
            return authModel;
        }

        var jwtSecurityToken  = await CreateJwtToken(user);
        var roles = await _userManager.GetRolesAsync(user);


        authModel.IsAuhtenticated = true;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        authModel.Email = user.Email;
        authModel.UserName = user.UserName;
        authModel.ExpiresOn = jwtSecurityToken.ValidTo;
        authModel.Roles = roles.ToList();

       return authModel;
    }

    public async Task<string> AddroleAsync(AddRoleModel model)
    {
        var user = await _userManager.FindByIdAsync(model.UserId);

        if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            return "Invalid user ID or Role";
        if (await _userManager.IsInRoleAsync(user, model.Role))
            return "User alredy assigned to this role!";

        var result = await _userManager.AddToRoleAsync(user, model.Role);

        return (result.Succeeded) ? string.Empty : "Something went wrong";
        


    }

    public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = new List<Claim>();
        foreach (var role in roles)
            roleClaims.Add(new Claim("roles", role));

        var claims = new Claim[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Email , user.Email),
            new Claim("uid",user.Id)

        }
        .Union(userClaims)
        .Union(roleClaims);

        SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
    
        SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey , SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.ValideIssure,
            audience: _jwt.ValideAudience,
            claims: claims,
            expires: DateTime.Now.AddHours(_jwt.DurationInHours),
            signingCredentials: signingCredentials
            ) ;

        return jwtSecurityToken;
    }

   
}

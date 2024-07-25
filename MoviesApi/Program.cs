
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoviesApi.Helpers;
using MoviesApi.Interfaces;
using MoviesApi.Models;
using MoviesApi.Models.Data;
using MoviesApi.Repositories;
using MoviesApi.Services;
using System.Text;

namespace MoviesApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.



        
        // using optional design pattern 
        builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));

        // Unit Of work
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        // AuthServices
        builder.Services.AddScoped<IAuthServices, AuthServices>();

        // AutoMapper
        builder.Services.AddAutoMapper(typeof(Program));

        // Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();


        // DbContext
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        // Authentication
        builder.Services.AddAuthentication(optios =>
        {
            optios.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            optios.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            optios.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(optios =>
        {
            optios.RequireHttpsMetadata = false;
            optios.SaveToken = true;
            optios.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = builder.Configuration["Jwt:ValideIssure"],
                ValidAudience = builder.Configuration["Jwt:ValideAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
            };
        });



        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();


        // Swagger
        builder.Services.AddSwaggerGen(options=>
        {
            options.SwaggerDoc("v1",new OpenApiInfo()
            {
                Version = "v2",
                Title = "ToDo API",
                Description = "An ASP.NET Core Web API for managing ToDo items"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
           
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();


        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

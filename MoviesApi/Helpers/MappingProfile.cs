using AutoMapper;
using MoviesApi.DTOs;
using MoviesApi.Models;

namespace MoviesApi.Helpers;

public class MappingProfile :Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, DetailsMovieDTO>();

        CreateMap<AddMovieDTO, Movie>()
            .ForMember(src => src.Poster , opt => opt.Ignore());
        
        CreateMap<UpdateMovieDTO, Movie>()
            .ForMember(src => src.Poster , opt => opt.Ignore());



        CreateMap<RegisterModel, ApplicationUser>();
           
    }
}

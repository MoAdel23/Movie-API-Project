using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs;
using MoviesApi.Interfaces;
using MoviesApi.Models;

namespace MoviesApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController:ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly List<string> _allowedExtenstions = [".jpg", ".jpeg", ".png"];
    private readonly long _maxAllowedPosterSize = 4096;

    public MoviesController(IUnitOfWork unitOfWork , IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        
        var movies = await _unitOfWork.Movie.GetAllAsync(
            orderBy:x => x.Rate,
            ascending:false,
            includes: [nameof(Genre)]
            //selector: x => new DetailsMovieDTO()
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Year = x.Year,
            //    Rate = x.Rate,
            //    StoryLine = x.StoryLine,
            //    Poster = x.Poster,
            //    GenreId = x.GenreId,
            //    GenreName = x.Genre.Name
            //}
            );


        var dtos = _mapper.Map<IEnumerable<DetailsMovieDTO>>(movies);
        return Ok(dtos);
    }

    [HttpGet("GetByGenreId")]
    public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
    {
        var movies = await _unitOfWork.Movie.GetAllAsync(
            criteria: x => x.GenreId == genreId,
            includes: [nameof(Genre)]
            //selector:x => new DetailsMovieDTO()
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Year = x.Year,
            //    Rate = x.Rate,
            //    StoryLine = x.StoryLine,
            //    Poster = x.Poster,
            //    GenreId = x.GenreId,
            //    GenreName = x.Genre.Name
            //}
            );

        if (movies is null)
            return NotFound();

        var dtos = _mapper.Map<IEnumerable<DetailsMovieDTO>>(movies);

        return Ok(dtos);
    }

    [HttpGet("{id}",Name = "GetMovieById")]
    public async Task<IActionResult> GetByAsync(int id)
    {
        var movie = await _unitOfWork.Movie.GetByAsync(
            criteria: m => m.Id == id ,
            includes:[nameof(Genre)]
            //selector: x => new DetailsMovieDTO()
            //{
            //    Id = x.Id,
            //    Title = x.Title,
            //    Year = x.Year,
            //    Rate = x.Rate,
            //    StoryLine = x.StoryLine,
            //    Poster = x.Poster,
            //    GenreId = x.GenreId,
            //    GenreName = x.Genre.Name 
            //}
            );

        if (movie is null)
            return NotFound($"No movie was found with ID : {id}");

        var dto = _mapper.Map<DetailsMovieDTO>(movie);


        return Ok(dto);

    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromForm]AddMovieDTO dto)
    {
        if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
            return BadRequest($"Only [.jpg | .jpeg | .png ] images are alloewed!!");

        if (dto.Poster.Length > _maxAllowedPosterSize)
            return BadRequest($"Max allowed size for postr is 4MB");

       
        using var dataStream = new MemoryStream();
        await dto.Poster.CopyToAsync(dataStream);

        var isValidGenre = await _unitOfWork.Genre.AnyAsync(g => g.Id == dto.GenreId);
        if (!isValidGenre)
            return BadRequest($"Invalid genre ID!");

        //
        var movie = _mapper.Map<Movie>(dto);
        movie.Poster = dataStream.ToArray();

       await _unitOfWork.Movie.AddAsync(movie);
        _unitOfWork.Compelete();

        var url = Url.Link("GetMovieById", new { movie.Id });
        return Created(url, movie);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id , [FromForm]UpdateMovieDTO dto)
    {
        
        var movie = await _unitOfWork.Movie.FindAsync(id);
        if (movie is null)
            return NotFound($"No movie was found with ID : {id}");


        var isValidGenre = await _unitOfWork.Genre.AnyAsync(g => g.Id == dto.GenreId);
        if (!isValidGenre)
            return BadRequest($"Invalid genre ID!");

        movie = _mapper.Map<Movie>(dto);
        if(dto.Poster != null)
        {
            if (!_allowedExtenstions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                return BadRequest($"Only [.jpg | .jpeg | .png ] images are alloewed!!");

            if (dto.Poster.Length > _maxAllowedPosterSize)
                return BadRequest($"Max allowed size for postr is 4MB");

            using var dataStream = new MemoryStream();
            await dto.Poster.CopyToAsync(dataStream);
            movie.Poster = dataStream.ToArray();
        }

       
        
         _unitOfWork.Movie.Update(movie);

        _unitOfWork.Compelete();

        return Ok(movie);

    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var movie = await _unitOfWork.Movie.FindAsync(id);

        if (movie is null)
            return NotFound($"No movie was found with ID : {id}");

        _unitOfWork.Movie.Remove(movie);

        _unitOfWork.Compelete();

        return Ok(movie);
            
    }
}

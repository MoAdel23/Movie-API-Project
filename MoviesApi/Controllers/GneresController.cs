using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs;
using MoviesApi.Interfaces;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController ]
    [Authorize]
    public class GenresController:ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenresController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {
            var geres = await _unitOfWork.Genre.GetAllAsync();
            
            if (geres == null)
                return NotFound();
            return Ok(geres);
        }

        [HttpGet("{id:int}",Name ="GetGenreById")]
        public async Task<IActionResult> GetByAsync(int id)
        {
            var genre = await _unitOfWork.Genre.GetByAsync(g => g.Id == id);
            
            if (genre == null)
                return NotFound($"No gere found with ID : {id}");

            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsnc(GenreDTO dto)
        {
           

            var isFound =await _unitOfWork.Genre.AnyAsync(g => g.Name == dto.Name);
            if (isFound)
                return BadRequest("Name of genre is already exists!");


            var genre = new Genre()
            {
                Name = dto.Name
            };
            await _unitOfWork.Genre.AddAsync(genre);
            _unitOfWork.Compelete();

            var url = Url.Link("GetGenreById", new { genre.Id });

            return Created(url, genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]GenreDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genre = await _unitOfWork.Genre.GetByAsync(g => g.Id == id);
            if (genre is null)
                return NotFound($"No gere found with ID : {id}");

            genre.Name = dto.Name;

            _unitOfWork.Compelete();

            return Ok(genre);


        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var genre = await _unitOfWork.Genre.GetByAsync(g => g.Id == id);

            if (genre == null)
                return NotFound($"No gere found with ID : {id}");

            _unitOfWork.Genre.Remove(genre);
            _unitOfWork.Compelete();

            return Ok(genre);
        }

    }
}

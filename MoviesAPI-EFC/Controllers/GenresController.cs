using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.Genres;
using MoviesAPI_EFC.Entities;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController: CustomBaseController
    {
        private readonly ApplicationDbContext _moviesDbContext;
        private readonly IMapper _mapper;

        public GenresController(ApplicationDbContext moviesDbContext, IMapper mapper): base(moviesDbContext, mapper)
        {
            _moviesDbContext = moviesDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await Get<Genre, GenreListItemDTO>()); 
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GenreListItemDTO>> GetById(int id)
        {
            return await Get<Genre, GenreListItemDTO>(id);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenreCreateReqDTO genreCreateReqDTO )
        {
            var genre = await _moviesDbContext.Genres.AddAsync(_mapper.Map<Genre>(genreCreateReqDTO));
            await _moviesDbContext.SaveChangesAsync();    
            return StatusCode(201, _mapper.Map<GenreListItemDTO>(genre.Entity));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id ,[FromBody] GenreUpdateReqDTO genreUpdateReqDTO)
        {
            var genreInDB = await _moviesDbContext.Genres.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (genreInDB == default) return NotFound("Resource not found");
            _mapper.Map(genreUpdateReqDTO, genreInDB);
            await _moviesDbContext.SaveChangesAsync();
            return StatusCode(201, _mapper.Map<GenreListItemDTO>(genreInDB));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Genre genreInDB = await _moviesDbContext.Genres.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (genreInDB == default) return NotFound("Resource not found");

            _moviesDbContext.Remove(genreInDB);
            _moviesDbContext.SaveChangesAsync();

            return Ok(id);
        }
    }
}

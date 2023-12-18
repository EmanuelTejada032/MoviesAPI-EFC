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
    public class GenresController: ControllerBase
    {
        private readonly ApplicationDbContext _moviesDbContext;
        private readonly IMapper _mapper;

        public GenresController(ApplicationDbContext moviesDbContext, IMapper mapper)
        {
            _moviesDbContext = moviesDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _moviesDbContext.Genres.ProjectTo<GenreListItemDTO>(_mapper.ConfigurationProvider).ToListAsync()); 
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            GenreListItemDTO genre = await _moviesDbContext.Genres.Where(x => x.Id == id).ProjectTo<GenreListItemDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if(genre == default) return NotFound("Resource not found");
            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GenreCreateReqDTO genreCreateReqDTO )
        {
           var genreInDB = await _moviesDbContext.Genres.AddAsync(_mapper.Map<Genre>(genreCreateReqDTO));
            await _moviesDbContext.SaveChangesAsync();    
            return StatusCode(201, _mapper.Map<GenreListItemDTO>(genreInDB.Entity));
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

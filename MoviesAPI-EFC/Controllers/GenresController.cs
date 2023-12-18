using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs;

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
    }
}

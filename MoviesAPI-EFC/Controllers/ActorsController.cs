using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.Actors;
using MoviesAPI_EFC.Entities;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _moviesDbContext;
        private readonly IMapper _mapper;

        public ActorsController(ApplicationDbContext moviesDbContext, IMapper mapper)
        {
            _moviesDbContext = moviesDbContext;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok( await _moviesDbContext.Actors.ProjectTo<ActorListItemResponseDTO>(_mapper.ConfigurationProvider).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            ActorListItemResponseDTO actor = await _moviesDbContext.Actors.Where(x => x.Id == id).ProjectTo<ActorListItemResponseDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (actor == default) return NotFound("Resource not found");
            return Ok(actor);
        }

       
    }
}

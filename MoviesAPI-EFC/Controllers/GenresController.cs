using AutoMapper;
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
        public async Task<ActionResult<GenreListItemDTO>> Post([FromBody] GenreCreateReqDTO genreCreateReqDTO )
        {
            return await Post<Genre, GenreCreateReqDTO, GenreListItemDTO>(genreCreateReqDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<GenreListItemDTO>> Put(int id ,[FromBody] GenreUpdateReqDTO genreUpdateReqDTO)
        {
            return await Put<Genre, GenreUpdateReqDTO, GenreListItemDTO>(id, genreUpdateReqDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            return await Delete<Genre>(id);
        }
    }
}

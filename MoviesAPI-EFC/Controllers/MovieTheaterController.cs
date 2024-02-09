using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI_EFC.DTOs.MovieTheater;
using MoviesAPI_EFC.Entities;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieTheaterController:CustomBaseController
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public MovieTheaterController(ApplicationDbContext applicationDbContext, IMapper mapper):base(applicationDbContext, mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>  Ok(await Get<MovieTheater, MovieTheaterListItemResDTO>());

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieTheaterListItemResDTO>> Get(int id) => await Get<MovieTheater, MovieTheaterListItemResDTO>(id);

        [HttpPost]
        public async Task<ActionResult<MovieTheaterListItemResDTO>> Post([FromBody] MovieTheaterCreateReqDTO movieTheaterCreateResDTO)
        {
            return await Post<MovieTheater, MovieTheaterCreateReqDTO, MovieTheaterListItemResDTO>(movieTheaterCreateResDTO);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<MovieTheaterListItemResDTO>> Put(int id , [FromBody] MovieTheaterUpdateReqDTO movieTheaterUpdateReqDTO)
        {
            return await Put<MovieTheater, MovieTheaterUpdateReqDTO, MovieTheaterListItemResDTO>(id, movieTheaterUpdateReqDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            return await Delete<MovieTheater>(id);
        }


    }
}

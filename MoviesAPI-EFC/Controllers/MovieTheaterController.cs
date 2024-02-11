using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.MovieTheater;
using MoviesAPI_EFC.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "IsAdmin")]
    public class MovieTheaterController:CustomBaseController
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly GeometryFactory _geometryFactory;

        public MovieTheaterController(ApplicationDbContext applicationDbContext, IMapper mapper, GeometryFactory geometryFactory ):base(applicationDbContext, mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _geometryFactory = geometryFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get() =>  Ok(await Get<MovieTheater, MovieTheaterListItemResDTO>());


        [HttpGet("closest")]
        public async Task<IActionResult> Get([FromQuery] MovieTheaterFilterReqDTO movieTheaterFilterReqDTO)
        {
            var Location = _geometryFactory.CreatePoint(new Coordinate(movieTheaterFilterReqDTO.Longitude, movieTheaterFilterReqDTO.Latitude));
            var movieTheaters = await _applicationDbContext.MovieTheaters.OrderBy(x => x.Location.Distance(Location))
                .Where(x => x.Location.IsWithinDistance( Location ,movieTheaterFilterReqDTO.MovieTheaterWithinDistanceInKms * 1000))
                .Select(x => new MovieTheaterListCLosestItemResDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    Latitude = x.Location.Y,
                    Longitude = x.Location.X,
                    DistanceInMetters = x.Location.Distance(Location)
                })
                .ToListAsync();

            return Ok(movieTheaters);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieTheaterListItemResDTO>> Get(int id) => await Get<MovieTheater, MovieTheaterListItemResDTO>(id);

        [HttpPost]
        public async Task<ActionResult<MovieTheaterListItemResDTO>> Post([FromBody] MovieTheaterCreateReqDTO movieTheaterCreateResDTO) => await Post<MovieTheater, MovieTheaterCreateReqDTO, MovieTheaterListItemResDTO>(movieTheaterCreateResDTO);

        [HttpPut("{id:int}")]
        public async Task<ActionResult<MovieTheaterListItemResDTO>> Put(int id , [FromBody] MovieTheaterUpdateReqDTO movieTheaterUpdateReqDTO) => await Put<MovieTheater, MovieTheaterUpdateReqDTO, MovieTheaterListItemResDTO>(id, movieTheaterUpdateReqDTO);

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<int>> Delete(int id) => await Delete<MovieTheater>(id);

    }
}

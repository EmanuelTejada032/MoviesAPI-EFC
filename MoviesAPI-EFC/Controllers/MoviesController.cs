using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.Movies;
using MoviesAPI_EFC.Entities;
using MoviesAPI_EFC.Extensions;
using MoviesAPI_EFC.Services.Contract;
using System.Linq.Dynamic.Core;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController: CustomBaseController
    {
        private readonly ApplicationDbContext _moviesDbContext;
        private readonly IMapper _mapper;
        private readonly IFileManager _fileManager;
        private readonly ILogger<MoviesController> _logger;
        private readonly string _CONTAINER = "movies";

        public MoviesController(ApplicationDbContext moviesDbContext ,IMapper mapper, IFileManager fileManager, ILogger<MoviesController> logger)
        {
            _moviesDbContext = moviesDbContext;
            _mapper = mapper;
            _fileManager = fileManager;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] MoviesFilterDTO filterData)
        {
            var resourceQueryable = _moviesDbContext.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(filterData.Title))
            {
                resourceQueryable = resourceQueryable.Where(x => x.Title.Contains(filterData.Title));
            }
            
            if(filterData.OnTheaters != null)
            {
                resourceQueryable = resourceQueryable.Where(x => x.OnTheaters == filterData.OnTheaters);
            }

            if (!string.IsNullOrEmpty(filterData.FieldToFilter))
            {
                var orderByValue = filterData.FilterDescending ? "descending" : "ascending";

                try
                {
                    resourceQueryable = resourceQueryable.OrderBy($"{filterData.FieldToFilter} {orderByValue}");
                }catch (Exception ex)
                {
                    _logger.LogCritical("Error when filtering dynamicly");
                }
            }

            var paginatedActors = await resourceQueryable.ProjectTo<MovieListItemResponseDTO>(_mapper.ConfigurationProvider)
                .Paginate(filterData).ToListAsync();

            return Ok(paginatedActors);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _moviesDbContext.Movies
                .Include(x => x.MoviesGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .Where(x => x.Id == id).ProjectTo<MovieDetailDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();

            if (movie == default) return NotFound("Resource not found");
            return Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] MovieCreateReqDTO movieCreateReqDTO)
        {
            Movie movie = _mapper.Map<Movie>(movieCreateReqDTO);
            if (movieCreateReqDTO.Poster != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieCreateReqDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreateReqDTO.Poster.FileName);
                    movie.Poster = await _fileManager.UploadFile(content, extension, _CONTAINER, movieCreateReqDTO.Poster.ContentType);
                }
            }

            _moviesDbContext.AddAsync(movie);
            await _moviesDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<MovieListItemResponseDTO>(movie));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromForm] MovieUpdateRequestDTO movieUpdateRequestDTO)
        {

            Movie movieInDb = await _moviesDbContext.Movies.Include(x => x.MoviesGenres).Include(x => x.MoviesActors).Where(x => x.Id == id).FirstOrDefaultAsync();
            if (movieInDb == default) return NotFound("Resource not found");

            if (movieUpdateRequestDTO.Poster!= default)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await movieUpdateRequestDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieUpdateRequestDTO.Poster.FileName);

                    if (movieInDb != null)
                    {
                        movieInDb.Poster = await _fileManager.EditFile(content, extension, _CONTAINER, movieUpdateRequestDTO.Poster.ContentType, movieInDb.Poster);
                    }
                    else
                    {
                        movieInDb.Poster = await _fileManager.UploadFile(content, extension, _CONTAINER, movieUpdateRequestDTO.Poster.ContentType);
                    }
                }
            }

            _mapper.Map(movieUpdateRequestDTO, movieInDb);
            await _moviesDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<MovieListItemResponseDTO>(movieInDb));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Movie movie = await _moviesDbContext.Movies.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (movie == default) return NotFound("Resource not found");

            var poster = movie.Poster;

            _moviesDbContext.Remove(movie);
            await _moviesDbContext.SaveChangesAsync();

            if (poster != null)
            {
                await _fileManager.DeleteFile(poster, _CONTAINER);
            }

            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> jsonPatchDocumentMoviePatchDTO)
        {
            return await Patch<Movie, MoviePatchDTO>(id, jsonPatchDocumentMoviePatchDTO);
        }
    }
}

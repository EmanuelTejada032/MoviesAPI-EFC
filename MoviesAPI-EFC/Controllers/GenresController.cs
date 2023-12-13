using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController: ControllerBase
    {
        private readonly ApplicationDbContext _moviesDbContext;

        public GenresController(ApplicationDbContext moviesDbContext)
        {
            _moviesDbContext = moviesDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _moviesDbContext.Genres.ToListAsync()); 
        }
    }
}

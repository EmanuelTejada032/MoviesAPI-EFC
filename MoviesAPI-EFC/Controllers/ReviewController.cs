using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.General;
using MoviesAPI_EFC.DTOs.Review;
using MoviesAPI_EFC.Entities;
using MoviesAPI_EFC.Helpers;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/movies/{movieId:int}/reviews")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ServiceFilter(typeof(MovieExistAttribute))]
    public class ReviewController : CustomBaseController
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public ReviewController(ApplicationDbContext applicationDbContext, IMapper mapper) : base(applicationDbContext, mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<ReviewListItemDTO>>> Get(int movieId, [FromQuery]PaginationData paginationData) 
        {
            var reviewsQueryable = _applicationDbContext.Reviews.Include(x => x.User).AsQueryable();
            reviewsQueryable.Where(x => x.MovieId == movieId);
            return await Get<Review, ReviewListItemDTO>(paginationData, reviewsQueryable);
        }

        [HttpPost]
        public async Task<IActionResult> Post(int movieId, ReviewCreateReqDTO reviewCreateReqDTO )
        {
            var userid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userid").Value;
            var reviewExist = await _applicationDbContext.Reviews.AnyAsync(x => x.MovieId == movieId && x.UserId == userid);

            if (reviewExist) return BadRequest("User already reviewed this movie");

            var review = _mapper.Map<Review>(reviewCreateReqDTO);
            review.UserId = userid;
            review.MovieId = movieId;

            await _applicationDbContext.AddAsync(review);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(review.Id);
        }

        [HttpPut("{reviewId:int}")]
        public async Task<IActionResult> Put(int movieId, int reviewId, ReviewUpdateReqDTO reviewUpdateReqDTO)
        {
            Review review = await _applicationDbContext.Reviews.FirstOrDefaultAsync(x => x.MovieId == movieId && x.Id == reviewId);
            if (review == default) return NotFound("Review not found");

            var userid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userid").Value;

            if(review.UserId != userid) return BadRequest("Unauthorized to manipulate resource");

            _mapper.Map(reviewUpdateReqDTO, review);
            await _applicationDbContext.SaveChangesAsync();

            return Ok(review.Id);
        }

        [HttpDelete("{reviewId:int}")]
        public async Task<ActionResult<int>> Delete(int movieId, int reviewId)
        {
            Review review = await _applicationDbContext.Reviews.FirstOrDefaultAsync(x => x.MovieId == movieId && x.Id == reviewId);
            if (review == default) return NotFound("Review not found");

            var userid = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userid").Value;

            if (review.UserId != userid) return BadRequest("Unauthorized to manipulate resource");

            return await Delete<Review>(reviewId);
        }



    }
}

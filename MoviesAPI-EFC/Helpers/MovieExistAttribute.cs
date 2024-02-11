using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI_EFC.Helpers
{
    public class MovieExistAttribute : Attribute, IAsyncResourceFilter
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public MovieExistAttribute(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var movieIdRouteValue = context.HttpContext.Request.RouteValues["movieId"];
            if (movieIdRouteValue == default || movieIdRouteValue == null) return;

            var movieId = int.Parse(movieIdRouteValue.ToString());

            var movie = await _applicationDbContext.Movies.AnyAsync(x => x.Id == movieId);
            if (movie == default)
            {
                context.Result =  new NotFoundResult();
            }
            else
            {
                await next();
            }
        }
    }
}

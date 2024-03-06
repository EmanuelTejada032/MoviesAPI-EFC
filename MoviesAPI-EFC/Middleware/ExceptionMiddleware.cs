using MoviesAPI_EFC.Helpers;
using System.Text.Json;

namespace MoviesAPI_EFC.Middleware
{
        public class ExceptionMiddleware
        {
            private readonly RequestDelegate _next;

            public ExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext httpContext)
            {
                try
                {
                    await _next(httpContext);
                }
                catch (Exception ex)
                {
                    if (ex is ValidationErrors valError)
                    {
                        httpContext.Response.ContentType = "application/json";
                        httpContext.Response.StatusCode = 400;
                        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(valError.Errors));
                    }
                    else
                        throw;
                }
            }
        }

}

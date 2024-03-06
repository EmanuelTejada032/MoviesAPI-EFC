using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using MoviesAPI_EFC.Entities;

namespace MoviesAPI_EFC.Filters
{
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<CustomExceptionFilter> logger;

        public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger, ApplicationDbContext applicationDbContext)
        {
            this.logger = logger;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            await CustomLogger.Log(new CustomExceptionFilterLogDTO { ErrorStackTrace = context.Exception.StackTrace});
            base.OnExceptionAsync(context);
        }
    }

    public class CustomExceptionFilterLogDTO
    {
        public string ErrorLocation { get; set; } = string.Empty;
        public string ErrorDescription { get; set; } = "Exception Log";
        public string? ErrorStackTrace { get; set; }
        public string ErrorType { get; set; } = "Exception error";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    public class CustomLogger
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private static IServiceProvider _serviceProvider = default;

        public CustomLogger( ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        public static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private static IMapper GetMapper()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMapper>();
        }

        private static ApplicationDbContext GetContext()
        {
            var scope = _serviceProvider.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        public static async Task Log(CustomExceptionFilterLogDTO exceptionData)
        {
            var mapper = GetMapper();
            var context = GetContext();
            CustomExceptionFilterLog customLog = mapper.Map<CustomExceptionFilterLog>(exceptionData);
            await context.AddAsync(customLog);
            await context.SaveChangesAsync();
        }
    }
}

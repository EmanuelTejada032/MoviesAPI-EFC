using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.Services.Implementation;

namespace MoviesAPI_EFC.Services.Contract
{
    public class RecurrentService: IRecurrentService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public RecurrentService(ApplicationDbContext context, ILogger<RecurrentService> logger)
        {
            _context = context;
            _logger = logger;
        }

    }
}

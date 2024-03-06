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

        public async Task Recurrent()
        {
            try
            {
                var logsCount = await _context.CustomExceptionFilterLogs.CountAsync();

                int[] numbers = {
                        1, 2, 3, 4, 5, 6, 7, 8, 9, 10,11,12
                };

                _logger.LogInformation($"New index is {logsCount}");
                var value = numbers[logsCount];

                _logger.LogInformation($"its value is {value}");
            }
            catch (Exception ex)
            {
                throw;
            }
          
        }

    }
}

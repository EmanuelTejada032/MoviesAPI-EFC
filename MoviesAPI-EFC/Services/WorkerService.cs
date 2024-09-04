using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.Services.Contract;
using MoviesAPI_EFC.Services.Implementation;
using System.Threading;

namespace MoviesAPI_EFC.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;
        private readonly IServiceProvider _serviceProvider;

        public WorkerService(ILogger<WorkerService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at a{DateTimeOffset.Now}");
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

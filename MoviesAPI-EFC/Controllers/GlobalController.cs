using Microsoft.AspNetCore.Mvc;
using MoviesAPI_EFC.Services.Contract;
using MoviesAPI_EFC.Services.Implementation;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GlobalController:ControllerBase
    {
        private readonly IRecurrentService _recurrentService;

        public GlobalController(IRecurrentService recurrentService)
        {
            _recurrentService = recurrentService;
        }

        [HttpPost(nameof(ScheduleJob))]
        public async Task<IActionResult> ScheduleJob() 
        {
            
            return Ok();
        }
    }
}

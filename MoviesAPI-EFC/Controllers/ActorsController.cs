using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.Actors;
using MoviesAPI_EFC.Entities;
using MoviesAPI_EFC.Services.Contract;
using System.Runtime.CompilerServices;

namespace MoviesAPI_EFC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext _moviesDbContext;
        private readonly IMapper _mapper;
        private readonly IFileManager _fileManagerService;
        private readonly string CONTAINER = "actors";

        public ActorsController(ApplicationDbContext moviesDbContext, IMapper mapper, IFileManager fileManagerService)
        {
            _moviesDbContext = moviesDbContext;
            _mapper = mapper; 
            _fileManagerService = fileManagerService;

        }


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok( await _moviesDbContext.Actors.ProjectTo<ActorListItemResponseDTO>(_mapper.ConfigurationProvider).ToListAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            ActorListItemResponseDTO actor = await _moviesDbContext.Actors.Where(x => x.Id == id).ProjectTo<ActorListItemResponseDTO>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
            if (actor == default) return NotFound("Resource not found");
            return Ok(actor);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] ActorCreateReqDTO actorCreateReqDTO)
        {
            Actor actor = _mapper.Map<Actor>(actorCreateReqDTO); 
            if(actorCreateReqDTO.profilepicture != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorCreateReqDTO.profilepicture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorCreateReqDTO.profilepicture.FileName);
                    actor.profilepicture = await _fileManagerService.UploadFile(content, extension, CONTAINER, actorCreateReqDTO.profilepicture.ContentType);
                }
            }

            _moviesDbContext.AddAsync(actor);
            await _moviesDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ActorListItemResponseDTO>(actor));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id , [FromForm] ActorUpdateReqDTO actorUpdateReqDTO)
        {

            Actor actorInDB = await _moviesDbContext.Actors.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (actorInDB == default) return NotFound("Resource not found");

            if (actorUpdateReqDTO.profilepicture != default)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await actorUpdateReqDTO.profilepicture.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(actorUpdateReqDTO.profilepicture.FileName);

                    if(actorInDB != null)
                    {
                        actorInDB.profilepicture = await _fileManagerService.EditFile(content, extension, CONTAINER, actorUpdateReqDTO.profilepicture.ContentType, actorInDB.profilepicture);
                    }
                    else
                    {
                        actorInDB.profilepicture = await _fileManagerService.UploadFile(content, extension, CONTAINER, actorUpdateReqDTO.profilepicture.ContentType);
                    }
                }
            }

            _mapper.Map(actorUpdateReqDTO, actorInDB);
            await _moviesDbContext.SaveChangesAsync();

            return Ok(_mapper.Map<ActorListItemResponseDTO>(actorInDB));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            Actor actor = await _moviesDbContext.Actors.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (actor == default) return NotFound("Resource not found");

            var profilePicture = actor.profilepicture;

            _moviesDbContext.Remove(actor);
            await _moviesDbContext.SaveChangesAsync();

            if(profilePicture != null)
            {
                await _fileManagerService.DeleteFile(profilePicture, CONTAINER);
            }


            return Ok();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> Patch(int id , [FromBody] JsonPatchDocument<ActorPatchDTO> actorPatchData )
        {
            if (actorPatchData == default) return BadRequest();

            var ActorInDB = await _moviesDbContext.Actors.Where(x => x.Id == id).FirstOrDefaultAsync();

            if(ActorInDB == default) return NotFound("Resource not found");

            var actorPatchDTO = _mapper.Map<ActorPatchDTO>(ActorInDB);

            actorPatchData.ApplyTo(actorPatchDTO, ModelState);

            var isValid = TryValidateModel(actorPatchData);

            if (!isValid) return BadRequest(ModelState);

            _mapper.Map(actorPatchDTO, ActorInDB);
            await _moviesDbContext.SaveChangesAsync();

            return Ok();
        }


    }
}

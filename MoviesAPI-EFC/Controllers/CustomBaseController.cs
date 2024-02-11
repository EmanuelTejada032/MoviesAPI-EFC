using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.General;
using MoviesAPI_EFC.DTOs.Interfaces;
using MoviesAPI_EFC.Extensions;

namespace MoviesAPI_EFC.Controllers
{
    public class CustomBaseController:ControllerBase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;

        public CustomBaseController(ApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
        }

        protected async Task<List<TDTO>> Get<TEntity, TDTO>() where TEntity : class
        {
            var entitiesInDB = await _applicationDbContext.Set<TEntity>().ToListAsync();
            var mappedEntities = _mapper.Map<List<TDTO>>(entitiesInDB);
            return mappedEntities;
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IId
        {
            var entityInDb = await _applicationDbContext.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entityInDb == default) return NotFound();
            var mappedEntity = _mapper.Map<TDTO>(entityInDb);
            return Ok(mappedEntity);
        }

        protected async Task<ActionResult<List<TDTO>>> Get<TEntity, TDTO>(PaginationData paginationData) where TEntity: class
        {
            var entityQueryable = _applicationDbContext.Set<TEntity>().AsQueryable();
            return Ok(Get<TEntity, TDTO>(paginationData, entityQueryable));
        }

        protected async Task<ActionResult<List<TDTO>>> Get<TEntity, TDTO>(PaginationData paginationData, IQueryable<TEntity> queryable) where TEntity : class
        {
            var paginatedItems = await queryable.Paginate(paginationData).ToListAsync();
            return Ok(_mapper.Map<List<TDTO>>(paginatedItems));
        }

        protected async Task<ActionResult<TRespDTO>> Post<TEntity, TReqDTO, TRespDTO>(TReqDTO reqDTO) where TEntity : class where TReqDTO : class 
        {
            var entityInDb = await _applicationDbContext.Set<TEntity>().AddAsync(_mapper.Map<TEntity>(reqDTO));
            await _applicationDbContext.SaveChangesAsync();
            return _mapper.Map<TRespDTO>(entityInDb.Entity);
        }

        protected async Task<ActionResult<TRespDTO>> Put<TEntity, TReqDTO, TRespDTO>(int id, TReqDTO reqDTO) where TEntity: class, IId where TReqDTO : class
        {
            var entityInDb = await _applicationDbContext.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entityInDb == default) return NotFound("Resource not found");
            _mapper.Map(reqDTO, entityInDb);
            await _applicationDbContext.SaveChangesAsync();
            return _mapper.Map<TRespDTO>(entityInDb);
        } 


        protected async Task<ActionResult<int>> Delete<TEntity>(int id) where TEntity : class, IId
        {
            var entityInDb = await _applicationDbContext.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entityInDb == default) return NotFound("Resource not found");
            _applicationDbContext.Remove(entityInDb);
            await _applicationDbContext.SaveChangesAsync();
            return Ok(id);
        }

        protected async Task<ActionResult> Patch<TEntity, TPatchReqDTO>(int id,JsonPatchDocument<TPatchReqDTO> patchReqDTO) where TEntity : class, IId where TPatchReqDTO : class
        {
            if (patchReqDTO == default) return BadRequest();

            var entityInDb = await _applicationDbContext.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();

            if (entityInDb == default) return NotFound("Resource not found");

            var actorPatchDTO = _mapper.Map<TPatchReqDTO>(entityInDb);

            patchReqDTO.ApplyTo(actorPatchDTO, ModelState);

            var isValid = TryValidateModel(patchReqDTO);

            if (!isValid) return BadRequest(ModelState);

            _mapper.Map(actorPatchDTO, entityInDb);
            await _applicationDbContext.SaveChangesAsync();

            return Ok();
        }

    }
}

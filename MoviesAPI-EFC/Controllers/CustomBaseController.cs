using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI_EFC.DTOs.Interfaces;

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
            var entities = await _applicationDbContext.Set<TEntity>().ToListAsync();
            var mappedEntities = _mapper.Map<List<TDTO>>(entities);
            return mappedEntities;
        }

        protected async Task<ActionResult<TDTO>> Get<TEntity, TDTO>(int id) where TEntity : class, IId
        {
            var entity = await _applicationDbContext.Set<TEntity>().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (entity == default) return NotFound();
            var mappedEntity = _mapper.Map<TDTO>(entity);
            return Ok(mappedEntity);
        }

    }
}

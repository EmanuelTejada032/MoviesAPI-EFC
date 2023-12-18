using AutoMapper;
using MoviesAPI_EFC.DTOs;
using MoviesAPI_EFC.Entities;

namespace MoviesAPI_EFC
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreListItemDTO>();
        }
    }
}

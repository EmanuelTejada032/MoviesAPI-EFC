using AutoMapper;
using MoviesAPI_EFC.DTOs.Genres;
using MoviesAPI_EFC.Entities;

namespace MoviesAPI_EFC
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Genre, GenreListItemDTO>();
            CreateMap<GenreCreateReqDTO, Genre>();
            CreateMap<GenreUpdateReqDTO, Genre>()
                .ForMember(src => src.Id, opt => opt.Ignore());
        }
    }
}

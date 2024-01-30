﻿using AutoMapper;
using MoviesAPI_EFC.DTOs.Actors;
using MoviesAPI_EFC.DTOs.Genres;
using MoviesAPI_EFC.DTOs.Movies;
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



            CreateMap<Actor, ActorListItemResponseDTO>();
            CreateMap<ActorCreateReqDTO, Actor>()
                .ForMember(src => src.profilepicture, opt => opt.Ignore());

            CreateMap<ActorUpdateReqDTO, Actor>()
               .ForMember(src => src.profilepicture, opt => opt.Ignore());

            CreateMap<ActorPatchDTO, Actor>().ReverseMap();


            CreateMap<Movie, MovieListItemResponseDTO>();

            CreateMap<MovieCreateReqDTO, Movie>()
               .ForMember(src => src.Poster, opt => opt.Ignore());

            CreateMap<MovieUpdateRequestDTO, Movie>()
               .ForMember(src => src.Poster, opt => opt.Ignore());

            CreateMap<MoviePatchDTO, Movie>().ReverseMap();

        }
    }
}

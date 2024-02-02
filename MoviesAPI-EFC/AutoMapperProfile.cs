using AutoMapper;
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
                .ForMember(dest => dest.Id, opt => opt.Ignore());



            CreateMap<Actor, ActorListItemResponseDTO>();
            CreateMap<ActorCreateReqDTO, Actor>()
                .ForMember(src => src.profilepicture, opt => opt.Ignore());

            CreateMap<ActorUpdateReqDTO, Actor>()
               .ForMember(dest => dest.profilepicture, opt => opt.Ignore());

            CreateMap<ActorPatchDTO, Actor>().ReverseMap();


            CreateMap<Movie, MovieListItemResponseDTO>();

            CreateMap<Movie, MovieDetailDTO>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.MoviesGenres.Select( mg => new MoviesGenresRespDTO { Id = mg.GenreId, Name = mg.Genre.Name })))
            .ForMember(dest => dest.Cast, opt => opt.MapFrom(src => src.MoviesActors.Select( ma => new MoviesActorsRespDTO { Id = ma.ActorId, Name = ma.Actor.Name, Character = ma.Character })));

            CreateMap<MovieCreateReqDTO, Movie>()
               .ForMember(dest => dest.Poster, opt => opt.Ignore())
               .ForMember(dest => dest.MoviesGenres, opt => opt.MapFrom(MapMoviesGenres))
               .ForMember(dest => dest.MoviesActors, opt => opt.MapFrom(MapMoviesActors));

            CreateMap<MovieUpdateRequestDTO, Movie>()
               .ForMember(dest => dest.Poster, opt => opt.Ignore())
               .ForMember(dest => dest.MoviesGenres, opt => opt.MapFrom(MapMoviesGenresUpdate))
               .ForMember(dest => dest.MoviesActors, opt => opt.MapFrom(MapMoviesActorsUpdate));

            CreateMap<MoviePatchDTO, Movie>().ReverseMap();

        }

        public List<MoviesActors> MapMoviesActors(MovieCreateReqDTO movieCreateReqDTO, Movie movie)
        {
            var result = new List<MoviesActors>();
            if (movieCreateReqDTO.MoviesActors == null) return result;
            foreach (var movieActor in movieCreateReqDTO.MoviesActors)
            {
                result.Add(new MoviesActors { ActorId = movieActor.ActorId, Character = movieActor.Character, DisplayOrder = movieActor.DisplayOrder });
            }

            return result;
        }
        public List<MoviesActors> MapMoviesActorsUpdate(MovieUpdateRequestDTO movieCreateReqDTO, Movie movie)
        {
            var result = new List<MoviesActors>();
            if (movieCreateReqDTO.MoviesActors == null) return result;
            foreach (var movieActor in movieCreateReqDTO.MoviesActors)
            {
                result.Add(new MoviesActors { ActorId = movieActor.ActorId, Character = movieActor.Character, DisplayOrder = movieActor.DisplayOrder});
            }

            return result;
        }

        public List<MoviesGenres> MapMoviesGenresUpdate(MovieUpdateRequestDTO movieCreateReqDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if(movieCreateReqDTO.GenreIds == null) return result;
            foreach (var genreId in movieCreateReqDTO.GenreIds)
            {
                result.Add(new MoviesGenres { GenreId = genreId });
            }

            return result;
        }
        public List<MoviesGenres> MapMoviesGenres(MovieCreateReqDTO movieCreateReqDTO, Movie movie)
        {
            var result = new List<MoviesGenres>();
            if(movieCreateReqDTO.GenreIds == null) return result;
            foreach (var genreId in movieCreateReqDTO.GenreIds)
            {
                result.Add(new MoviesGenres { GenreId = genreId });
            }

            return result;
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using MoviesAPI_EFC.Helpers;
using MoviesAPI_EFC.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Movies
{
    public class MovieCreateReqDTO
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool OnTheaters { get; set; }
        public DateTime Date { get; set; }
        [ValidateImage(size: 4)]
        public IFormFile Poster { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> GenreIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<MoviesActorCreateReqDTO>>))]
        public List<MoviesActorCreateReqDTO> MoviesActors { get; set; }
    }
}

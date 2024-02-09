using MoviesAPI_EFC.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Entities
{
    public class Movie: IId
    {
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool OnTheaters { get; set; }
        public DateTime Date { get; set; }
        public string Poster { get; set; }
        public List<MoviesActors> MoviesActors { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set;}
        public List<MovieTheaterMovie> MovieTheaterMovies { get; set; }

    }
}

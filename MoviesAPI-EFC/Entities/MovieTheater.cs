using MoviesAPI_EFC.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Entities
{
    public class MovieTheater: IId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public List<MovieTheaterMovie> MovieTheaterMovies { get; set; }
    }
}

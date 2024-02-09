using MoviesAPI_EFC.DTOs.Interfaces;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Entities
{
    public class MovieTheater: IId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public Point Location { get; set; }
        public List<MovieTheaterMovie> MovieTheaterMovies { get; set; }
    }
}

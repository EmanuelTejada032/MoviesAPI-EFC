using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.MovieTheater
{
    public class MovieTheaterUpdateReqDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
    }
}

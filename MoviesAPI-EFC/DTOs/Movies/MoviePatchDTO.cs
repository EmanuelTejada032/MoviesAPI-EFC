using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Movies
{
    public class MoviePatchDTO
    {
        [Required]
        [StringLength(300)]
        public string Title { get; set; }
        public bool OnTheaters { get; set; }
        public DateTime Date { get; set; }
    }
}

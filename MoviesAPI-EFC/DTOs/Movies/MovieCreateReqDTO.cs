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
        public List<int> GenreIds { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Genres
{
    public class GenreUpdateReqDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}

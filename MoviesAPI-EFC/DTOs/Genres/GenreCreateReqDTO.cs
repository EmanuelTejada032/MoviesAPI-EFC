using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Genres
{
    public class GenreCreateReqDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}

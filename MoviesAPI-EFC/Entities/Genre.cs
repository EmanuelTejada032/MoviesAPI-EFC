using MoviesAPI_EFC.DTOs.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Entities
{
    public class Genre: IId
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public List<MoviesGenres> MoviesGenres { get; set; }
    }
}

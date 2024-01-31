using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.Entities
{
    public class Actor
    {
        public int Id { get; set; }
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string? profilepicture { get; set; }
        public List<MoviesActors> MoviesActors { get; set; } 
    }
}

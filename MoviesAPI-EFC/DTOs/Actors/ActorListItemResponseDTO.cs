using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Actors
{
    public class ActorListItemResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string profilepicture { get; set; }
    }
}

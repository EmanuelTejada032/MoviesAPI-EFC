using MoviesAPI_EFC.Validations;
using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Actors
{
    public class ActorUpdateReqDTO
    {
        [Required]
        [StringLength(120)]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        [ValidateImage(size: 4)]
        public IFormFile? profilepicture { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Review
{
    public class ReviewCreateReqDTO
    {
        [Required]
        public string Text { get; set; }
        [Range(1,5)]
        public int Points { get; set; }
    }
}

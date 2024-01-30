using System.ComponentModel.DataAnnotations;

namespace MoviesAPI_EFC.DTOs.Movies
{
    public class MovieListItemResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool OnTheaters { get; set; }
        public DateTime Date { get; set; }
        public string Poster { get; set; }
    }
}

using MoviesAPI_EFC.DTOs.General;

namespace MoviesAPI_EFC.DTOs.Movies
{
    public class MoviesFilterDTO: PaginationData
    {
        public string? Title { get; set; }
        public bool? OnTheaters { get; set; }
        public string? FieldToFilter { get; set; }
        public bool FilterDescending { get; set; } = false;
    }
}

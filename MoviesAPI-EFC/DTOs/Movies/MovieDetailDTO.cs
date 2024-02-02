namespace MoviesAPI_EFC.DTOs.Movies
{
    public class MovieDetailDTO: MovieListItemResponseDTO
    {
        public List<MoviesActorsRespDTO> Cast { get; set; }
        public List<MoviesGenresRespDTO> Genres  { get; set; }
    }
}

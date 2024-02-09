namespace MoviesAPI_EFC.DTOs.MovieTheater
{
    public class MovieTheaterListCLosestItemResDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double DistanceInMetters { get; set; }
    }
}

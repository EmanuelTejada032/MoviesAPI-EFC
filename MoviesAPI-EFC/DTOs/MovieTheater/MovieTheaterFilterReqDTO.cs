namespace MoviesAPI_EFC.DTOs.MovieTheater
{
    public class MovieTheaterFilterReqDTO
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MovieTheaterWithinDistanceInKms { get; set; } = 5;
    }
}

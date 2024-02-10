namespace MoviesAPI_EFC.DTOs.Security
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
    }
}

namespace MoviesAPI_EFC.DTOs.Security
{
    public class HashResponse
    {
        public string Hash { get; set; }
        public byte[] Salt { get; set; }
    }
}

namespace MoviesAPI_EFC.Entities
{
    public class CustomExceptionFilterLog
    {
        public int Id { get; set; }
        public string ErrorLocation { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorStackTrace { get; set; }
        public string ErrorType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

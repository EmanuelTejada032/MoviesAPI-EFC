namespace MoviesAPI_EFC.DTOs.Review
{
    public class ReviewListItemDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Points { get; set; }
        public int MovieId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}

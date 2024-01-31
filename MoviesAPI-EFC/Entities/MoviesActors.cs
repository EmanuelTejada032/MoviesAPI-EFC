namespace MoviesAPI_EFC.Entities
{
    public class MoviesActors
    {
        public int MovieId { get; set; }
        public int ActorId { get; set; }
        public string Character {  get; set; }
        public int DisplayOrder { get; set; }
        public Movie Movie { get; set; }
        public Actor Actor { get; set; }
    }
}

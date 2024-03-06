namespace MoviesAPI_EFC.Helpers
{
    public class ValidationErrors : Exception
    {
        public Dictionary<string, string> Errors { get; }

        public ValidationErrors(Dictionary<string, string> errors)
        {
            Errors = errors;
        }

        public ValidationErrors(string key, string error)
        {
            Errors = new()
         {
             { key, error }
         };
        }
    }
}

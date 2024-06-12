namespace Data.Utilities
{
    public class Error
    {
        public string Message {get; private set;}
        public Error (string message) 
        {
            Message = message;
        }
        public static Error None => new("");
    }
}
namespace Data.Utilities
{
    public class RequestResult 
    {
        public bool IsSuccess {get; private set;}
        public Error Error {get; private set;}

        public RequestResult(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = error;    
        }
        
        public static RequestResult Success() => new(true, Error.None);
        public static RequestResult Failure(Error error) => new(false, error);
    }
}
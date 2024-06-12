namespace Data.Utilities
{
    public class RequestResult<T> : RequestResult
    {   
        public T? Data {get; private set;}

        public RequestResult(bool isSuccess, Error error, T? data) : base(isSuccess, error)
        {
            Data = data;
        }

        public static new RequestResult<T> Success(T data) => new(true, Error.None, data);
        public static new RequestResult<T> Failure(Error error) => new(false, Error.None, default);

    }
}
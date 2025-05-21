namespace SurveyBasket.Abstractions
{
    public class Result
    {

        public Result(bool success , Error error)
        {
            if((success && error !=Error.None) || (!success && error == Error.None))
                throw new ArgumentException("Invalid arguments for Result");
            IsSuccess = success;
            Error = error;
        }
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        public Error Error { get; } = default!;


        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T value) => new(true, value, Error.None);
        public static Result<T> Failure<T>(Error error) => new(false, default!, error);


    }

    public class Result<T> : Result
    {
        private T _value { get; } = default!;
        public Result(bool success, T value, Error error) : base(success, error)
        {
            _value = value;
        }

        public T Value => IsSuccess ? _value : throw new InvalidOperationException("Failure Cant Have Value");


    }
}

namespace Services.Infrastructure.Exceptions
{
    public class ValidationException : ErrorException
    {
        public string Property { get; protected set; }
        public ValidationException(string msg, string prop) : base(msg)
        {
            Property = prop;
        }
    }
}

namespace Services.Infrastructure.Exceptions
{
    public class AccessViolationException : ErrorException
    {
        public AccessViolationException(string msg) : base(msg)
        {

        }
    }
}

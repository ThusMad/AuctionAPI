namespace Services.Infrastructure.Exceptions
{
    internal class AccessViolationException : ErrorException
    {
        public AccessViolationException(string msg) : base(msg)
        {

        }
    }
}

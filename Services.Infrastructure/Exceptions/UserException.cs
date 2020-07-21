namespace Services.Infrastructure.Exceptions
{
    public class UserException : ErrorException
    {
        public UserException(int code, string msg) : base(msg, code)
        {
        }
    }
}

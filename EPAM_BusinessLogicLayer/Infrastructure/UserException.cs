namespace EPAM_BusinessLogicLayer.Infrastructure
{
    public class UserException : ErrorException
    {
        public UserException(int code, string msg) : base(msg, code)
        {
        }
    }
}

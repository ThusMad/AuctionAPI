namespace EPAM_BusinessLogicLayer.Infrastructure
{
    internal class AccessViolationException : ErrorException
    {
        public AccessViolationException(string msg) : base(msg)
        {

        }
    }
}

namespace Services.Infrastructure.Exceptions
{
    public class ItemNotFountException : ErrorException
    {
        public string PropertyName { get; private set; }

        public ItemNotFountException(string prop, string msg) : base(msg, 404)
        {
            PropertyName = prop;
        }
    }
}

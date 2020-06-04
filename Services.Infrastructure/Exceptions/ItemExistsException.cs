namespace Services.Infrastructure.Exceptions
{
    public class ItemExistsException : ErrorException
    {
        public string Name { get; set; }
        public ItemExistsException(string name, string msg) : base(msg)
        {
            Name = name;
        }

    }
}

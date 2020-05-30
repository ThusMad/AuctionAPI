namespace EPAM_BusinessLogicLayer.Infrastructure
{
    internal class ItemExistsException : ErrorException
    {
        public string Name { get; set; }
        public ItemExistsException(string name, string msg) : base(msg)
        {
            Name = name;
        }

    }
}

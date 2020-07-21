using System;

namespace EPAM_DataAccessLayer.Entities
{
    public class Media
    {
        public Guid Id { get; set; }
        public string Url { get; set; }

        public Media()
        {
        }

        public Media(string url)
        {
            Url = url;
        }
    }
}

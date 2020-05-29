using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPAM_DataAccessLayer.Entities
{
    public class Media
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Media(string url)
        {
            Url = url;
        }
    }
}

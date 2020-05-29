using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EPAM_DataAccessLayer.Entities
{
    public class Bage
    { 
        public Guid Id { get; set; }
        public string IconUrl { get; set; }
        public string BackgroundColor { get; set; }
        public string Description { get; set; }
    }
}

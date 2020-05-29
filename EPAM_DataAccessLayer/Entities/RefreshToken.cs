using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.Extensions.Logging;

namespace EPAM_DataAccessLayer.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string? Token { get; set; }
        public long TokenExpiration { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}

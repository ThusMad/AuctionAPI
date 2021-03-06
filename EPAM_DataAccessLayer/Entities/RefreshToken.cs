﻿using System;

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

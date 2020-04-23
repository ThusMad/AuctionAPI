using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EPAM_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EPAM_API.Services
{
    public class UserProvider : IUserProvider
    {
        private readonly IHttpContextAccessor _context;

        public UserProvider(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Guid GetUserId()
        {
            return Guid.Parse(_context.HttpContext.User.Claims
                .First(i => i.Type == ClaimTypes.NameIdentifier).Value);
        }

        public  string GetUserRole()
        {
            return _context.HttpContext.User.Claims
                .First(i => i.Type == ClaimTypes.Role).Value;
        }
    }
}

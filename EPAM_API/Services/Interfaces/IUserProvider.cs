using System;

namespace EPAM_API.Services.Interfaces
{
    public interface IUserProvider
    {
        Guid GetUserId();
        string GetUserRole();
    }
}
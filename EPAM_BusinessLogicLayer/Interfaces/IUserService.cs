using System;
using System.Collections.Generic;
using EPAM_BusinessLogicLayer.DTO;

namespace EPAM_BusinessLogicLayer.Interfaces
{
    public interface IUserService : IDisposable
    {
        UserDTO CreateUser(UserDTO userDto);
        UserDTO GetUser(int? id);
        UserDTO GetUser(string username, string password);
        IEnumerable<UserDTO> GetUsers();
    }
}
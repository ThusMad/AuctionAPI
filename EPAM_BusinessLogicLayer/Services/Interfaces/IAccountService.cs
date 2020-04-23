using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.DTO;
using Microsoft.AspNetCore.Identity;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    public interface IAccountService 
    {
        Task<ApplicationUserDto> CreateUserAsync(RegistrationDTO registrationDto, IEnumerable<string> roles);
        Task<ApplicationUserDto>? GetUserByIdAsync(Guid id);
        ApplicationUserDto? GetDetailedUser(int? id);
        ApplicationUserDto GetUser(string username, string password);
        IEnumerable<ApplicationUserDto> GetUsers();
        ApplicationUserDto? UpdateUser(ApplicationUserDto applicationUserDto);
        void DeleteUser(int userId, string reason);
        Task<bool> IsValidUsernameAndPasswordCombinationAsync(string username, string password);
    }
}
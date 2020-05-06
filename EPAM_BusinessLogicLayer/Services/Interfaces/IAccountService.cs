using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using EPAM_BusinessLogicLayer.DataTransferObject;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace EPAM_BusinessLogicLayer.Services.Interfaces
{
    /// <summary>
    /// This service provides basic operations that needed to work with user accounts
    /// </summary>
    public interface IAccountService 
    {
        Task<ApplicationUserDto> CreateUserAsync(RegistrationDTO registrationDto, IEnumerable<string> roles);
        /// <summary>
        /// This method found user by provided id and map it to <seealso cref="TUserDto"/> 
        /// </summary>
        /// <typeparam name="TUserDto">Type that <see cref="ApplicationUser"/> entity will be mapped </typeparam>
        /// <param name="id">User id that must be found</param>
        /// <returns>User dto</returns>
        /// <exception cref="ArgumentNullException">Throws when provided id is null or empty</exception>
        /// <exception cref="ItemNotFountException">Throws when a database doesn't contain user with the provided id</exception>
        Task<TUserDto> GetUserByIdAsync<TUserDto>(Guid id);
        IEnumerable<ApplicationUserDto> GetAllUsers(int? limit, int? offset);
        Task UpdateUserAsync(Guid id, ApplicationUserPatchModel applicationUserDto);
        Task DeleteUserAsync(Guid id, string reason);
        Task<bool> IsValidUsernameAndPasswordCombinationAsync(string username, string password);
    }
}
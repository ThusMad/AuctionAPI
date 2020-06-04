using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services.DataTransferObjects.Objects;

namespace Services.AccountService.Interfaces
{
    /// <summary>
    /// This service provides basic operations that needed to work with user accounts
    /// </summary>
    public interface IAccountService 
    {
        Task<ApplicationUserDto> InsertUserAsync(RegistrationDTO registrationDto, IEnumerable<string> roles);
        /// <summary>
        /// This method found user by provided id and map it to <seealso cref="TUserDto"/> 
        /// </summary>
        /// <typeparam name="TUserDto">Type that <see cref="ApplicationUser"/> entity will be mapped </typeparam>
        /// <param name="id">User id that must be found</param>
        /// <returns><seealso cref="TUserDto"/></returns>
        Task<TUserDto> GetUserByIdAsync<TUserDto>(Guid id);
        Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync(int? limit, int? offset);
        Task UpdateUserAsync(Guid id, ApplicationUserPatchModel applicationUserDto);
        Task RemoveUserAsync(Guid id);
        Task<bool> IsValidUsernameAndPasswordCombinationAsync(string? username, string? password);
        Task AttachProfilePicture(Guid userId, string url);
    }
}
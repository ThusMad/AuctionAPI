using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.BusinessModels;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.UnitOfWork.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Services.AccountService.Interfaces;
using Services.DataTransferObjects.Objects;
using Services.Helpers.Utilities;
using Services.Infrastructure.Exceptions;

namespace Services.AccountService.Service
{
    /// <summary>
    /// This class provides a realization of basic operations that needed to work with user accounts
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;

        public AccountService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        /// <summary>
        /// This function create a new user for application
        /// </summary>
        /// <param name="registrationDto">Object in which data necessary for registration is stored</param>
        /// <param name="roles">List of roles for created user</param>
        /// <returns>DTO model for created user</returns>
        /// <exception cref="UserException">Thrown when user already registered or password is incorrect for details <seealso cref="UserException.Message"/> </exception>
        public async Task<ApplicationUserDto> InsertUserAsync(RegistrationDTO registrationDto, IEnumerable<string> roles)
        {
            if (await _userManager.FindByNameAsync(registrationDto.Username).ConfigureAwait(false) != null)
            {
                throw new UserException(200, "User with following username already exists");
            }

            // TODO: use mapper
            var user = new ApplicationUser
            {
                Email = registrationDto.Email,
                UserName = registrationDto.Username,
                RegistrationDate = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            };

            var createResult = await _userManager.CreateAsync(user, registrationDto.Password);

            if (!createResult.Succeeded)
            {
                throw new UserException(500,
                    string.Join("\n", createResult.Errors.ToArray().SelectMany(a => a.Description)));
            }

            foreach (var role in roles)
            {
                var addRoleResult = await _userManager.AddToRoleAsync(user, role.ToUpperInvariant());

                if (!addRoleResult.Succeeded)
                {
                    throw new UserException(500,
                        string.Join("\n", addRoleResult.Errors.ToArray().SelectMany(a => a.Description)));
                }
            }

            var balance = await _unitOfWork.InsertAsync(new Balance { PersonalFunds = 0, User = user});
            await _unitOfWork.InsertAsync(balance);
            await _unitOfWork.CommitAsync();

            return _mapper.Map<ApplicationUser, ApplicationUserDto>(user);
        }

        /// <summary>
        /// Checks if a user with the following username have the following password
        /// </summary>
        /// <param name="username">User password</param>
        /// <param name="password">User username</param>
        /// <returns>Result of comparing password stored in a database with provided</returns>
        ///<exception cref="ItemNotFountException">Throws when a database doesn't contain user with the provided username</exception>
        public async Task<bool> IsValidUsernameAndPasswordCombinationAsync(string? username, string? password)
        {
            if (username == null || password == null)
            {
                throw new ArgumentNullException(nameof(password), "Password or username is null");
            }

            var user = await GetUserByUsernameAsync(username).ConfigureAwait(false);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task AttachProfilePictureAsync(Guid userId, string url)
        {
            var user = await GetUserByIdAsync(userId);
            string prevImagePath = null;
            if (user.ProfilePicture != null)
            {
                prevImagePath = user.ProfilePicture.Url;
            }

            using (var transaction = _unitOfWork.BeginTransaction())
            {
                if (prevImagePath != null)
                {
                    transaction.Remove(user.ProfilePicture);
                }
                user.ProfilePicture = new Media(url);
                transaction.Update(user);
            }

            if (!string.IsNullOrEmpty(prevImagePath))
            {
                Utility.RemoveImage(prevImagePath);
            }
        }

        /// <summary>
        /// This method found user by provided id and map it to <seealso cref="TUserDto"/> 
        /// </summary>
        /// <typeparam name="TUserDto">Type that <see cref="ApplicationUser"/> entity will be mapped </typeparam>
        /// <param name="id">User id that must be found</param>
        /// <returns>User dto</returns>
        /// <exception cref="ArgumentNullException">Throws when provided id is null or empty</exception>
        /// <exception cref="ItemNotFountException">Throws when a database doesn't contain user with the provided id</exception>
        public async Task<TUserDto> GetUserByIdAsync<TUserDto>(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id), "id was null");
            }

            var user = await GetUserByIdAsync(id);

            return _mapper.Map<ApplicationUser, TUserDto> (user);
        }

        public async Task<decimal> GetUserFeeAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains(Roles.Plus))
            {
                return 4;
            }
            if (roles.Contains(Roles.Premium))
            {
                return 2.5M;
            }

            return 5;
        }

        public async Task<string> GetUserRoleAsync(Guid id)
        {
            var user = await GetUserByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);

            return roles.First();
        }

        /// <summary>
        /// This function returns an enumerable collection of <seealso cref="ApplicationUserDto"/> sorted by identifier that are offset by <see cref="offset"/>
        /// and contain no more than <see cref="limit"/> records
        /// </summary>
        /// <param name="limit">limit returned <seealso cref="ApplicationUserDto"/> entities, must be less equal 20</param>
        /// <param name="offset">offset searched <seealso cref="ApplicationUserDto"/> entities</param>
        /// <returns>Enumerable collection of <seealso cref="ApplicationUserDto"/></returns>
        public async Task<IEnumerable<ApplicationUserDto>> GetAllUsersAsync(int? limit, int? offset)
        {
            var limitVal = limit == null || limit > 20 ? 20 : limit.Value;
            var offsetVal = offset ?? 0;

            var users = await _unitOfWork.GetAll<ApplicationUser>(limitVal, offsetVal).ToListAsync();

            return _mapper.Map<IEnumerable<ApplicationUser>, List<ApplicationUserDto>>(users);
        }

        public async Task UpdateUserAsync(Guid id, ApplicationUserPatchModel applicationUserDto)
        {
            var user = await GetUserByIdAsync(id).ConfigureAwait(false);

            await _userManager.UpdateAsync(_mapper.Map(applicationUserDto, user));
        }

        public async Task RemoveUserAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            await _userManager.DeleteAsync(user);
        }

        private async Task<ApplicationUser> GetUserByIdAsync(Guid id)
        {
            var user = await _userManager.Users
                .Include(x => x.Balance)
                .Include(x => x.ProfilePicture)
                .SingleAsync(i => i.Id == id.ToString());

            if (user == null)
            {
                throw new ItemNotFountException("User", $"User with following {nameof(id)} not found");
            }

            return user;
        }

        private async Task<ApplicationUser> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.Users
                .Include(x => x.Balance)
                .Include(x => x.ProfilePicture)
                .SingleAsync(i => i.NormalizedUserName == username.ToUpperInvariant());

            if (user == null)
            {
                throw new ItemNotFountException("User", $"User with following {nameof(username)} not found");
            }

            return user;
        }
    }
}

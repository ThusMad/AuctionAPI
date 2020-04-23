using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Services.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Enums;
using EPAM_DataAccessLayer.Interfaces;
using EPAM_DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;

namespace EPAM_BusinessLogicLayer.Services
{
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
        public async Task<ApplicationUserDto> CreateUserAsync(RegistrationDTO registrationDto, IEnumerable<string> roles)
        {
            if (await _userManager.FindByNameAsync(registrationDto.Username) != null)
            {
                throw new UserException("User with following username already exists");
            }

            var user = new ApplicationUser()
            {
                Email = registrationDto.Email,
                UserName = registrationDto.Username,
                RegistrationDate = Utility.DateTimeToUnixTimestamp(DateTime.UtcNow)
            };
            var createResult = await _userManager.CreateAsync(user, registrationDto.Password);

            if (!createResult.Succeeded)
            {
                throw new UserException(500,
                    string.Join("\n", createResult.Errors.ToArray().SelectMany(a => a.Description)));
            }

            foreach (var role in roles)
            {
                var addRoleResult = await _userManager.AddToRoleAsync(user, role);

                if (!addRoleResult.Succeeded)
                {
                    throw new UserException(500,
                        string.Join("\n", addRoleResult.Errors.ToArray().SelectMany(a => a.Description)));
                }
            }

            return _mapper.Map<ApplicationUser, ApplicationUserDto>(user);
        }

        public async Task<bool> IsValidUsernameAndPasswordCombinationAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                throw new ItemNotFountException("User", "User with following username not found");
            }

            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<ApplicationUserDto>? GetUserByIdAsync(Guid id)
        {
            if (string.IsNullOrEmpty(id.ToString()))
            {
                throw new ArgumentNullException(nameof(id), "id was null");
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                throw new ItemNotFountException("User", $"User with following {nameof(id)} not found");
            }

            return _mapper.Map<ApplicationUser, ApplicationUserDto> (user);
        }

        public ApplicationUserDto? GetDetailedUser(int? id)
        {
            throw new NotImplementedException();
        }

        public ApplicationUserDto GetUser(string username, string password)
        {
            if(username == string.Empty)
            {
                throw new ArgumentNullException($"{nameof(username)} was empty");
            }

            if (password == string.Empty)
            {
                throw new ArgumentNullException($"{nameof(password)} was empty");
            }

            var users = _unitOfWork.Repository<ApplicationUser>().Find(entry => entry.UserName == username);

            if (!users.Any())
            {
                throw new ValidationException($"User with following username: \"{username}\" was not found", nameof(username));
            }

            var user = users.First();


            return _mapper.Map<ApplicationUser, ApplicationUserDto>(user);
        }

        public IEnumerable<ApplicationUserDto> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<ApplicationUser, ApplicationUserDto>()).CreateMapper();
            return mapper.Map<IEnumerable<ApplicationUser>, List<ApplicationUserDto>>(_unitOfWork.Repository<ApplicationUser>().GetAll());
        }

        public ApplicationUserDto? UpdateUser(ApplicationUserDto applicationUserDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int userId, string reason)
        {
            throw new NotImplementedException();
        }
    }
}

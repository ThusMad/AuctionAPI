using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using EPAM_BusinessLogicLayer.DTO;
using EPAM_BusinessLogicLayer.Infrastructure;
using EPAM_BusinessLogicLayer.Interfaces;
using EPAM_DataAccessLayer.Entities;
using EPAM_DataAccessLayer.Interfaces;

namespace EPAM_BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        private static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }

        public UserDTO CreateUser(UserDTO userDto)
        {
            var users = Database.Users.Find(entry => entry.Username == userDto.Username);

            if (users.Count() != 0)
            {
                throw new ValidationException("User with following username already exists", nameof(userDto.Username));
            }

            User user = new User()
            {
                Username = userDto.Username,
                Password = HashPassword(userDto.Password)
            };

            Database.Users.Create(user);

            return new UserDTO()
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password
            };
        }

        public UserDTO GetUser(int? id)
        {
            if (id == null)
                throw new ValidationException("id was null", nameof(id));
            var user = Database.Users.Get(id.Value);
            if (user == null)
                throw new ValidationException("User not found", nameof(id));
            return new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Username = user.Username
            };
        }

        public UserDTO GetUser(string username, string password)
        {
            if(username == string.Empty)
                throw new ValidationException("username was empty", nameof(username));

            if (password == string.Empty)
                throw new ValidationException("password was empty", nameof(password));

            var users = Database.Users.Find(entry => entry.Username == username);

            if (!users.Any())
            {
                throw new ValidationException($"User with following username: \"{username}\" was not found", nameof(username));
            }

            var user = users.First();

            if (!VerifyHashedPassword(user.Password, password))
            {
                throw new ValidationException("Username or password is incorrect ", nameof(password));
            }

            return new UserDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Username = user.Username
            };
        }

        public IEnumerable<UserDTO> GetUsers()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<User, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<User>, List<UserDTO>>(Database.Users.GetAll());
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}

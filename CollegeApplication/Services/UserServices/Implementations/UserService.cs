using AutoMapper;
using CollegeApplication.Models;
using CollegeApplication.Services.UserService.Contracts;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Update;
using Repository.Contracts;
using Repository.Models;
using System.Security.Cryptography;

namespace CollegeApplication.Services.UserServices.Implementations
{
    public class UserService : IUserService
    {
        private readonly ICollegeRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(ICollegeRepository<User> userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public (string passwordHash, string salt) CreatePasswordHashWithSalt(string password)
        {
            try
            {
                var salt = new byte[128 / 8];
                using(var rand = RandomNumberGenerator.Create())
                {
                    rand.GetBytes(salt);
                }

                //Create Password hash
                var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                    ));

                return (hash, Convert.ToBase64String(salt));
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateUserAsync(UserDto dto)
        {
            //Old way
            if(dto == null)
            {
                throw new ArgumentNullException(nameof(dto)); //, "Please provide valid User details"
            }

            //New way from .NET 6
            //ArgumentNullException.ThrowIfNull(dto, nameof(dto));
            ArgumentNullException.ThrowIfNull(dto, $"Arguement {nameof(dto)} is null");

            var existingUser = await _userRepository.GetAsync(x => x.UserName == dto.UserName);
            if(existingUser != null)
            {
                throw new Exception($"UserName Already Taken");
            }

            User user = _mapper.Map<User>(dto);

            user.IsDeleted = false;
            user.CreatedDate = DateTime.Now;
            user.ModifiedDate = DateTime.Now;

            if(!string.IsNullOrEmpty(dto.Password))
            {
                var passwordHash = CreatePasswordHashWithSalt(dto.Password);
                user.Password = passwordHash.passwordHash;
                user.PasswordSalt = passwordHash.salt;
            }

            await _userRepository.CreateAsync(user);

            return true;
        }

        public async Task<List<UserReadOnlyDto>> GetUsersAsync()
        {
            var users = await _userRepository.GetAllByColumnAsync(x => !x.IsDeleted);
            var userDtos = _mapper.Map<List<UserReadOnlyDto>>(users);
            return userDtos;
        }

        public async Task<UserReadOnlyDto> GetUserById(int id)
        {
            var user = await _userRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if(user == null)
            {
                throw new Exception($"No user found with provided id {id}");
            }

            return _mapper.Map<UserReadOnlyDto>(user);
        }

        public async Task<UserReadOnlyDto> GetUserByName(string name)
        {
            var user = await _userRepository.GetAsync(x => !x.IsDeleted && x.UserName.Contains(name));

            if (user == null)
            {
                throw new Exception($"No user found with provided name {name}");
            }

            return _mapper.Map<UserReadOnlyDto>(user);
        }

        public async Task<bool> UpdateUser(UserReadOnlyDto dto)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var existingUser = await _userRepository.GetAsync(x => !x.IsDeleted && x.Id == dto.Id, true);

            if(existingUser == null)
            {
                throw new Exception($"User not founf with id {dto.Id}");
            }

            var userToUpdate = _mapper.Map<User>(dto);
            userToUpdate.Password = existingUser.Password;
            userToUpdate.PasswordSalt = existingUser.PasswordSalt;
            userToUpdate.CreatedDate = existingUser.CreatedDate;
            userToUpdate.ModifiedDate = DateTime.Now;

            await _userRepository.UpdateAsync(userToUpdate);

            return true;
        }

        public async Task<bool> DeleteUser(int id)
        {
            if(id <= 0)
            {
                throw new ArgumentException($"Invalid user id {id}");
            }

            var user = await _userRepository.GetAsync(x => !x.IsDeleted && x.Id == id);
            if(user == null)
            {
                throw new Exception($"User not found with id {id}");
            }

            user.IsDeleted = true;
            user.IsActive = false;

            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
}

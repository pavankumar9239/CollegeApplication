using CollegeApplication.Models;

namespace CollegeApplication.Services.UserService.Contracts
{
    public interface IUserService
    {
        public Task<bool> CreateUserAsync(UserDto dto);
        public Task<List<UserReadOnlyDto>> GetUsersAsync();
        public Task<UserReadOnlyDto> GetUserById(int id);
        public Task<UserReadOnlyDto> GetUserByName(string name);
        public Task<bool> UpdateUser(UserReadOnlyDto dto);
        public Task<bool> DeleteUser(int id);
        public (string passwordHash, string salt) CreatePasswordHashWithSalt(string password);
    }
}

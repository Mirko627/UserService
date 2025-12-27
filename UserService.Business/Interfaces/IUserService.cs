using UserService.Repository.Entities;
using UserService.Shared.dtos;

namespace UserService.Business.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserDto>> GetAllAsync();
        public Task<UserDto?> GetUserByIdAsync(int id);
        public Task AddAsync(CreateUserDto user);
        public Task UpdateAsync(int id, UpdateUserDto user);
        public Task DeleteAsync(int id);
        public Task<bool> ChangePasswordAsync(int id, ChangePasswordDto dto);
        public Task<string?> LoginAsync(LoginDto dto);
    }
}

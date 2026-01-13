using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Business.Interfaces;
using UserService.Repository.Entities;
using UserService.Repository.Interfaces;
using UserService.Shared.dtos;

namespace UserService.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            List<User> users = await repository.GetAllAsync();
            return mapper.Map<List<UserDto>>(users);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            User user = await repository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"Utente con ID {id} non trovato.");

            return mapper.Map<UserDto>(user);
        }

        public async Task AddAsync(CreateUserDto userDto)
        {
            User? existingUser = await repository.GetUserByUsernameAsync(userDto.UserName);
            if (existingUser != null)
                throw new InvalidOperationException($"Lo username '{userDto.UserName}' è già in uso.");

            User u = mapper.Map<User>(userDto);

            u.LastModified = DateTime.UtcNow;
            u.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            await repository.AddAsync(u);
        }

        public async Task DeleteAsync(int id, int userId)
        {
            if (userId != id) throw new UnauthorizedAccessException("Non hai i permessi per eliminare questo utente.");
            User user = await repository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"Impossibile eliminare: utente con ID {id} non trovato.");
            await repository.DeleteAsync(id);
        }

        public async Task UpdateAsync(int id, UpdateUserDto userDto, int userId)
        {
            if (userId != id) throw new UnauthorizedAccessException("Non hai i permessi per modificare questo utente.");
            User user = await repository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"Impossibile eliminare: utente con ID {id} non trovato.");

            mapper.Map(userDto, user);
            user.LastModified = DateTime.UtcNow;

            await repository.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(int id, ChangePasswordDto dto, int userId)
        {
            if (userId != id) throw new UnauthorizedAccessException("Non hai i permessi per modificare questo utente.");
            User user = await repository.GetUserByIdAsync(id) ?? throw new KeyNotFoundException($"Impossibile eliminare: utente con ID {id} non trovato.");

            bool isOldPasswordCorrect = BCrypt.Net.BCrypt.Verify(dto.OldPassword, user.Password);
            if (!isOldPasswordCorrect) throw new UnauthorizedAccessException("La vecchia password non è corretta.");

            if (dto.OldPassword == dto.NewPassword) throw new InvalidOperationException("La nuova password non può essere uguale alla precedente.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.LastModified = DateTime.UtcNow;

            await repository.UpdateAsync(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            User userEntity = await repository.GetUserByUsernameAsync(dto.Username) ?? throw new UnauthorizedAccessException("Credenziali non valide.");

            if (userEntity == null || !BCrypt.Net.BCrypt.Verify(dto.Password, userEntity.Password)) throw new UnauthorizedAccessException("Credenziali non valide.");

            return GenerateJwtToken(userEntity);
        }
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("b133a0c0e9bee3be20163d2ad31d6248db292aa6dcb1ee087a2aa50e0fc75ae2"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "UserService",
                audience: "ProjectMicroservizi",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

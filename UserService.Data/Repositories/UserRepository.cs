using Microsoft.EntityFrameworkCore;
using UserService.Data.Context;
using UserService.Repository.Entities;
using UserService.Repository.Interfaces;

namespace UserService.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDBContext _context;

        public UserRepository(UserDBContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            List<User> users = await _context.users.ToListAsync();
            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            User? user = await _context.users.FindAsync(id);
            return user;
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            User? user = await _context.users.FirstOrDefaultAsync(u => u.UserName == username); 
            return user;
        }
        public async Task AddAsync(User user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            User? u = await GetUserByIdAsync(id);
            if (u == null)
                throw new Exception("Utente non trovato");
            _context.users.Remove(u);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
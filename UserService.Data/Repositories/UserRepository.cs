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

        public async Task<List<User>> GetAllAsync(CancellationToken ct = default)
        {
            List<User> users = await _context.users.ToListAsync(ct);
            return users;
        }

        public async Task<User?> GetUserByIdAsync(int id, CancellationToken ct = default)
        {
            User? user = await _context.users.FindAsync(id, ct);
            return user;
        }
        public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken ct = default)
        {
            User? user = await _context.users.FirstOrDefaultAsync(u => u.UserName == username, ct); 
            return user;
        }
        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _context.users.AddAsync(user, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            User? u = await GetUserByIdAsync(id, ct);
            if (u == null)
                throw new Exception("Utente non trovato");
            _context.users.Remove(u);
            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateAsync(User user, CancellationToken ct = default)
        {
            _context.users.Update(user);
            await _context.SaveChangesAsync(ct);
        }
    }
}
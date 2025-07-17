using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HawalatySystem.Data;
using HawalatySystem.Models;

namespace HawalatySystem.Services
{
    public class AuthenticationService
    {
        private readonly HawalatyDbContext _context;
        public User? CurrentUser { get; private set; }

        public AuthenticationService(HawalatyDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Agent)
                    .Include(u => u.UserPermissions)
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {
                    CurrentUser = user;
                    user.LastLoginDate = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public bool HasPermission(string permission)
        {
            if (CurrentUser == null) return false;
            if (CurrentUser.Role == UserRole.Admin) return true;

            return CurrentUser.UserPermissions?.Any(p => p.Permission == permission && p.IsGranted) ?? false;
        }

        public bool IsInRole(UserRole role)
        {
            return CurrentUser?.Role == role;
        }

        public async Task<User?> CreateUserAsync(User user, string password)
        {
            try
            {
                // Check if username already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Username == user.Username);

                if (existingUser != null)
                    return null;

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
                user.CreatedDate = DateTime.Now;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null || !BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                    return false;

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
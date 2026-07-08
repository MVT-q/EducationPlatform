using Education_Platform.Data;
using Education_Platform.Helpers;
using Education_Platform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Services
{
    public class UserService
    {
        public async Task<List<User>> GetUsersAsync()
        {
            using var db = new AppDbContext();

            return await db.Users.ToListAsync();
        }

        public async Task<UserActionResult> DeleteUserAsync(int currentUserId, int userId)
        {
            using var db = new AppDbContext();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if(user == null)
            {
                return new UserActionResult
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (currentUserId == userId)
            {
                return new UserActionResult
                {
                    Success = false,
                    Message = "You cannot delete yourself"
                };
            }

            if(user.Role == "Admin")
            {
                int adminCount = await db.Users.CountAsync(u => u.Role == "Admin");

                if(adminCount <= 1)
                {
                    return new UserActionResult
                    {
                        Success = false,
                        Message = "Cannot delete last admin"
                    };
                } 
            }

            db.Users.Remove(user);

            await db.SaveChangesAsync();

            return new UserActionResult
            {
                Success = true,
                Message = "User deleted"
            };
        }

        public async Task<UserActionResult> UpdateRoleAsync(int currentUserId, int userId, string role)
        {
            using var db = new AppDbContext();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return new UserActionResult
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (currentUserId == userId && role != "Admin")
            {
                return new UserActionResult
                {
                    Success = false,
                    Message = "You cannot remove your own admin role"
                };
            }

            if(user.Role == "Admin" && role != "Admin")
            {
                int adminCount = await db.Users.CountAsync(u => u.Role == "Admin");

                if (adminCount <= 1)
                {
                    return new UserActionResult
                    {
                        Success = false,
                        Message = "Cannot change role of the last admin"
                    };
                }
            }

            user.Role = role;

            await db.SaveChangesAsync();

            return new UserActionResult
            {
                Success = true,
                Message = "Role updated"
            };
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            using var db = new AppDbContext();

            return await db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetUsersAsync(string? search = null)
        {
            using var db = new AppDbContext();

            var query = db.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(q =>
                    q.Login.Contains(search) ||
                    q.Email.Contains(search));
            }

            return await query.ToListAsync();
        }
    }
}

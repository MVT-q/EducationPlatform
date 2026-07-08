using BCrypt.Net;
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
    public class AuthService
    {
        public static int? CurrentUserId { get; set; }
        public static string? CurrentUserRole { get; set; }

        public async Task<AuthResult> RegisterAsync(string login, string password, string email)
        {
            using var db = new AppDbContext();

            if (await db.Users.AnyAsync(u => u.Login == login))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Login already exists"
                };
            }

            if (await db.Users.AnyAsync(u => u.Email == email))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Email already exists"
                };
            }

            string hash = await Task.Run(() => BCrypt.Net.BCrypt.HashPassword(password));

            var user = new User
            {
                Login = login,
                Password = hash,
                Email = email,
                Role = "Student"
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            return new AuthResult
            {
                Success = true,
                Message = "Registration successful"
            };
        }

        public async Task<AuthResult> LoginAsync(string login, string password)
        {
            using var db = new AppDbContext();

            var user = await db.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Wrong login or password"
                };
            }

            bool verified = await Task.Run(() => BCrypt.Net.BCrypt.Verify(password, user.Password));
            if (!verified)
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Wrong login or password"
                };
            }

            CurrentUserId = user.Id;
            CurrentUserRole = user.Role;

            return new AuthResult
            {
                Success = true,
                Message = "Login successful"
            };
        }
    }
}

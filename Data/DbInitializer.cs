using Education_Platform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Data
{
    public static class DbInitializer
    {
        public static void Initialize()
        {
            using var db = new AppDbContext();

            db.Database.Migrate();

            SeedAdmin(db);
        }

        private static void SeedAdmin(AppDbContext db)
        {
            if (db.Users.Any(u => u.Role == "Admin"))
                return;

            var admin = new User
            {
                Login = "admin",

                Email = "admin@gmail.com",

                Password = BCrypt.Net.BCrypt.HashPassword("Admin1!"),

                Role = "Admin"
            };

            db.Users.Add(admin);

            db.SaveChanges();
        }
    }
}

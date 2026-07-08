using Education_Platform.Data;
using Education_Platform.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Services
{
    public class MaterialService
    {
        public async Task <List<Material>> GetMaterialsAsync(string? search = null)
        {
            using var db = new AppDbContext();

            var query = db.Materials
                .Include(m => m.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(m => m.Title.Contains(search));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Material>> GetMaterialsByAuthorAsync(int userId)
        {
            using var db = new AppDbContext();

            return await db.Materials
                .Include(x => x.Author)
                .Where(m => m.AuthorId == userId)
                .ToListAsync();
        }
    }
}

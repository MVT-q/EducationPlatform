using Education_Platform.Data;
using Education_Platform.Helpers;
using Education_Platform.Models;
using MaterialDesignColors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Services
{
    public class TestService
    {
        public async Task<List<Test>> GetTestsAsync(string? search = null)
        {
            using var db = new AppDbContext();

            var query = db.Tests
                .Include(x => x.Author)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(t => t.Title.Contains(search));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Test>> GetTestsByAuthorAsync(int userId)
        {
            using var db = new AppDbContext();

            return await db.Tests
                .Include(x => x.Author)
                .Where(t => t.AuthorId == userId)
                .ToListAsync();
        }

        public async Task<TestActionResult> DeleteTestAsync(int currentUserId, int testId)
        {
            using var db = new AppDbContext();

            var test = await db.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .Include(t => t.TestAttempts)
                    .ThenInclude(a => a.UserAnswers)
                .FirstOrDefaultAsync(t => t.Id == testId);

            if (test == null)
            {
                return new TestActionResult
                {
                    Success = false,
                    Message = "Test not found"
                };
            }

            if (test.AuthorId != currentUserId)
            {
                return new TestActionResult
                {
                    Success = false,
                    Message = "Access denied"
                };
            }

            foreach (var attempt in test.TestAttempts)
            {
                db.UserAnswers.RemoveRange(attempt.UserAnswers);
            }

            db.TestAttempts.RemoveRange(test.TestAttempts);

            foreach (var question in test.Questions)
            {
                db.Answers.RemoveRange(question.Answers);
            }

            db.Questions.RemoveRange(test.Questions);

            db.Tests.Remove(test);

            await db.SaveChangesAsync();

            return new TestActionResult
            {
                Success = true,
                Message = "Test deleted"
            };
        }
    }
}

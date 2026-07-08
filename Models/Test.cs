using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Models
{
    public class Test
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int AuthorId { get; set; }
        public User? Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public List<Question> Questions { get; set; } = new();
        public int MaxAttempts { get; set; } = 0;
        public List<TestAttempt> TestAttempts { get; set; } = new();
    }
}

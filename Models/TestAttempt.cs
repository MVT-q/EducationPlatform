using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Models
{
    public class TestAttempt
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int TestId { get; set; }
        public Test? Test { get; set; }
        public double Score { get; set; }
        public int Total { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<UserAnswer> UserAnswers { get; set; } = new();
        public double Percentage => Total > 0 ? (Score / Total) * 100 : 0;
    }
}

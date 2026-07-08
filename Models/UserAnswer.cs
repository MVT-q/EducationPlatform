using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Models
{
    public class UserAnswer
    {
        public int Id { get; set; }
        public int TestAttemptId { get; set; }
        public TestAttempt? TestAttempt { get; set; }
        public int QuestionId { get; set; }
        public int? SelectedAnswerId { get; set; }
        public string? TextAnswer { get; set; }
    }
}

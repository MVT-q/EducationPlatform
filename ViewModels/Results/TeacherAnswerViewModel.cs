using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Results
{
    public class TeacherAnswerViewModel
    {
        public string? QuestionText { get; set; }
        public string? StudentAnswer { get; set; }
        public string? CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public string? QuestionType { get; set; }
        public int Number { get; set; }
    }
}

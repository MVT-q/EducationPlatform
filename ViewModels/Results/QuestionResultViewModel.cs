using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Results
{
    public class QuestionResultViewModel
    {
        public string? TextAnswer { get; set; }
        public string? Type { get; set; }
        public string? Text { get; set; }
        public int Number { get; set; }
        public ObservableCollection<AnswerResultViewModel> Answers { get; set; } = new();
    }
}

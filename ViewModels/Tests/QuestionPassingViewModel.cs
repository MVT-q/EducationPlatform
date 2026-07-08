using CommunityToolkit.Mvvm.ComponentModel;
using Education_Platform.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Tests
{
    public partial class QuestionPassingViewModel : ObservableObject
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? Type { get; set; }
        public int Number { get; set; }
        public ObservableCollection<AnswerViewModel> Answers { get; set; } = new();

        [ObservableProperty]
        private string? textAnswer;

        public void OnAnswerSelected(AnswerViewModel selected)
        {
            if (Type == "Single choice")
            {
                foreach (var a in Answers)
                {
                    if (a != selected)
                        a.IsSelected = false;
                }
            }
        }
    }
}

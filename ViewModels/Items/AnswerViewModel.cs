using CommunityToolkit.Mvvm.ComponentModel;
using Education_Platform.ViewModels.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Items
{
    public partial class AnswerViewModel : ObservableObject
    {
        public QuestionPassingViewModel? Parent { get; set; }
        public int Id { get; set; }

        [ObservableProperty]
        private string? text;

        [ObservableProperty]
        private bool isCorrect;

        [ObservableProperty]
        private bool isSelected;

        partial void OnIsSelectedChanged(bool value)
        {
            if (value)
            {
                Parent?.OnAnswerSelected(this);
            }
        }
    }
}

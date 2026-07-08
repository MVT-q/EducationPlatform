using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Items
{
    public partial class QuestionViewModel : ObservableObject
    {
        public ObservableCollection<AnswerViewModel> Answers { get; set; } = new();

        [ObservableProperty]
        private string? text;

        [ObservableProperty]
        private string? type;

        public QuestionViewModel()
        {
            Answers.CollectionChanged += Answers_CollectionChanged;
        }

        [RelayCommand]
        private void AddAnswer()
        {
            Answers.Add(new AnswerViewModel
            {
                Text = "New Answer"
            });
        }

        [RelayCommand]
        private void RemoveAnswer(AnswerViewModel answer)
        {
            Answers.Remove(answer);
        }

        private void Answer_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AnswerViewModel.IsCorrect) && Type == "Single choice")
            {
                var changed = sender as AnswerViewModel;

                if (changed != null && changed.IsCorrect)
                {
                    foreach (var answer in Answers)
                    {
                        if (answer != changed)
                            answer.IsCorrect = false;
                    }
                }
            }
        }

        private void Answers_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (AnswerViewModel answer in e.NewItems)
                {
                    answer.PropertyChanged += Answer_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (AnswerViewModel answer in e.OldItems)
                {
                    answer.PropertyChanged -= Answer_PropertyChanged;
                }
            }
        }

        partial void OnTypeChanged(string? value)
        {
            if (value == "Single choice")
            {
                foreach (var answer in Answers)
                {
                    answer.IsCorrect = false;
                }
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Models;
using Education_Platform.Services;
using Education_Platform.ViewModels.Items;
using Education_Platform.Views.Tests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Tests
{
    public partial class TestCreatorViewModel : ObservableObject
    {
        public ObservableCollection<QuestionViewModel> Questions { get; set; } = new();

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private int maxAttempts;

        [RelayCommand]
        public void Save()
        {
            if (!Validate())
                return;

            var test = BuildTestModel();

            using var db = new AppDbContext();

            db.Tests.Add(test);
            db.SaveChanges();

            MessageBox.Show("Test saved!");

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TestsView());
        }

        [RelayCommand]
        private void AddQuestion()
        {
            Questions.Add(new QuestionViewModel
            {
                Text = "New Question"
            });
        }

        [RelayCommand]
        private void RemoveQuestion(QuestionViewModel question)
        {
            Questions.Remove(question);
        }

        public List<string> Types { get; } = new()
        {
            "Single choice",
            "Multiple choice",
            "Text"
        };

        private Test BuildTestModel()
        {
            if (AuthService.CurrentUserId == null)
                throw new Exception("User not authorized");

            var test = new Test
            {
                Title = Title,
                Description = Description,
                CreatedAt = DateTime.Now,
                AuthorId = AuthService.CurrentUserId.Value,
                MaxAttempts = MaxAttempts
            };

            foreach (var q in Questions)
            {
                var question = new Question
                {
                    Text = q.Text,
                    Type = q.Type
                };

                foreach (var a in q.Answers)
                {
                    question.Answers.Add(new Answer
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect
                    });
                }

                test.Questions.Add(question);
            }
            return test;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Enter title");
                return false;
            }

            if (Questions.Count == 0)
            {
                MessageBox.Show("Add at least one question");
                return false;
            }

            foreach (var q in Questions)
            {
                if (string.IsNullOrWhiteSpace(q.Text))
                {
                    MessageBox.Show("Question text cannot be empty");
                    return false;
                }

                if (q.Type != "Text")
                {
                    if (q.Answers.Count == 0)
                    {
                        MessageBox.Show($"Question '{q.Text}' must have answers");
                        return false;
                    }

                    bool hasCorrect = q.Answers.Any(a => a.IsCorrect);

                    if (!hasCorrect)
                    {
                        MessageBox.Show($"Question '{q.Text}' must have a correct answer");
                        return false;
                    }

                    foreach (var a in q.Answers)
                    {
                        if (string.IsNullOrWhiteSpace(a.Text))
                        {
                            MessageBox.Show($"Question '{q.Text}' has empty answers");
                            return false;
                        }
                    }
                }

                if (q.Type == "Single choice")
                {
                    int correctCount = q.Answers.Count(a => a.IsCorrect);

                    if (correctCount > 1)
                    {
                        MessageBox.Show($"Question '{q.Text}' can have only one correct answer");
                        return false;
                    }
                }
            }

            if (MaxAttempts < 0)
            {
                MessageBox.Show("Max attempts cannot be negative");
                return false;
            }

            return true;
        }
    }
}

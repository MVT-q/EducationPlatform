using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Services;
using Education_Platform.ViewModels.Items;
using Education_Platform.Views.Tests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Tests
{
    public partial class TestDetailsViewModel : ObservableObject
    {
        private int _testId;
        public ObservableCollection<QuestionViewModel> Questions { get; set; } = new();
        public bool IsStudent => AuthService.CurrentUserRole == "Student";
        public int QuestionsCount => Questions.Count;
        public bool HasNoAttempts => !CanStartTest;

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string? attemptsText;

        [ObservableProperty]
        private bool canStartTest = true;

        public TestDetailsViewModel(int testId)
        {
            _testId = testId;
            Load(testId);
        }

        partial void OnCanStartTestChanged(bool value)
        {
            OnPropertyChanged(nameof(HasNoAttempts));
        }

        private void Load(int id)
        {
            using var db = new AppDbContext();

            var test = db.Tests
                .Include(t => t.Questions)
                .ThenInclude(q => q.Answers)
                .FirstOrDefault(t => t.Id == id);

            if (test != null)
            {
                Title = test.Title;
                Description = test.Description;

                int used = db.TestAttempts
                    .Count(a => a.TestId == id && 
                        a.UserId == AuthService.CurrentUserId);

                if (test.MaxAttempts > 0 && used >= test.MaxAttempts)
                    CanStartTest = false;
                else
                    CanStartTest = true;

                if (test.MaxAttempts == 0)
                    AttemptsText = "Unlimited attempts";
                else
                    AttemptsText = $"{used} / {test.MaxAttempts}";

                foreach (var q in test.Questions)
                {
                    Questions.Add(new QuestionViewModel
                    {
                        Text = q.Text,
                        Type = q.Type
                    });
                }
            }
        }

        [RelayCommand]
        private void StartTest()
        {
            using var db = new AppDbContext();

            var test = db.Tests.FirstOrDefault(t => t.Id == _testId);

            if (test == null)
                return;

            int usedAttempts = db.TestAttempts
                .Count(a => a.TestId == _testId
                && a.UserId == AuthService.CurrentUserId);

            if (test.MaxAttempts > 0 && usedAttempts >= test.MaxAttempts)
            {
                MessageBox.Show("No attempts left");
                return;
            }

            int attemptNumber = usedAttempts + 1;
            MessageBox.Show($"Attempt #{attemptNumber}");

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TestPassingView(_testId));
        }
    }
}

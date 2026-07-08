using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.ViewModels.Results;
using Education_Platform.Views.Teacher;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Teacher
{
    public partial class TeacherAttemptDetailsViewModel : ObservableObject
    {
        private readonly int _attemptId;

        private List<int> _attemptIds = new();

        private int _currentIndex;
        public bool CanGoPrevious => _currentIndex > 0;
        public bool CanGoNext => _currentIndex < _attemptIds.Count - 1;
        public ObservableCollection<TeacherAnswerViewModel> Answers { get; set; } = new();

        [ObservableProperty]
        private string? studentLogin;

        [ObservableProperty]
        private int attemptNumber;

        [ObservableProperty]
        private DateTime createdAt;

        public TeacherAttemptDetailsViewModel(int attemptId)
        {
            _attemptId = attemptId;
            Load(attemptId);
        }

        private void Load(int attemptId)
        {
            using var db = new AppDbContext();

            var attempt = db.TestAttempts
                .Include(a => a.Test)
                    .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Answers)
                .Include(a => a.User)
                .Include(a => a.UserAnswers)
                .FirstOrDefault(a => a.Id == attemptId);

            if (attempt == null)
                return;

            StudentLogin = attempt.User?.Login;

            _attemptIds = db.TestAttempts
                .Where(a =>
                    a.UserId == attempt.UserId &&
                    a.TestId == attempt.TestId)
                .OrderBy(a => a.CreatedAt)
                .Select(a => a.Id)
                .ToList();

            _currentIndex = _attemptIds.IndexOf(attempt.Id);

            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));

            var allAttempts = db.TestAttempts
                .Where(a =>
                    a.UserId == attempt.UserId &&
                    a.TestId == attempt.TestId)
                .OrderBy(a => a.CreatedAt)
                .ToList();

            AttemptNumber = allAttempts.FindIndex(a => a.Id == attempt.Id) + 1;

            CreatedAt = attempt.CreatedAt;
            int number = 1;

            foreach (var question in attempt.Test.Questions)
            {
                var userAnswers = attempt.UserAnswers
                    .Where(x => x.QuestionId == question.Id)
                    .ToList();

                if (question.Type == "Single choice")
                {
                    var selected = question.Answers
                        .FirstOrDefault(a => userAnswers
                        .Any(u => u.SelectedAnswerId == a.Id));

                    var correct = question.Answers
                        .FirstOrDefault(a => a.IsCorrect);

                    Answers.Add(new TeacherAnswerViewModel
                    {
                        QuestionText = question.Text,
                        StudentAnswer = selected?.Text,
                        CorrectAnswer = correct?.Text,
                        IsCorrect = selected?.IsCorrect == true,
                        QuestionType = question.Type,
                        Number = number++,
                    });
                }

                else if (question.Type == "Multiple choice")
                {
                    var selectedAnswers = question.Answers
                        .Where(a => userAnswers
                        .Any(u => u.SelectedAnswerId == a.Id))
                        .Select(a => a.Text);

                    var correctAnswers = question.Answers
                        .Where(a => a.IsCorrect)
                        .Select(a => a.Text);

                    bool isCorrect = question.Answers
                        .All(a =>
                            a.IsCorrect ==
                            userAnswers.Any(u => u.SelectedAnswerId == a.Id));

                    Answers.Add(new TeacherAnswerViewModel
                    {
                        QuestionText = question.Text,
                        StudentAnswer = string.Join(", ", selectedAnswers),
                        CorrectAnswer = string.Join(", ", correctAnswers),
                        IsCorrect = isCorrect,
                        QuestionType = question.Type,
                        Number = number++
                    });
                }

                else if (question.Type == "Text")
                {
                    var textAnswer = userAnswers
                        .FirstOrDefault()?.TextAnswer;

                    Answers.Add(new TeacherAnswerViewModel
                    {
                        QuestionText = question.Text,
                        StudentAnswer = textAnswer,
                        CorrectAnswer = "-",
                        IsCorrect = false,
                        QuestionType = question.Type,
                        Number = number++
                    });
                }
            }
        }

        [RelayCommand]
        private void PreviousAttempt()
        {
            if (!CanGoPrevious)
                return;

            int previousId = _attemptIds[_currentIndex - 1];

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherAttemptDetailsView(previousId));
        }

        [RelayCommand]
        private void NextAttempt()
        {
            if (!CanGoNext)
                return;

            int nextId = _attemptIds[_currentIndex + 1];

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherAttemptDetailsView(nextId));
        }

        [RelayCommand]
        private void DeleteAttempt()
        {
            var result = MessageBox.Show(
                "Delete this attempt?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new AppDbContext();

            var attempt = db.TestAttempts
                .Include(a => a.UserAnswers)
                .FirstOrDefault(a => a.Id == _attemptId);

            if (attempt == null)
            {
                MessageBox.Show("Attempt not found");
                return;
            }

            db.UserAnswers.RemoveRange(attempt.UserAnswers);

            db.TestAttempts.Remove(attempt);

            db.SaveChanges();

            MessageBox.Show("Attempt deleted");

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherTestResultsView(attempt.TestId));
        }
    }
}

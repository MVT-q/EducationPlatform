using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Models;
using Education_Platform.Services;
using Education_Platform.ViewModels.Items;
using Education_Platform.Views.Results;
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
    public partial class TestPassingViewModel : ObservableObject
    {
        private int _testId;
        public ObservableCollection<QuestionPassingViewModel> Questions { get; set; } = new();
        public TestPassingViewModel(int testId)
        {
            _testId = testId;
            Load(testId);
        }

        [RelayCommand]
        private void Submit()
        {
            double correct = 0;
            int total = Questions.Count(q => q.Type != "Text");

            foreach (var q in Questions)
            {
                if (q.Type == "Single choice")
                {
                    var selected = q.Answers.FirstOrDefault(a => a.IsSelected);

                    if (selected != null && selected.IsCorrect)
                        correct += 1;
                }

                else if (q.Type == "Multiple choice")
                {
                    int correctAnswers = q.Answers.Count(a => a.IsCorrect);

                    if (correctAnswers == 0)
                        continue;

                    int selectedCorrect = q.Answers.Count(a => a.IsCorrect && a.IsSelected);

                    int selectedWrong = q.Answers.Count(a => !a.IsCorrect && a.IsSelected);

                    if (selectedWrong == 0)
                    {
                        correct += (double)selectedCorrect / correctAnswers;
                    }
                }

                else if (q.Type == "Text")
                {

                }
            }

            double percentage = total > 0 ? (correct / total) * 100 : 0;

            MessageBox.Show($"Result: {correct:F2} / {total} ({percentage:F1}%)");

            using var db = new AppDbContext();

            if (AuthService.CurrentUserId == null)
            {
                MessageBox.Show("User not logged in");
                return;
            }

            var attempt = new TestAttempt
            {
                UserId = AuthService.CurrentUserId.Value,
                TestId = _testId,
                Score = correct,
                Total = total,
                CreatedAt = DateTime.Now,
            };

            db.TestAttempts.Add(attempt);
            db.SaveChanges();

            foreach (var q in Questions)
            {
                if (q.Type == "Single choice")
                {
                    var selected = q.Answers.FirstOrDefault(a => a.IsSelected);

                    if (selected != null)
                    {
                        db.UserAnswers.Add(new UserAnswer
                        {
                            TestAttemptId = attempt.Id,
                            QuestionId = q.Id,
                            SelectedAnswerId = selected.Id
                        });
                    }
                }

                else if (q.Type == "Multiple choice")
                {
                    foreach (var a in q.Answers.Where(a => a.IsSelected))
                    {
                        db.UserAnswers.Add(new UserAnswer
                        {
                            TestAttemptId = attempt.Id,
                            QuestionId = q.Id,
                            SelectedAnswerId = a.Id
                        });
                    }
                }

                else if (q.Type == "Text")
                {
                    if (!string.IsNullOrWhiteSpace(q.TextAnswer))
                    {
                        db.UserAnswers.Add(new UserAnswer
                        {
                            TestAttemptId = attempt.Id,
                            QuestionId = q.Id,
                            TextAnswer = q.TextAnswer
                        });
                    }
                }
            }
            db.SaveChanges();

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new ResultsView());
        }

        private void Load(int id)
        {
            using var db = new AppDbContext();

            var test = db.Tests
                .Include(t => t.Questions)
                    .ThenInclude(q => q.Answers)
                .FirstOrDefault(t => t.Id == id);

            int number = 1;

            foreach (var q in test.Questions)
            {
                var vm = new QuestionPassingViewModel
                {
                    Id = q.Id,
                    Text = q.Text,
                    Type = q.Type,
                    Number = number++
                };

                foreach (var a in q.Answers)
                {
                    var ansverVm = new AnswerViewModel
                    {
                        Id = a.Id,
                        Text = a.Text,
                        IsCorrect = a.IsCorrect,
                        Parent = vm
                    };
                    vm.Answers.Add(ansverVm);
                }

                Questions.Add(vm);
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using Education_Platform.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Results
{
    public partial class ResultDetailsViewModel : ObservableObject
    {
        public ObservableCollection<QuestionResultViewModel> Questions { get; set; } = new();

        public ResultDetailsViewModel(int attemptId)
        {
            Load(attemptId);
        }

        private void Load(int attemptId)
        {
            using var db = new AppDbContext();

            var attempt = db.TestAttempts
                .Include(a => a.Test)
                    .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Answers)
                .Include(a => a.UserAnswers)
                .FirstOrDefault(a => a.Id == attemptId);

            if (attempt == null)
                return;

            int number = 1;

            foreach (var q in attempt.Test.Questions)
            {
                var qvm = new QuestionResultViewModel
                {
                    Text = q.Text,
                    Type = q.Type,
                    Number = number++
                };

                var userAnswers = attempt.UserAnswers
                    .Where(ua => ua.QuestionId == q.Id)
                    .ToList();

                if (q.Type == "Text")
                {
                    qvm.TextAnswer = userAnswers.FirstOrDefault()?.TextAnswer;

                    Questions.Add(qvm);

                    continue;
                }

                foreach (var a in q.Answers)
                {
                    bool isSelected = userAnswers.Any(ua => ua.SelectedAnswerId == a.Id);

                    qvm.Answers.Add(new AnswerResultViewModel
                    {
                        Text = a.Text,
                        IsCorrect = a.IsCorrect,
                        IsSelected = isSelected
                    });
                }

                Questions.Add(qvm);
            }
        }
    }
}

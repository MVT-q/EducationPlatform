using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Models;
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
    public partial class TeacherTestResultsViewModel : ObservableObject
    {
        public ObservableCollection<TestAttempt> Attempts { get; set; } = new();

        [ObservableProperty]
        private string? testTitle;

        [ObservableProperty]
        private double average;

        [ObservableProperty]
        private int studentsCount;

        public TeacherTestResultsViewModel(int testId)
        {
            Load(testId);
        }

        private void Load(int testId)
        {
            using var db = new AppDbContext();

            var attempts = db.TestAttempts
                .Include(a => a.User)
                .Where(a => a.TestId == testId)
                .ToList();

            var test = db.Tests.FirstOrDefault(t => t.Id == testId);

            if (test != null)
            {
                TestTitle = test.Title;
            }

            var bestAttempts = attempts
                .GroupBy(a => a.UserId)
                .Select(g => g.OrderByDescending(x => x.Score).First())
                .OrderByDescending(a => a.Score)
                .ToList();

            if (bestAttempts.Any())
            {
                Average = bestAttempts.Average(a => a.Score);
                StudentsCount = bestAttempts.Count;
            }

            foreach (var a in bestAttempts)
            {
                Attempts.Add(a);
            }
        }

        [RelayCommand]
        private void OpenAttemptDetails(int attemptId)
        {
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherAttemptDetailsView(attemptId));
        }
    }
}

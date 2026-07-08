using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Models;
using Education_Platform.Services;
using Education_Platform.Views.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Results
{
    public partial class ResultsViewModel : ObservableObject
    {
        public ObservableCollection<TestAttempt> Attempts { get; set; } = new();
        public ResultsViewModel()
        {
            Load();
        }

        private void Load()
        {
            using var db = new AppDbContext();

            var attempts = db.TestAttempts.Include(a => a.Test)
                .Where(a => a.UserId == AuthService.CurrentUserId)
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            foreach (var a in attempts)
            {
                Attempts.Add(a);
            }
        }

        [RelayCommand]
        private void OpenDetails(TestAttempt attempt)
        {
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new ResultDetailsView(attempt.Id));
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Services;
using Education_Platform.Views.Teacher;
using Education_Platform.Views.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Tests
{
    public partial class TestCardViewModel : ObservableObject
    {
        private readonly TestService testService = new();
        public bool IsTeacher { get; set; }
        public bool IsStudent { get; set; }
        public bool IsOwner { get; set; }
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? CreatedAt { get; set; }
        public int AuthorId { get; set; }

        [RelayCommand]
        private void Open()
        {
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            if (mainWindow != null)
                mainWindow?.MainFrame.Navigate(new TestDetailsView(Id));
        }

        [RelayCommand]
        private void OpenResults()
        {
            if (AuthService.CurrentUserRole != "Teacher")
            {
                MessageBox.Show("Access denied");
                return;
            }
            else if (AuthorId != AuthService.CurrentUserId)
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherTestResultsView(Id));
        }

        [RelayCommand]
        private async Task Delete()
        {
            var result = MessageBox.Show(
                "Delete this test?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            var actionResult = await testService.DeleteTestAsync(AuthService.CurrentUserId.Value, Id);

            MessageBox.Show(actionResult.Message);

            if (!actionResult.Success)
                return;

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherTestsView());
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Helpers;
using Education_Platform.Services;
using Education_Platform.ViewModels.Tests;
using Education_Platform.Views.Teacher;
using Education_Platform.Views.Tests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Teacher
{
    public partial class TeacherTestsViewModel : ObservableObject
    {
        private readonly TestService testService = new();
        public ObservableCollection<TestCardViewModel> Tests { get; set; } = new();

        public TeacherTestsViewModel()
        {
            _ = LoadTests();
        }

        private async Task LoadTests()
        {
            Tests.Clear();

            var tests = await testService.GetTestsByAuthorAsync(AuthService.CurrentUserId.Value);

            foreach (var test in tests)
            {
                Tests.Add(TestCardFactory.Create(test));
            }
        }

        [RelayCommand]
        private void OpenAllTests()
        {
            if (AuthService.CurrentUserRole != "Teacher")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TestsView());
        }
    }
}

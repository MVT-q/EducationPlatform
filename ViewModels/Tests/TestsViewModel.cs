using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Helpers;
using Education_Platform.Services;
using Education_Platform.Views.Teacher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Tests
{
    public partial class TestsViewModel : ObservableObject
    {
        private readonly TestService testService = new();
        public bool IsTeacher => AuthService.CurrentUserRole == "Teacher";
        public ObservableCollection<TestCardViewModel> Tests { get; set; } = new();

        [ObservableProperty]
        private string? searchText;

        public TestsViewModel()
        {
            _ = LoadTests();
        }

        partial void OnSearchTextChanged(string? value)
        {
            _ = LoadTests();
        }

        private async Task LoadTests()
        {
            Tests.Clear();

            var tests = await testService.GetTestsAsync(SearchText);

            foreach (var test in tests)
            {
                Tests.Add(TestCardFactory.Create(test));
            }
        }

        [RelayCommand]
        private void OpenTeacherTests()
        {
            if (AuthService.CurrentUserRole != "Teacher")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherTestsView());
        }
    }
}

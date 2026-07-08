using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Services;
using Education_Platform.Views.Admin;
using Education_Platform.Views.Dashboard;
using Education_Platform.Views.Materials;
using Education_Platform.Views.Profile;
using Education_Platform.Views.Results;
using Education_Platform.Views.Teacher;
using Education_Platform.Views.Tests;
using Education_Platform.Views.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Windows
{
    public partial class MainViewModel : ObservableObject
    {
        public bool IsAdmin => AuthService.CurrentUserRole == "Admin";
        public bool IsTeacher => AuthService.CurrentUserRole == "Teacher";
        public bool IsStudent => AuthService.CurrentUserRole == "Student";

        [ObservableProperty]
        private string? login;

        [ObservableProperty]
        private string? role;

        [ObservableProperty]
        private string? avatarPath;

        public MainViewModel()
        {
            RefreshUserInfo();
        }

        public void RefreshRoles()
        {
            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsTeacher));
            OnPropertyChanged(nameof(IsStudent));
        }

        public void RefreshUserInfo()
        {
            using var db = new AppDbContext();

            var user = db.Users.FirstOrDefault(u => u.Id == AuthService.CurrentUserId);

            if (user == null)
                return;

            Login = user.Login;

            Role = user.Role;

            AvatarPath = user.AvatarPath;

            OnPropertyChanged(nameof(IsAdmin));
            OnPropertyChanged(nameof(IsTeacher));
            OnPropertyChanged(nameof(IsStudent));
        }

        [RelayCommand]
        private void OpenDashboard()
        {
            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new DashboardView());
        }

        [RelayCommand]
        private void OpenAdminPanel()
        {
            if(AuthService.CurrentUserRole != "Admin")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new AdminPanelView());
        }

        [RelayCommand]
        private void OpenTestsPanel()
        {
            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TestsView());
        }

        [RelayCommand]
        private void OpenTestsCreator()
        {
            if (AuthService.CurrentUserRole != "Teacher")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TestCreatorView());
        }

        [RelayCommand]
        private void OpenResults()
        {
            if (AuthService.CurrentUserRole != "Student")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new ResultsView());
        }

        

        [RelayCommand]
        private void OpenMaterialsPanel()
        {
            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new MaterialsView());
        }

        [RelayCommand]
        private void OpenMaterialsCreator()
        {
            if (AuthService.CurrentUserRole != "Teacher")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new MaterialCreatorView());
        }

        [RelayCommand]
        private void OpenProfile()
        {
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new ProfileView());
        }

        [RelayCommand]
        private void Logout()
        {
            AuthService.CurrentUserId = null;
            AuthService.CurrentUserRole = null;

            var authWindow = new AuthWindow();
            authWindow.Show();

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.Close();
        }
    }
}

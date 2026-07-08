using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Services;
using Education_Platform.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Profile
{
    public partial class ProfileViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? login;

        [ObservableProperty]
        private string? newLogin;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string? role;

        [ObservableProperty]
        private string? avatarPath;


        public ProfileViewModel()
        {
            Load();
        }

        private void Load()
        {
            using var db = new AppDbContext();

            var user = db.Users.FirstOrDefault(u => u.Id == AuthService.CurrentUserId);

            if (user != null)
            {
                Login = user.Login;
                NewLogin = user.Login;
                Email = user.Email;
                Role = user.Role;
                AvatarPath = user.AvatarPath;
            }
        }

        [RelayCommand]
        private void ChooseAvatar()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            dialog.Filter = "Image files|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() != true)
                return;

            string avatarsFolder = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.LocalApplicationData),
                "Education Platform",
                "Avatars");

            Directory.CreateDirectory(avatarsFolder);

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(dialog.FileName)}";

            string destination = Path.Combine(avatarsFolder, fileName);

            File.Copy(dialog.FileName, destination, true);

            using var db = new AppDbContext();

            var user = db.Users.FirstOrDefault(u => u.Id == AuthService.CurrentUserId);

            if (user == null)
                return;

            user.AvatarPath = destination;

            db.SaveChanges();

            AvatarPath = destination;

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            (mainWindow?.DataContext as MainViewModel)?.RefreshUserInfo();
        }

        [RelayCommand]
        private void ChangeLogin()
        {
            var validator = new ValidationService();

            var validation = validator.ValidateLogin(NewLogin);

            if (!validation.Success)
            {
                MessageBox.Show(validation.Message);
                return;
            }

            using var db = new AppDbContext();

            bool exists = db.Users
                .Any(u =>
                    u.Login == NewLogin && 
                    u.Id != AuthService.CurrentUserId);

            if (exists)
            {
                MessageBox.Show("Login already taken");
                return;
            }

            var user = db.Users.FirstOrDefault(u => u.Id == AuthService.CurrentUserId);

            if (user == null)
                return;

            user.Login = NewLogin;

            db.SaveChanges();

            Login = NewLogin;

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            (mainWindow?.DataContext as MainViewModel)?.RefreshUserInfo();

            MessageBox.Show("Login updated");
        }
    }
}

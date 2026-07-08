using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Services;
using Education_Platform.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Windows
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly ValidationService validationService = new();

        private readonly AuthService authService = new();

        [ObservableProperty]
        private string? login;

        [ObservableProperty]
        private string? password;

        [ObservableProperty]
        private string? confirmPassword;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private bool isRegisterMode;

        [ObservableProperty]
        private string? loginError;

        [ObservableProperty]
        private string? passwordError;

        [ObservableProperty]
        private string? confirmPasswordError;

        [ObservableProperty]
        private string? emailError;

        [RelayCommand]
        private void SwitchToRegister()
        {
            ClearForm();

            (Application.Current.Windows
                .OfType<AuthWindow>()
                .FirstOrDefault())
                ?.ClearPasswordBoxes();

            IsRegisterMode = true;
        }

        [RelayCommand]
        private void SwitchToLogin()
        {
            ClearForm();

            (Application.Current.Windows
                .OfType<AuthWindow>()
                .FirstOrDefault())
                ?.ClearPasswordBoxes();

            IsRegisterMode = false;
        }

        [RelayCommand]
        private async Task Submit()
        {
            ClearErrors();

            if (IsRegisterMode)
            {
                await Register();
            }
            else
            {
                await LoginUser();
            }
        }

        private void ClearForm()
        {
            Login = "";
            Password = "";
            ConfirmPassword = "";
            Email = "";

            ClearErrors();
        }

        private void ClearErrors()
        {
            LoginError = "";
            PasswordError = "";
            ConfirmPasswordError = "";
            EmailError = "";
        }

        private async Task Register()
        {
            var loginValidation = validationService.ValidateLogin(Login);

            if (!loginValidation.Success)
            {
                LoginError = loginValidation.Message;
                return;
            }

            var passwordValidation = validationService.ValidatePassword(Password);

            if (!passwordValidation.Success)
            {
                PasswordError = passwordValidation.Message;
                return;
            }

            var confirmValidation = validationService.ValidateConfirmPassword(Password, ConfirmPassword);

            if (!confirmValidation.Success)
            {
                ConfirmPasswordError = confirmValidation.Message;
                return;
            }

            var emailValidation = validationService.ValidateEmail(Email);

            if (!emailValidation.Success)
            {
                EmailError = emailValidation.Message;
                return;
            }

            var result = await authService.RegisterAsync(Login, Password, Email);

            if (!result.Success)
            {
                MessageBox.Show(result.Message);
                return;
            }

            await authService.LoginAsync(Login, Password);

            MessageBox.Show(result.Message);

            OpenMainWindow();
        }

        private async Task LoginUser()
        {
            var result = await authService.LoginAsync(Login, Password);

            if (!result.Success)
            {
                PasswordError = result.Message;
                return;
            }

            MessageBox.Show(result.Message);

            OpenMainWindow();
        }

        private void OpenMainWindow()
        {
            var mainWindow = new MainWindow();

            mainWindow.Show();

            (mainWindow.DataContext as MainViewModel)?.RefreshRoles();

            Application.Current.Windows
                .OfType<AuthWindow>()
                .FirstOrDefault()
                ?.Close();
        }
    }
}

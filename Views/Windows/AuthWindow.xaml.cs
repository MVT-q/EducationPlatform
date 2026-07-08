using Education_Platform.ViewModels.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Education_Platform.Views.Windows
{
    public partial class AuthWindow : Window
    {
        public AuthWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AuthViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AuthViewModel vm)
            {
                vm.ConfirmPassword = ConfirmPasswordBox.Password;
            }
        }

        private void TogglePassword_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordVisibleBox.Visibility == Visibility.Collapsed)
            {
                PasswordVisibleBox.Text = PasswordBox.Password;

                PasswordVisibleBox.Visibility = Visibility.Visible;

                PasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                PasswordBox.Password = PasswordVisibleBox.Text;

                PasswordVisibleBox.Visibility = Visibility.Collapsed;

                PasswordBox.Visibility = Visibility.Visible;
            }
        }

        private void ToggleConfirmPassword_Click(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordVisibleBox.Visibility == Visibility.Collapsed)
            {
                ConfirmPasswordVisibleBox.Text = ConfirmPasswordBox.Password;

                ConfirmPasswordVisibleBox.Visibility = Visibility.Visible;

                ConfirmPasswordBox.Visibility = Visibility.Collapsed;
            }
            else
            {
                ConfirmPasswordBox.Password = ConfirmPasswordVisibleBox.Text;

                ConfirmPasswordVisibleBox.Visibility = Visibility.Collapsed;

                ConfirmPasswordBox.Visibility = Visibility.Visible;
            }
        }

        public void ClearPasswordBoxes()
        {
            PasswordBox.Clear();
            ConfirmPasswordBox.Clear();

            PasswordVisibleBox.Clear();
            ConfirmPasswordVisibleBox.Clear();
        }
    }   
}

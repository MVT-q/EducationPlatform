using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Migrations;
using Education_Platform.Models;
using Education_Platform.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Admin
{
    public partial class AdminPanelViewModel : ObservableObject
    {
        private readonly UserService userService = new();

        [ObservableProperty]
        private ObservableCollection<User> users = new();

        [ObservableProperty]
        private string? searchText;

        public AdminPanelViewModel()
        {
            _ = LoadUsers();
        }      

        private async Task LoadUsers()
        {
            await Refresh();
        }    

        partial void OnSearchTextChanged(string? value)
        {
            _ = Refresh();
        }

        [RelayCommand]
        private async Task Refresh()
        {
            Users.Clear();

            var users = await userService.GetUsersAsync(SearchText);

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        [RelayCommand]
        private async Task Delete(User user)
        {
            var result = MessageBox.Show(
                $"Delete user {user.Login}?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            var actionResult = await userService.DeleteUserAsync(AuthService.CurrentUserId.Value, user.Id);

            MessageBox.Show(actionResult.Message);

            if (!actionResult.Success)
                return;

            Users.Remove(user);
        }

        [RelayCommand]
        private async Task UpdateRole(User user)
        {
            var result = await userService.UpdateRoleAsync(AuthService.CurrentUserId.Value, user.Id, user.Role);

            if (!result.Success)
            {
                MessageBox.Show(result.Message);

                await Refresh();

                return;
            }
        }

        public List<string> Roles { get; } = new()
        {
            "Student",
            "Teacher",
            "Admin"
        };
    }
}

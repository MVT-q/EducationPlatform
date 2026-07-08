using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Services;
using Education_Platform.ViewModels.Materials;
using Education_Platform.Views.Materials;
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
    public partial class TeacherMaterialsViewModel : ObservableObject
    {
        private readonly MaterialService materialService = new();
        public ObservableCollection<MaterialCardViewModel> Materials { get; set; } = new();

        public TeacherMaterialsViewModel()
        {
            _ = LoadMaterials();
        }

        private async Task LoadMaterials()
        {
            Materials.Clear();

            var materials = await materialService.GetMaterialsByAuthorAsync(AuthService.CurrentUserId.Value);

            foreach (var material in materials)
            {
                Materials.Add(new MaterialCardViewModel
                {
                    Id = material.Id,
                    Title = material.Title,
                    Description = material.Description,
                    Author = material.Author?.Login,
                    Color = material.Color,
                    AuthorId = material.AuthorId
                });
            }
        }

        [RelayCommand]
        private void OpenAllMaterials()
        {
            if (AuthService.CurrentUserRole != "Teacher")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new MaterialsView());
        }
    }
}

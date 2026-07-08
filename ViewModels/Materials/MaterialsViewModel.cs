using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Services;
using Education_Platform.Views.Teacher;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Materials
{
    public partial class MaterialsViewModel : ObservableObject
    {
        private readonly MaterialService materialService = new();
        public bool IsTeacher => AuthService.CurrentUserRole == "Teacher";
        public ObservableCollection<MaterialCardViewModel> Materials { get; set; } = new();

        [ObservableProperty]
        private string? searchText;

        public MaterialsViewModel()
        {
            _ = LoadMaterials();
        }

        partial void OnSearchTextChanged(string? value)
        {
            _ = LoadMaterials();
        }

        private async Task LoadMaterials()
        {
            Materials.Clear();

            var materials = await materialService.GetMaterialsAsync(SearchText);

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
        private void OpenTeacherMaterials()
        {
            if (AuthService.CurrentUserRole != "Teacher")
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new TeacherMaterialsView());
        }
    }
}

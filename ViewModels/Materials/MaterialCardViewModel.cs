using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Services;
using Education_Platform.Views.Materials;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Materials
{
    public partial class MaterialCardViewModel : ObservableObject
    {
        public bool IsOwner => AuthorId == AuthService.CurrentUserId;
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Color { get; set; }
        public int AuthorId { get; set; }

        [RelayCommand]
        private void Open()
        {
            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new MaterialDetailsView(Id));
        }

        [RelayCommand]
        private void Edit()
        {
            if (!IsOwner)
            {
                MessageBox.Show("Access denied");
                return;
            }

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new MaterialCreatorView(Id));
        }

        [RelayCommand]
        private void Delete()
        {
            if (!IsOwner)
            {
                MessageBox.Show("Denied");
                return;
            }

            var result = MessageBox.Show(
                "Delete this material?",
                "Confirm",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes)
                return;

            using var db = new AppDbContext();

            var material = db.Materials
                .Include(m => m.Resources)
                .FirstOrDefault(m => m.Id == Id);

            if (material == null)
            {
                MessageBox.Show("Material not found");
                return;
            }

            foreach (var resource in material.Resources)
            {
                if (resource.Type == "File")
                {
                    try
                    {
                        string baseFolder = Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "Education Platform",
                            "Files");

                        string path = Path.Combine(baseFolder, resource.Url ?? "");

                        if (File.Exists(path))
                        {
                            bool usedSomewhereElse = db.MaterialResources
                                .Any(r => r.Url == resource.Url && 
                                    r.Id != resource.Id);

                            if (!usedSomewhereElse)
                            {
                                File.Delete(path);
                            }
                            else
                            {
                                MessageBox.Show("File in this material is used in other materials");
                            }
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Failed to delete file");
                    }
                }
            }

            db.Materials.Remove(material);
            db.SaveChanges();

            MessageBox.Show("Material deleted");

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new MaterialsView());
        }
    }
}

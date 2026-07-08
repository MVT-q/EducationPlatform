using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Models;
using Education_Platform.Services;
using Education_Platform.ViewModels.Items;
using Education_Platform.Views.Materials;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Materials
{
    public partial class MaterialCreatorViewModel : ObservableObject
    {
        private readonly int? _materialId;
        public ObservableCollection<ResourceViewModel> Resources { get; set; } = new();

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string? content;

        [ObservableProperty]
        private string? color = "#3F51B5";

        public MaterialCreatorViewModel(int? materialId = null)
        {
            _materialId = materialId;

            if (_materialId != null)
                Load();
        }

        private void Load()
        {
            using var db = new AppDbContext();

            var material = db.Materials
                .Include(m => m.Resources)
                .FirstOrDefault(m => m.Id == _materialId);

            if (material == null)
            {
                MessageBox.Show("Material not found");
                return;
            }

            Title = material.Title;
            Description = material.Description;
            Content = material.Content;
            Color = material.Color;

            if (material.Resources != null)
            {
                foreach (var r in material.Resources)
                {
                    Resources.Add(new ResourceViewModel
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Url = r.Url,
                        Type = r.Type
                    });
                }
            }
        }

        [RelayCommand]
        private void AddResource()
        {
            Resources.Add(new ResourceViewModel
            {
                Name = "New Resource",
                Url = "",
                Type = "Link"
            });
        }

        [RelayCommand]
        private void RemoveResource(ResourceViewModel resource)
        {
            if (resource != null)
                Resources.Remove(resource);
        }

        [RelayCommand]
        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                MessageBox.Show("Enter title");
                return;
            }

            if (AuthService.CurrentUserId == null)
            {
                MessageBox.Show("User not logged in");
                return;
            }

            using var db = new AppDbContext();

            Material? material;

            if (_materialId != null)
            {
                material = db.Materials
                    .Include(m => m.Resources)
                    .FirstOrDefault(m => m.Id == _materialId);

                if (material == null)
                {
                    MessageBox.Show("Material not found");
                    return;
                }

                material.Title = Title;
                material.Description = Description;
                material.Content = Content;
                material.Color = Color;
                material.Resources.Clear();

                foreach (var r in Resources)
                {
                    material.Resources.Add(new MaterialResource
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Url = r.Url,
                        Type = r.Type
                    });
                }
            }

            else
            {
                material = new Material
                {
                    Title = Title,
                    Description = Description,
                    Content = Content,
                    Color = Color,
                    AuthorId = AuthService.CurrentUserId.Value
                };

                foreach (var r in Resources)
                {
                    material.Resources.Add(new MaterialResource
                    {
                        Name = r.Name,
                        Url = r.Url,
                        Type = r.Type
                    });
                }

                db.Materials.Add(material);
            }

            db.SaveChanges();

            MessageBox.Show("Material saved!");

            var mainWindow = Application.Current.Windows
                .OfType<MainWindow>()
                .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new MaterialsView());
        }

        public List<string> ResourceTypes { get; } = new()
        {
            "Link",
            "File",
            "Video"
        };

        public List<string> Colors { get; } = new()
        {
            "#3F51B5",
            "#4CAF50",
            "#2196F3",
            "#FF9800",
            "#F44336",
            "#607D8B",
            "#795548",
            "#000000"
        };
    }
}

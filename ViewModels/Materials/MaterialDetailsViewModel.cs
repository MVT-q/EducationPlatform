using CommunityToolkit.Mvvm.ComponentModel;
using Education_Platform.Data;
using Education_Platform.ViewModels.Items;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Materials
{
    public partial class MaterialDetailsViewModel : ObservableObject
    {
        private readonly int _materialId;
        public ObservableCollection<ResourceViewModel> Resources { get; set; } = new();

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string? content;

        public MaterialDetailsViewModel(int materialId)
        {
            _materialId = materialId;
            Load();
        }

        private void Load()
        {
            using var db = new AppDbContext();

            var material = db.Materials
                .Include(m => m.Resources)
                .FirstOrDefault(m => m.Id == _materialId);

            if (material != null)
            {
                Title = material.Title;
                Content = material.Content;
                Description = material.Description;

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
    }
}

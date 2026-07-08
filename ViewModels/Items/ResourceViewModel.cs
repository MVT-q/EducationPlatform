using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Education_Platform.ViewModels.Items
{
    public partial class ResourceViewModel : ObservableObject
    {
        public int Id { get; set; }

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? url;

        [ObservableProperty]
        private string? type;

        [RelayCommand]
        private void ChooseFile()
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            if (dialog.ShowDialog() == true)
            {
                string selectedFile = dialog.FileName;

                string folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Education Platform",
                    "Files");

                Directory.CreateDirectory(folder);

                string fileName = Path.GetFileName(selectedFile);

                string uniqueName = $"{Guid.NewGuid()}_{fileName}";

                string destination = Path.Combine(folder, uniqueName);

                File.Copy(selectedFile, destination, true);

                Url = uniqueName;

                if (string.IsNullOrWhiteSpace(Name))
                    Name = fileName;
            }
        }

        public string Icon
        {
            get
            {
                return Type switch
                {
                    "File" => "📄",
                    "Video" => "🎥",
                    _ => "🔗"
                };
            }
        }
    }
}

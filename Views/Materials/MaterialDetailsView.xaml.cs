using Education_Platform.ViewModels.Items;
using Education_Platform.ViewModels.Materials;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Navigation;

namespace Education_Platform.Views.Materials
{
    public partial class MaterialDetailsView : UserControl
    {
        public MaterialDetailsView(int materialId)
        {
            InitializeComponent();
            DataContext = new MaterialDetailsViewModel(materialId);
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Hyperlink hyperlink &&
                hyperlink.DataContext is ResourceViewModel resource)
            {
                string path = resource.Url ?? "";

                bool isWebLink =
                    path.StartsWith("http://") ||
                    path.StartsWith("https://");

                if (!isWebLink)
                {
                    string baseFolder = Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Education Platform",
                        "Files");

                    path = Path.Combine(baseFolder, path);

                    if (!File.Exists(path))
                    {
                        MessageBox.Show("File not found");
                        return;
                    }
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = path,
                    UseShellExecute = true
                });
            }
        }
    }
}

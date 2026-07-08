using Education_Platform.Data;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Education_Platform
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            DbInitializer.Initialize();
            base.OnStartup(e);
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Education_Platform.Data;
using Education_Platform.Services;
using Education_Platform.Views.Dashboard;
using Education_Platform.Views.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Education_Platform.ViewModels.Dashboard
{
    public partial class DashboardViewModel : ObservableObject
    {
        public bool IsAdmin => AuthService.CurrentUserRole == "Admin";
        public bool IsTeacher => AuthService.CurrentUserRole == "Teacher";
        public bool IsStudent => AuthService.CurrentUserRole == "Student";

        [ObservableProperty]
        private int testsCount;

        [ObservableProperty]
        private int materialsCount;

        [ObservableProperty]
        private int passedTestsCount;

        [ObservableProperty]
        private int usersCount;

        [ObservableProperty]
        private int teachersCount;

        [ObservableProperty]
        private int studentsCount;

        [ObservableProperty]
        private int createdTestsCount;

        [ObservableProperty]
        private int createdMaterialsCount;

        public DashboardViewModel()
        {
            Load();
        }

        private void Load()
        {
            using var db = new AppDbContext();

            if (AuthService.CurrentUserId != null)
            {
                int currentUserId = AuthService.CurrentUserId.Value;

                TestsCount = db.Tests.Count(t => t.MaxAttempts == 0 || 
                    db.TestAttempts.Count(a => a.TestId == t.Id && 
                        a.UserId == currentUserId) < t.MaxAttempts);
            }

            MaterialsCount = db.Materials.Count();

            if (AuthService.CurrentUserId != null)
            {
                PassedTestsCount = db.TestAttempts.Count(x => x.UserId == AuthService.CurrentUserId);

                CreatedTestsCount = db.Tests.Count(x => x.AuthorId == AuthService.CurrentUserId);

                CreatedMaterialsCount = db.Materials.Count(x => x.AuthorId == AuthService.CurrentUserId);
            }

            UsersCount = db.Users.Count();

            TeachersCount = db.Users.Count(x => x.Role == "Teacher");

            StudentsCount = db.Users.Count(x => x.Role == "Student");
        }

        [RelayCommand]
        private void OpenReference()
        {
            var mainWindow = Application.Current.Windows
               .OfType<MainWindow>()
               .FirstOrDefault();

            mainWindow?.MainFrame.Navigate(new ReferenceView());
        }
    }
}

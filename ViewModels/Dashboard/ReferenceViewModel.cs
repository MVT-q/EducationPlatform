using CommunityToolkit.Mvvm.ComponentModel;
using Education_Platform.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.ViewModels.Dashboard
{
    public partial class ReferenceViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? content;

        public ReferenceViewModel()
        {
            Load();
        }

        private void Load()
        {
            switch (AuthService.CurrentUserRole)
            {
                case "Admin":

                    Title = "Administrator Reference";

                    Content =
                        "After logging in, the administrator gains access to the main system modules and the user management panel. The side menu allows navigation between the available sections of the application.\n\n" +

                        "The administrator's primary responsibility is managing user accounts. The corresponding module provides access to the user list, search functionality by username or email address, user role management, and the ability to delete accounts from the system.\n\n" +

                        "The administrator can also view learning materials and tests, use the profile module to change their username and set an avatar, and log out of the system.\n\n" +

                        "The administrator role is designed to oversee users and ensure the proper operation of the learning platform.";

                    break;

                case "Teacher":

                    Title = "Teacher Reference";

                    Content =
                        "After logging into the system, the teacher gains access to modules for working " +
                        "with tests, learning materials, and student results.\n\n" +

                        "In the test creation module, the teacher can add new tests and create questions " +
                        "with a single correct answer, multiple correct answers, or text-based questions. " +
                        "After creation, the test is saved to the database and becomes available to students.\n\n" +

                        "In the results section, the teacher can view student test results, open detailed " +
                        "answer reviews, view individual test attempts, and delete attempts if necessary.\n\n" +

                        "In the learning materials module, the teacher can create, edit, and delete materials. " +
                        "The system supports adding text, external links, video materials, and files of various formats. " +
                        "Filtering of personal tests and materials is available for quick search.\n\n" +

                        "On the profile page, the teacher can change the login and set a profile avatar.";

                    break;

                case "Student":

                    Title = "Student Reference";

                    Content =
                        "After authorization, the student gains access to learning materials, tests, and test results.\n\n" +

                        "In the tests section, the user can view the list of available tests, open detailed information, " +
                        "and complete test attempts. After finishing the test, the results are automatically saved to the database.\n\n" +

                        "In the results module, the student can view received scores, the number of correct answers, " +
                        "and the results of previous attempts.\n\n" +

                        "In the learning materials section, the student can view text materials, external links, " +
                        "videos, and additional files.\n\n" +

                        "To change personal data, the student can open the profile page, change the login, " +
                        "or set a profile avatar. The logout button is used to finish working with the system.";

                    break;
            }
        }
    }
}

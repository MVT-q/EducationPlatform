using Education_Platform.Models;
using Education_Platform.Services;
using Education_Platform.ViewModels.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Education_Platform.Helpers
{
    public static class TestCardFactory
    {
        public static TestCardViewModel Create(Test test)
        {
            return new TestCardViewModel
            {
                Id = test.Id,
                Title = test.Title,
                Author = test.Author?.Login,
                AuthorId = test.AuthorId,
                CreatedAt = test.CreatedAt.ToString("dd.MM.yyyy"),

                IsTeacher = AuthService.CurrentUserRole == "Teacher",

                IsStudent = AuthService.CurrentUserRole == "Student",

                IsOwner = test.AuthorId == AuthService.CurrentUserId
            };
        }
    }
}

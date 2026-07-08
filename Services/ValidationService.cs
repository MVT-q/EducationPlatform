using Education_Platform.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Education_Platform.Services
{
    public class ValidationService
    {
        public AppValidationResult ValidateLogin(string? login)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Login is required"
                };
            }

            if (login.Length < 4 || login.Length > 16)
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Login must be 4-16 characters"
                };
            }

            if (!Regex.IsMatch(login, @"^[a-zA-Z0-9_]+$"))
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Only letters, numbers and _ allowed"
                };
            }

            return new AppValidationResult
            {
                Success = true
            };
        }

        public AppValidationResult ValidatePassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Password is required"
                };
            }

            if (password.Length < 6 || password.Length > 16)
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Password must be 6-16 characters"
                };
            }

            if (!Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$"))
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Must contain upper, lower, digit and special char"
                };
            }

            return new AppValidationResult
            {
                Success = true
            };
        }

        public AppValidationResult ValidateEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Email is required"
                };
            }

            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Invalid email format"
                };
            }

            return new AppValidationResult
            {
                Success = true
            };
        }

        public AppValidationResult ValidateConfirmPassword(string? password, string? confirmPassword)
        {
            if (password != confirmPassword)
            {
                return new AppValidationResult
                {
                    Success = false,
                    Message = "Passwords do not match"
                };
            }

            return new AppValidationResult
            {
                Success = true
            };
        }
    }
}

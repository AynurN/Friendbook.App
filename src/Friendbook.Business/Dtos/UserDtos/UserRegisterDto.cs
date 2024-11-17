using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Friendbook.Business.Exceptions.LoginRegisterExceptions;
using Microsoft.AspNetCore.Http;

namespace Friendbook.Business.Dtos.UserDtos
{
    public record UserRegisterDto(string FullName, string Email, string Password, string ConfirmPassword);
    public static class UserValidationHelper
    {
        public static void ValidateUserRegistration(UserRegisterDto userRegisterDto)
        {
            // Validate ConfirmPassword
            if (userRegisterDto.Password != userRegisterDto.ConfirmPassword)
            {
                throw new PasswordsDoNotMatchException(
                    StatusCodes.Status400BadRequest,
                    "ConfirmPassword",
                    "Passwords do not match");
            }

        
            if (!IsValidEmail(userRegisterDto.Email))
            {
                throw new InvalidEmailFormatException(
                    StatusCodes.Status400BadRequest,
                    "Email",
                    "Invalid email format.");
            }

            if (!IsValidPassword(userRegisterDto.Password))
            {
                throw new WeakPasswordException(
                    StatusCodes.Status400BadRequest,
                    "Password",
                    "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one number, and one special character.");
            }
        }

        public static bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return emailRegex.IsMatch(email);
        }

        public static bool IsValidPassword(string password)
        {
       
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            return passwordRegex.IsMatch(password);
        }
    }

    public class ValidateUserRegistrationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.TryGetValue("userRegisterDto", out var value) && value is UserRegisterDto userRegisterDto)
            {
                try
                {
                    UserValidationHelper.ValidateUserRegistration(userRegisterDto);
                }
                catch (Exception ex)
                {
                    context.Result = new BadRequestObjectResult(new { error = ex.Message });
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }


}


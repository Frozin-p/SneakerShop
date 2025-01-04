using System.Text.RegularExpressions;

namespace AuthService.Services
{
    public static class PasswordValidator
    {
        public static bool IsValid(string password, out string errorMessage)
        {
            if (password.Length < 6)
            {
                errorMessage = "Password must be at least 6 characters long.";
                return false;
            }

            if (!Regex.IsMatch(password, @"[A-Za-z]"))
            {
                errorMessage = "Password must contain at least one letter.";
                return false;
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                errorMessage = "Password must contain at least one number.";
                return false;
            }

            errorMessage = string.Empty;
            return true;
        }
    }
}

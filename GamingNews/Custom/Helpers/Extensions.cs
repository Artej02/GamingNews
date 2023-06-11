using System;
using System.ComponentModel.DataAnnotations;

namespace GamingWeb.Custom.Helpers
{
    public static class Extensions
    {
        public static bool IsValidEmail(this string email)
        {
            var validator = new EmailAddressAttribute();
            return !String.IsNullOrWhiteSpace(email) && validator.IsValid(email);
        }

        public static bool IsValidPhoneNumber(this string phonenumber)
        {
            var validator = new PhoneAttribute();
            return !String.IsNullOrWhiteSpace(phonenumber) && validator.IsValid(phonenumber);
        }

        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}

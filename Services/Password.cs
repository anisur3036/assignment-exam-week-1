using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace EmailSendingApiApp.Services
{
    public class Password
    {
        public readonly PasswordHasher<string> _passwordHasher;

        public Password()
        {
            _passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword("anis", password);
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword("anis", hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}
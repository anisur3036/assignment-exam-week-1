using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailSendingApiApp.Data;
using EmailSendingApiApp.Models;
using EmailSendingApiApp.Services;
using EmailSendingApiApp.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace EmailSendingApiApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public RegisterController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterUser user)
        {
            // Check if the user already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            var passwordUtility = new Password();
            var hashPassword = passwordUtility.HashPassword(user.Password);
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = hashPassword
            };
            // Add the new user to the database
            _context.Users.Add(newUser);
            _context.SaveChanges();

            await SendEmailAsync(user.Email, "Registration Confirmation", $"Hello {user.Name},\n\nThank you for registering with us.\n\nBest regards,\nAnisur Rahman");

            return Ok("User registered successfully.");
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("anisurlist3@gmail.com"));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var smtp = new SmtpClient())
            {
                await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync("anisurlist3@gmail.com", "vbnc vkdo yiwm gdmx");
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }


        }

    }
}
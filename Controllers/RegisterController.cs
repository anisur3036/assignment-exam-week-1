using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailSendingApiApp.Data;
using EmailSendingApiApp.Models;
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
        public IActionResult Register([FromBody] User user)
        {
            if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("Invalid user data.");
            }

            // Check if the user already exists
            var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return Conflict("User already exists.");
            }

            // Add the new user to the database
            _context.Users.Add(user);
            _context.SaveChanges();

            // email sending
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("anisur3036@gmail.com"));
            email.To.Add(MailboxAddress.Parse(user.Email));
            email.Subject = "Registration Confirmation";
            email.Body = new TextPart("plain")
            {
                Text = $"Hello {user.Name},\n\nThank you for registering with us.\n\nBest regards,\nAnisur Rahman"
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("anisurlist3@gmail.com", "vbnc vkdo yiwm gdmx");
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            return Ok("User registered successfully.");
        }

    }
}
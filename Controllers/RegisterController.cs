using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailSendingApiApp.Data;
using EmailSendingApiApp.Models;
using Microsoft.AspNetCore.Mvc;

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

            return Ok("User registered successfully.");
            //return CreatedAtAction(nameof(Register), new { id = user.Id }, user);
        }

    }
}
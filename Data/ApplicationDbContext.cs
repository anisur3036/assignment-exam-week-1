using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailSendingApiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailSendingApiApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
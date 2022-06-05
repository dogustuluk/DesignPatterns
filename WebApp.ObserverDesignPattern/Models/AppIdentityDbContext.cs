using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.ObserverDesignPattern.Models;

namespace BaseProject.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) //buradaki options'u startup.cs tarafında dolduruyor olacağız.
        {

        }
        public DbSet<Discount> Discounts { get; set; }
    }
}

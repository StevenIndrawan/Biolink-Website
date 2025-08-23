using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BioLinkWeb.Models;

namespace BioLinkWeb.Data
{
    public class _AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public _AppDbContext(DbContextOptions<_AppDbContext> options) : base(options) { }

        public DbSet<Link> Links { get; set; }
    }
}

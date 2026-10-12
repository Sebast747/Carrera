using Microsoft.EntityFrameworkCore;
using CarreraCFE.Models;

namespace CarreraCFE.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Corredor> Corredores { get; set; }
    }
}
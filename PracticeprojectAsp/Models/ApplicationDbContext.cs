using Microsoft.EntityFrameworkCore;

namespace PracticeprojectAsp.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<products> products { get; set; }
        public DbSet<equity> equities { get; set; }
        public DbSet<student> students { get; set; }
        public DbSet<Teacher> teachers { get; set; }
    }
}

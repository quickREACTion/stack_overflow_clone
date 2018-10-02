using Microsoft.EntityFrameworkCore;
 
namespace CoderHelp.Models
{
    public class UserContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        public DbSet<Users> users { get; set;}
        public DbSet<Comments> comments { get; set;}
        public DbSet<Messages> messages { get; set; }
    }
}
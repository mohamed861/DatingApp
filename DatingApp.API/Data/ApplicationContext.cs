using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data

{
    public class ApplicationContext: DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options):base(options){}

        public DbSet<Value> Values { get; set; }         
        public DbSet<User> Users { get; set; }         
    }
}
using Microsoft.EntityFrameworkCore;
using WebApi_Sample.Models.Entities;

namespace WebApi_Sample.Models.Contexts;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<ToDo> ToDos { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }
    public DbSet<SmsCode> SmsCodes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ToDo>().HasQueryFilter(p => !p.IsRemoved);
    }
}

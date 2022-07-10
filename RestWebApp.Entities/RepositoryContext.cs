using Microsoft.EntityFrameworkCore;
using RestWebApp.Entities.Models;

namespace RestWebApp.Entities;

public class RepositoryContext: DbContext
{
    public RepositoryContext(DbContextOptions<RepositoryContext> options)
        : base(options)
    {
    }
        
    public DbSet<Car> Cars { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData
        (
            new User {Id = 1, Name = "admin", Password = "AAAAAAAAA", Role = "Admin"}, 
            new User {Id = 2, Name = "test", Password = "test", Role = "User"}
        );

        modelBuilder.Entity<Car>().HasData(
            new Car { Id = 1, Brand = "BMW", Description = "Ssss", Date = "14-03-2001" },
            new Car { Id = 2, Brand = "Audi", Description = "Ssss", Date = "14-05-2001" }
        );
    }
}
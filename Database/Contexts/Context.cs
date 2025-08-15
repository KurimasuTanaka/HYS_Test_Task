using System;
using Microsoft.EntityFrameworkCore;

namespace Database;

public class Context : DbContext
{
    public DbSet<UserModel> Users { get; set; }
    public DbSet<MeetingModel> Meetings { get; set; }

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("InMemoryDb"); 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}

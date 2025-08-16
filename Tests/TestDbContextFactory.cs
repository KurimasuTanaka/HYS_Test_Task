using System;
using Database;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class TestDbContextFactory : IDbContextFactory<Context>
{
    DbContextOptionsBuilder<Context> builder = new DbContextOptionsBuilder<Context>();
    public TestDbContextFactory()
    {
        builder.UseInMemoryDatabase("TestDatabase");
    }

    public void DeleteTestDb()
    {
        Context context = new(builder.Options);
        context.Database.EnsureDeleted();
    }

    public Context CreateDbContext()
    {
        return new Context(builder.Options);
    }
}
using Microsoft.EntityFrameworkCore;
using Database;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataAccess;

public class UserRepository : IUserRepository
{
    private readonly IDbContextFactory<Context> _contextFactory;

    public UserRepository(IDbContextFactory<Context> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var entity = await context.Users.Include(u => u.Meetings).FindAsync(id);
            if (entity == null) return null;
            return new User(entity);
        }
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var entities = await context.Users.Include(u => u.Meetings).ToListAsync();
            return entities.Select(a => new User(a)).ToList();
        }
    }

    public async Task<User?> AddAsync(User user)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var entity = await context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
            if (entity == null) return null;
            return new User(entity);
        }
    }

    public async Task UpdateAsync(User user)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(User user)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }
} 
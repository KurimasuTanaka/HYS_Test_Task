using Microsoft.EntityFrameworkCore;
using Database;
using Microsoft.EntityFrameworkCore.Internal;

namespace DataAccess;

public class MeetingRepository : IMeetingRepository
{
    private readonly IDbContextFactory<Context> _contextFactory;

    public MeetingRepository(IDbContextFactory<Context> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<Meeting?> GetByIdAsync(long id)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var entity = await context.Meetings.Include(p => p.Participants).FirstOrDefaultAsync(m => m.Id == id);
            if (entity == null) return null;
            return new Meeting(entity);
        }
    }

    public async Task<IEnumerable<Meeting>> GetAllAsync()
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var entities = await context.Meetings.Include(p => p.Participants).ToListAsync();
            return entities.Select(a => new Meeting(a)).ToList();
        }
    }

    public async Task<Meeting?> AddAsync(Meeting meeting)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            var participants = new List<UserModel>();
            var participantIds = new HashSet<long>();

            foreach (var participant in meeting.Participants)
            {
                if (participantIds.Add(participant.Id))
                {
                    var p = context.Users.FirstOrDefault(u => u.Id == participant.Id);
                    if (p != null)
                    {
                        participants.Add(p);
                    }
                }
            }

            meeting.Participants = participants;

            await context.Meetings.AddAsync(meeting);
            await context.SaveChangesAsync();
            return null;
        }
    }
    public async Task UpdateAsync(Meeting meeting)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Meetings.Update(meeting);
            await context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Meeting meeting)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            context.Meetings.Remove(meeting);
            await context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Meeting>> GetByUserIdAsync(long userId)
    {
        using (var context = _contextFactory.CreateDbContext())
        {
            return await context.Meetings.Include(m => m.Participants)
                .Where(m => m.Participants.Any(user => user.Id == userId))
                .Select(m => new Meeting(m))
                .ToListAsync();
        }
    }
}
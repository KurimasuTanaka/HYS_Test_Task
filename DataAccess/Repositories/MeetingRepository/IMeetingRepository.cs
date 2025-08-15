
namespace DataAccess;

public interface IMeetingRepository : IRepository<Meeting>
{
    Task<IEnumerable<Meeting>> GetByUserIdAsync(long userId);
} 
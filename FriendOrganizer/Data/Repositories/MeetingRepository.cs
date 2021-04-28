using FriendOrganizer.Model;
using FriendOrganizerDataAccess;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public class MeetingRepository : GenericRepository<Meeting, FriendOrganizerDBContext>, IMeetingRepository
    {
        public MeetingRepository(FriendOrganizerDBContext context) : base(context)
        {
        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings.Include(m => m.Friends).SingleAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Friend>> GetAllFriendsAsync()
        {
            return await Context.Set<Friend>().ToListAsync();
        }

        public async Task ReloadFriendAsync(int id)
        {
            var dbEntity = Context.ChangeTracker.Entries<Friend>()
                 .SingleOrDefault(db => db.Entity.Id == id);
            if (dbEntity != null)
            {
                await dbEntity.ReloadAsync();
            }
        }
    }
}

using FriendOrganizer.Model;
using FriendOrganizerDataAccess;
using System.Data.Entity;
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
    }
}

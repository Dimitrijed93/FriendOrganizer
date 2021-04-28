using FriendOrganizer.Model;
using FriendOrganizerDataAccess;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public class FriendRepository : GenericRepository<Friend, FriendOrganizerDBContext>, IFriendRepository
    {

        public FriendRepository(FriendOrganizerDBContext context) : base (context)
        {
        }

        public override async Task<Friend> GetByIdAsync(int id)
        {
            return await Context.Friends.Include(f => f.PhoneNumbers).SingleAsync(f => f.Id == id);
        }

        public void RemovePhoneNumber(FriendPhoneNumber model)
        {
            Context.FriendPhoneNumbers.Remove(model);
        }
    }
}

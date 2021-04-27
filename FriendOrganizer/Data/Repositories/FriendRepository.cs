using FriendOrganizer.Model;
using FriendOrganizerDataAccess;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public class FriendRepository : IFriendRepository
    {
        private FriendOrganizerDBContext _context;

        public FriendRepository(FriendOrganizerDBContext context)
        {
            _context = context;
        }

        public async Task<List<Friend>> GetAllAsync()
        {
            return await _context.Friends.ToListAsync();
        }

        public async Task<Friend> GetByIdAsync(int id)
        {
            return await _context.Friends.SingleAsync(f => f.Id == id);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}

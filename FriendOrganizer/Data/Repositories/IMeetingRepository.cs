using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
        Task<IEnumerable<Friend>> GetAllFriendsAsync();

        Task ReloadFriendAsync(int id);
    }
}
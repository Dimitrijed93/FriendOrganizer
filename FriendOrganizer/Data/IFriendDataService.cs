using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.Data
{
    public interface IFriendDataService
    {
        Task<List<Friend>> GetAllAsync();

        Task<Friend> GetByIdAsync(int id);

    }
}
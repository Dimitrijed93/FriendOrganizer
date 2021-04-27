using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Repositories
{
    public interface IFriendRepository
    {
        Task<List<Friend>> GetAllAsync();

        Task<Friend> GetByIdAsync(int id);

        Task SaveAsync();

        bool HasChanges();

    }
}
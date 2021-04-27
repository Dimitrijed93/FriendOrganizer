using FriendOrganizer.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FriendOrganizer.Data.Lookups
{
    public interface ILookupDataService
    {
        Task<IEnumerable<LookupItem>> GetFriendLookupAsync();
    }
}
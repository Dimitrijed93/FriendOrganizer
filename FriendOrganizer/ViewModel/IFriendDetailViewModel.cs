using System.Threading.Tasks;

namespace FriendOrganizer.ViewModel
{
    public interface IFriendDetailViewModel
    {
        Task LoadAsync(int id);
    }
}
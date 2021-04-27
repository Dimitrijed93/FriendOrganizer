using System.Threading.Tasks;

namespace FriendOrganizer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public MainViewModel(INavigationViewModel navigationViewModel, IFriendDetailViewModel friendDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            FriendDetailViewModel = friendDetailViewModel;
        }

        public INavigationViewModel NavigationViewModel { get; private set; }
        public IFriendDetailViewModel FriendDetailViewModel { get; private set; }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


    }
}

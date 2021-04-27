using FriendOrganizer.Data.Lookups;
using FriendOrganizer.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private ILookupDataService _lookupDataService;
        private IEventAggregator _eventAggregator;
        private NavigationItemViewModel _selectedFriend;

        public ObservableCollection<NavigationItemViewModel> Friends { get; }

        public NavigationViewModel(ILookupDataService lookupDataService, IEventAggregator eventAggregator)
        {
            _lookupDataService = lookupDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe(AfterFriendSaved);
            _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Subscribe(AfterFriendDeleted);
            Friends = new ObservableCollection<NavigationItemViewModel>();
        }

        public async Task LoadAsync()
        {
            var lookup = await _lookupDataService.GetFriendLookupAsync();

            foreach (var friend in lookup)
            {
                Friends.Add(new NavigationItemViewModel (friend.Id, friend.DisplayMember , _eventAggregator));
            }
        }

        public NavigationItemViewModel SelectedFriend
        {
            get { return _selectedFriend; }
            set
            { 
                _selectedFriend = value;
                OnPropertyChanged(); 
            }
        }

        private void AfterFriendDeleted(int id)
        {
            var friend = Friends.SingleOrDefault(f => f.Id == id);
            if (friend != null)
            {
                Friends.Remove(friend);
            }
        }

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookupItem = Friends.SingleOrDefault(x => x.Id == obj.Id);
            if (lookupItem == null)
            {
                Friends.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }


    }

}

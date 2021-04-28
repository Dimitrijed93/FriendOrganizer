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
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }


        public NavigationViewModel(ILookupDataService lookupDataService, IEventAggregator eventAggregator)
        {
            _lookupDataService = lookupDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
            Friends = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();

        }

        public async Task LoadAsync()
        {
            await LoadMeetings();
            await LoadFriends();
        }

        private async Task LoadFriends()
        {
            var lookup = await _lookupDataService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var friend in lookup)
            {
                Friends.Add(new NavigationItemViewModel(friend.Id, friend.DisplayMember, _eventAggregator, nameof(FriendDetailViewModel)));
            }
        }

        private async Task LoadMeetings()
        {
            var lookup = await _lookupDataService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var meeting in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(meeting.Id, meeting.DisplayMember, _eventAggregator, nameof(MeetingDetailViewModel)));
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

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailDeleted(Friends, args);
                break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(f => f.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(FriendDetailViewModel):
                    AfterDetailSaved(Friends, obj);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailSaved(Meetings, obj);
                    break;
            }
        }

        private void AfterDetailSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(x => x.Id == args.Id);
            if (lookupItem == null)
            {
                items.Add(new NavigationItemViewModel(args.Id, args.DisplayMember, _eventAggregator, args.ViewModelName));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }
    }
}

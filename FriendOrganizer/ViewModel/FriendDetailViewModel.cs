using FriendOrganizer.Data;
using FriendOrganizer.Event;
using FriendOrganizer.Model;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendDataService _dataService;
        private IEventAggregator _eventAggregator;
        private Friend _friend;

        public FriendDetailViewModel(IFriendDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);
        }

        private async void OnOpenFriendDetailView(int id)
        {
            await LoadAsync(id);
        }

        public Friend Friend
        {
            get { return _friend; }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public async Task LoadAsync(int id)
        {
            Friend = await _dataService.GetByIdAsync(id);
        }
    }
}

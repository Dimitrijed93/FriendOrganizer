using FriendOrganizer.Event;
using FriendOrganizer.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        public INavigationViewModel NavigationViewModel { get; private set; }
        private IFriendDetailViewModel _friendDetailViewModel;

        public IFriendDetailViewModel FriendDetailViewModel 
        {
            get { return _friendDetailViewModel; }
            private  set { _friendDetailViewModel = value; OnPropertyChanged();  }
        }


        private Func<IFriendDetailViewModel> _friendDetailViewModelCreator;
        private IMessageDialogService _messageDialogService;
        private IEventAggregator _eventAggregator;

        public MainViewModel(INavigationViewModel navigationViewModel, 
            Func<IFriendDetailViewModel> friendDetailViewModelCreator, 
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _friendDetailViewModelCreator = friendDetailViewModelCreator;
            _messageDialogService = messageDialogService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenFriendDetailViewEvent>().Subscribe(OnOpenFriendDetailView);
            NavigationViewModel = navigationViewModel;

        }

        private async void OnOpenFriendDetailView(int id)
        {
            if (FriendDetailViewModel != null && FriendDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.CANCEL)
                {
                    return;
                }
            }
            FriendDetailViewModel = _friendDetailViewModelCreator();
            await FriendDetailViewModel.LoadAsync(id);
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }


    }
}

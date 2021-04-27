using FriendOrganizer.Data.Lookups;
using FriendOrganizer.Data.Repositories;
using FriendOrganizer.Event;
using FriendOrganizer.Model;
using FriendOrganizer.View.Services;
using FriendOrganizer.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private IFriendRepository _dataService;
        private IEventAggregator _eventAggregator;
        private ILookupDataService _lookupDataService;
        private IMessageDialogService _messageDialogService;
        private FriendWrapper _friend;
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        private bool _hasChanges;

        public FriendDetailViewModel(IFriendRepository dataService,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            ILookupDataService lookupDataService)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _lookupDataService = lookupDataService;
            _messageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            ProgrammingLanguages = new ObservableCollection<LookupItem>();
        }

        public bool HasChanges
        {
            get { return _hasChanges; }
            set 
            { 
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public FriendWrapper Friend
        {
            get { return _friend; }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }


        public async Task LoadAsync(int? id)
        {
            var friend = id.HasValue ? await _dataService.GetByIdAsync(id.Value)
                : CreateNewFriend();
            InitializeFriend(friend);

            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _dataService.HasChanges();
                }
                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            if (Friend.Id == 0)
            {
                Friend.FirstName = string.Empty;
            }
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem());
            var lookup = await _lookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var item in lookup)
            {
                ProgrammingLanguages.Add(item);
            }
        }

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete friend {Friend.FirstName} {Friend.LastName}", "Question");
            if (result == MessageDialogResult.OK)
            {
                _dataService.Remove(Friend.Model);
                await _dataService.SaveAsync();
                _eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private async void OnSaveExecute()
        {
            await _dataService.SaveAsync();
            HasChanges = _dataService.HasChanges();
            _eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish(new AfterFriendSavedEventArgs { Id = Friend.Id, DisplayMember = $"{Friend.FirstName} {Friend.LastName}" });
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }


        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            _dataService.Add(friend);
            return friend;
        }
    }
}

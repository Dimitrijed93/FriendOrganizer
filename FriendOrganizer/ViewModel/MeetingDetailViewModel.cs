using FriendOrganizer.Data.Repositories;
using FriendOrganizer.Event;
using FriendOrganizer.Model;
using FriendOrganizer.View.Services;
using FriendOrganizer.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        public ObservableCollection<Friend> AddedFriends { get; }
        public ObservableCollection<Friend> AvailabileFriends { get; }
        public DelegateCommand AddFriendCommand { get; }
        public DelegateCommand RemoveFriendCommand { get; }

        private MeetingWrapper _meeting;
        private Friend _selectedAvailableFriend;
        private Friend _selectedAddedFriend;
        private IEnumerable<Friend> _allFriends;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base (eventAggregator, messageDialogService)
        {
            this._meetingRepository = meetingRepository;

            AddedFriends = new ObservableCollection<Friend>();
            AvailabileFriends = new ObservableCollection<Friend>();

            AddFriendCommand = new DelegateCommand(OnAddFriendExecute, OnAddFriendCanExecute);
            RemoveFriendCommand = new DelegateCommand(OnRemoveFriendExecute, OnRemoveFriendCanExecute);

            EventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            EventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set { _meeting = value; OnPropertyChanged(); }
        }

        public Friend SelectedAvailableFriend 
        { 
            get
            {
                return _selectedAvailableFriend;
            }
            set
            {
                _selectedAvailableFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)AddFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public Friend SelectedAddedFriend
        {
            get
            {
                return _selectedAddedFriend;
            }
            set
            {
                _selectedAddedFriend = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
            }
        }

        public async override Task LoadAsync(int id)
        {
            var meeting = id > 0 ?
                await _meetingRepository.GetByIdAsync(id)
                : CreateNewMeeting();
            Id = meeting.Id;
            InitializeMeeting(meeting);

            _allFriends = await _meetingRepository.GetAllFriendsAsync();

            SetupPicklist();
        }

        private async void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                _allFriends = await _meetingRepository.GetAllFriendsAsync();
                SetupPicklist();
            }
        }

        private async void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(FriendDetailViewModel))
            {
                await _meetingRepository.ReloadFriendAsync(args.Id);
                _allFriends = await _meetingRepository.GetAllFriendsAsync();
                SetupPicklist();
            }
        }

        private void OnRemoveFriendExecute()
        {
            var friend = SelectedAddedFriend;

            Meeting.Model.Friends.Remove(friend);
            AddedFriends.Remove(friend);
            AvailabileFriends.Add(friend);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)RemoveFriendCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemoveFriendCanExecute()
        {
            return SelectedAddedFriend != null;
        }

        private void OnAddFriendExecute()
        {
            var friend = SelectedAvailableFriend;

            Meeting.Model.Friends.Add(friend);
            AddedFriends.Add(friend);
            AvailabileFriends.Remove(friend);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddFriendCanExecute()
        {
            return SelectedAvailableFriend != null;
        }


        private void SetupPicklist()
        {
            var meetingFriendIds = Meeting.Model.Friends.Select(f => f.Id).ToList();
            var addedFriends = _allFriends.Where(f => meetingFriendIds.Contains(f.Id)).OrderBy(f => f.FirstName);
            var availableFriends = _allFriends.Except(addedFriends).OrderBy(f => f.FirstName);

            AddedFriends.Clear();
            AvailabileFriends.Clear();

            foreach (var addedFriend in addedFriends)
            {
                AddedFriends.Add(addedFriend);
            }

            foreach (var availableFriend in availableFriends)
            {
                AvailabileFriends.Add(availableFriend);
            }
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date,
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }

        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
                if (e.PropertyName == nameof(Meeting.Title))
                {
                    SetTitle();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0)
            {
                Meeting.Title = String.Empty;
            }
            SetTitle();

        }

        private void SetTitle()
        {
            Title = Meeting.Title;
        }

        protected override void OnDeleteExecute()
        {
            var result = MessageDialogService.ShowOkCancelDialog($"Do you really want to delete meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _meetingRepository.Remove(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected async override void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            Id = Meeting.Id;
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}

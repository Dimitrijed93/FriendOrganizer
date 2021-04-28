using FriendOrganizer.Data.Repositories;
using FriendOrganizer.Model;
using FriendOrganizer.View.Services;
using FriendOrganizer.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace FriendOrganizer.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageDialogService _messageDialogeService;
        private MeetingWrapper _meeting;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base (eventAggregator)
        {
            this._meetingRepository = meetingRepository;
            this._messageDialogeService = messageDialogService;
        }


        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set { _meeting = value; OnPropertyChanged(); }
        }

        public async override Task LoadAsync(int? id)
        {
            var meeting = id.HasValue ?
                await _meetingRepository.GetByIdAsync(id.Value)
                : CreateNewMeeting();
            InitializeMeeting(meeting);
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
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0)
            {
                Meeting.Title = String.Empty;
            }

        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogeService.ShowOkCancelDialog($"Do you really want to delete meeting {Meeting.Title}?", "Question");
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
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }
    }
}

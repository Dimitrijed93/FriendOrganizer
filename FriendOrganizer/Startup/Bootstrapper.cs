using Autofac;
using FriendOrganizer.Data.Lookups;
using FriendOrganizer.Data.Repositories;
using FriendOrganizer.View.Services;
using FriendOrganizer.ViewModel;
using FriendOrganizerDataAccess;
using Prism.Events;

namespace FriendOrganizer.Startup
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<EventAggregator>().As<IEventAggregator>().SingleInstance();
            builder.RegisterType<FriendOrganizerDBContext>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<MainViewModel>().AsSelf();
            builder.RegisterType<FriendRepository>().As<IFriendRepository>();
            builder.RegisterType<FriendDetailViewModel>().Keyed<IDetailViewModel>(nameof(FriendDetailViewModel));
            builder.RegisterType<MeetingDetailViewModel>().Keyed<IDetailViewModel>(nameof(MeetingDetailViewModel));
            builder.RegisterType<LookupDataService>().As<ILookupDataService>();
            builder.RegisterType<MeetingRepository>().As<IMeetingRepository>();
            builder.RegisterType<MessageDialogService>().As<IMessageDialogService>();
            builder.RegisterType<NavigationViewModel>().As<INavigationViewModel>();
            return builder.Build();
        }

    }
}

using System.Windows;

namespace FriendOrganizer.View.Services
{
    public class MessageDialogService : IMessageDialogService
    {
        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK ? MessageDialogResult.OK : MessageDialogResult.CANCEL;
        }
    }

    public enum MessageDialogResult
    {
        OK,
        CANCEL
    }
}

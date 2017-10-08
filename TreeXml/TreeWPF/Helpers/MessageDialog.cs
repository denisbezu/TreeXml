using TreeWPF.ViewModels;

namespace TreeWPF.Helpers
{
    public class MessageDialog
    {
        public static void ShowMessage(MessageDialogViewModel messageDialogViewModel)
        {
            Views.MessageDialog messageDialog = new Views.MessageDialog(messageDialogViewModel);
            messageDialog.ShowDialog();
        }

        public static void ShowMessage(string message)
        {
            Views.MessageDialog messageDialog = new Views.MessageDialog(new MessageDialogViewModel(message));
            messageDialog.ShowDialog();
        }

        private MessageDialogViewModel CreateMessageViewModel(string message = null)
        {
            MessageDialogViewModel messageViewModel = new MessageDialogViewModel(message);
            return messageViewModel;
        }
    }
}
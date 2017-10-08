using System.Windows;
using System.Windows.Controls;
using TreeWPF.Helpers;
using TreeWPF.Interfaces;

namespace TreeWPF.Views
{
    /// <summary>
    /// Interaction logic for ConnectToServerView.xaml
    /// </summary>
    public partial class ConnectToServerView : Window
    {
        public ConnectToServerView(IClosableViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.Close += (sender, args) => Close();
        }
        
        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pBox = sender as PasswordBox;
            PasswordHelper.SetEncryptedPassword(pBox, pBox.SecurePassword);
        }

    }
}

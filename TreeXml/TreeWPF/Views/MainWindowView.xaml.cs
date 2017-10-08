using System.Windows;
using System.Windows.Controls;
using TreeWPF.Helpers;
using TreeWPF.ViewModels;

namespace TreeWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView_.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {

        public MainWindowView()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            ItemToContextMenuConverter.Folder
                = this.Resources["Folder"] as ContextMenu;
            ItemToContextMenuConverter.NotFolder
                = this.Resources["NotFolder"] as ContextMenu;
        }
    }
}

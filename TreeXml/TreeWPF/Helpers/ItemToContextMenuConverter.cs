using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using DatabaseLibrary.Enums;
using TreeWPF.ViewModels;
using GroupItem = DatabaseLibrary.Enums.GroupItem;

namespace TreeWPF.Helpers
{
    public class ItemToContextMenuConverter : IValueConverter
    {
        public static ContextMenu Folder;
        public static ContextMenu NotFolder;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            NodeViewModel node = value as NodeViewModel;
            if (node == null)
                return null;
            return GroupItem.IsGroupItem(node.Type) || node.Type == SingleItem.Server
                || node.Type == SingleItem.Column || node.Type == SingleItem.Parameter ? Folder : NotFolder;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
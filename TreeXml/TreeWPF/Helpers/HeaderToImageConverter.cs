using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using DatabaseLibrary.Enums;
using TreeXmlLibrary;

namespace TreeWPF.Helpers
{
    [ValueConversion(typeof(string), typeof(BitmapImage))]
    public class HeaderToImageConverter : IValueConverter
    {
        public static HeaderToImageConverter Instance = new HeaderToImageConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var image = "Images/folder.png";

            switch ((string)value)
            {
                case SingleItem.Schema:
                    image = "Images/schema.png";
                    break;
                case SingleItem.Database:
                    image = "Images/database.png";
                    break;
                case SingleItem.CheckConstraint:
                    image = "Images/check.png";
                    break;
                case SingleItem.Column:
                    image = "Images/column.png";
                    break;
                case SingleItem.DefaultConstraint:
                    image = "Images/default.png";
                    break;
                case SingleItem.ForeignKey:
                    image = "Images/foreignkey.png";
                    break;
                case SingleItem.Function:
                    image = "Images/function.png";
                    break;
                case SingleItem.Index:
                    image = "Images/index.png";
                    break;
                case SingleItem.Parameter:
                    image = "Images/parameter.png";
                    break;
                case SingleItem.PrimaryKey:
                    image = "Images/primarykey.png";
                    break;
                case SingleItem.Procedure:
                    image = "Images/procedure.png";
                    break;
                case SingleItem.Table:
                    image = "Images/table.png";
                    break;
                case SingleItem.Trigger:
                    image = "Images/trigger.png";
                    break;
                case SingleItem.UserType:
                    image = "Images/type.png";
                    break;
                case SingleItem.View:
                    image = "Images/view.png";
                    break;
                case SingleItem.Server:
                    image = "Images/server.png";
                    break;
                default:
                    image = "Images/folder.png";
                    break;
            }

            return new BitmapImage(new Uri($"pack://application:,,,/{image}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
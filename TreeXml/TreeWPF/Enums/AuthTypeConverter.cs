using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace TreeWPF.Enums
{
    public class AuthTypeConverter : MarkupExtension, IValueConverter
    {
        private static AuthTypeConverter converter;

        public AuthTypeConverter()
        {
            
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AuthType)
            {
                switch ((AuthType)value)
                {
                    case AuthType.Windows:
                        return "Windows Authentication";
                    case AuthType.SqlServer:
                        return "Sql Server Authentication";
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return converter ?? (converter = new AuthTypeConverter());
        }
    }
}
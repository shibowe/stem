using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MicrobitCPA.MicrobitUtils.Helpers
{
    public class ButtonStateToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int num;
            if (!int.TryParse(value.ToString(), out num))
            {
                return "[Not Available]";
            }

            switch (num)
            {
                case -1:
                case 0:
                    return "Not Pressed";
                case 1:
                    return "Pressed";
                case 2:
                    return "Long Press";
                default:
                    return "[Not Available]";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

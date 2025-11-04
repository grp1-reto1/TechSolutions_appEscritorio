using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Data;

namespace Grupo_1_Interfaces.Utils
{
    //para poner el background en el 'Datagrid' del stock
    public class StockColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.Transparent;

            float stock = System.Convert.ToSingle(value);

            //if (stock < 0) return Brushes.DarkRed; //Si es menor de 0
            if (stock <= 5) return Brushes.Red; //si el stock es menor o igual a cinco
            if (stock <= 20) return Brushes.Orange; //si el stock es emnos o igual a veinte
            if (stock <= 50) return Brushes.Yellow; //si es menor o igual a cincuenta
            return Brushes.LightGreen; //los demás stocks 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

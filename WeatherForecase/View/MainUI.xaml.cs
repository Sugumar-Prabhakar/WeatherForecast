using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeatherForecast.Viewmodel;

namespace WeatherForecast.View
{
    /// <summary>
    /// Interaction logic for MainUI.xaml
    /// </summary>
    public partial class MainUI : UserControl
    {
        MainUIViewModel viewmodel;
        public MainUI()
        {
            InitializeComponent();
            viewmodel = new MainUIViewModel();
            DataContext = viewmodel;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            viewmodel.InitializeView();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            viewmodel.UnInitializeView();
        }
    }

    public class VisiblityFlipConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null)
            {
                return (bool)value == true ? Visibility.Collapsed : Visibility.Visible;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
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
using FirstFloor.ModernUI.Windows.Controls;

namespace DbDictExport.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new ModernDialog
            {
                //Style = (Style)Application.Current.Resources["EmptyWindow"],
                Content = new Connect
                {
                    Margin = new Thickness(32)
                },
                Width = 400,
                Height= 260,
                MinWidth = 400,
                MinHeight = 260,
                MaxHeight = 260,
                MaxWidth = 400,
                WindowStartupLocation  = WindowStartupLocation.CenterScreen
            };

            window.ShowDialog();
        }
    }
}

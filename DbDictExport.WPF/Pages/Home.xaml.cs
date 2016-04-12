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
using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;

namespace DbDictExport.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home
    {
        public Home()
        {
            InitializeComponent();
            Loaded += Home_Loaded;

        }

        private void Home_Loaded(object sender, RoutedEventArgs e)
        {
            if (Global.ConnectionBuilder == null)
            {
                var routedCommand = NavigationCommands.GoToPage;
                IInputElement target = NavigationHelper.FindFrame("_top", this);
                routedCommand.Execute("/Pages/Connect.xaml", target);
            }
        }

    }
}

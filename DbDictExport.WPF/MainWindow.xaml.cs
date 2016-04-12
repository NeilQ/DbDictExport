using System.Windows;
using FirstFloor.ModernUI.Windows.Controls;


namespace DbDictExport.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.MainWindow = this;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.ComponentModel;
using FirstFloor.ModernUI.Presentation;


namespace DbDictExport.WPF
{
    public sealed class AppManagement : NotifyPropertyChanged
    {
        public MainWindow AppMainWindow { get; set; }

        public static AppManagement Current = new AppManagement();
       
    }
}

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
using System.Xml;
using System.Configuration;
using System.Windows.Forms;

namespace Production_control_1._0
{
    public partial class MainWindow : CustomChromeLibrary.CustomChromeWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            main.Content = new PagePrincipal();
            NavigationCommands.BrowseBack.InputGestures.Clear();
            NavigationCommands.BrowseForward.InputGestures.Clear();
        }
    }
}

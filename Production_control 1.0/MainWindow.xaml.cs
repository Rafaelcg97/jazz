using System.Windows.Input;

namespace JazzCCO._0
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

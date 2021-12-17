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

namespace JazzCCO._0.pantallasKanban
{
    public partial class cuadraturaEntregaMateriales : Page
    {
        public cuadraturaEntregaMateriales()
        {
            InitializeComponent();
        }

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

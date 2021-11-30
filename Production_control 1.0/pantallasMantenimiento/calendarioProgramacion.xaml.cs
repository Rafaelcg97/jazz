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

namespace JazzCCO._0.pantallasMantenimiento
{
    public partial class calendarioProgramacion : Page
    {
        public calendarioProgramacion()
        {
            InitializeComponent();
        }

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void btnAgregarActividad_Click(object sender, RoutedEventArgs e)
        {
            if (calendarFecha.SelectedDate.HasValue)
            {
                dtpFecha.SelectedDate = calendarFecha.SelectedDate;
                cmbMaquina.SelectedIndex = -1;
                cmbActividad.SelectedIndex = -1;
            }
            popUpAgregarActividad.IsOpen = true;
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            popUpAgregarActividad.IsOpen = false;
        }
    }
}

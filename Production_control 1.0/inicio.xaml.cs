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

namespace Production_control_1._0
{
    /// <summary>
    /// Interaction logic for inicio.xaml
    /// </summary>
    public partial class inicio : Page
    {
        public inicio()
        {
            InitializeComponent();
        }

        private void navegar_sam_Click(object sender, RoutedEventArgs e)
        {
            sam sam = new sam();
            this.NavigationService.Navigate(sam);
        }

        private void navegar_configuraciones_Click(object sender, RoutedEventArgs e)
        {
            configuraciones configuraciones = new configuraciones();
            this.NavigationService.Navigate(configuraciones);
        }

        private void abrir_balances_Click(object sender, RoutedEventArgs e)
        {
            abrir abrir = new abrir();
            this.NavigationService.Navigate(abrir);
        }

        private void estado_plant_Click(object sender, RoutedEventArgs e)
        {
            planta planta = new planta();
            this.NavigationService.Navigate(planta);
        }

        private void reporte_maquina_Click(object sender, RoutedEventArgs e)
        {
            solicitudes solicitudes= new solicitudes();
            this.NavigationService.Navigate(solicitudes);
        }

        private void mantenimiento_preventiv_Click(object sender, RoutedEventArgs e)
        {
            preventivo preventivo = new preventivo();
            this.NavigationService.Navigate(preventivo);
        }
    }



}


using Production_control_1._0.clases;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Production_control_1._0.pantallasBMP
{
    public partial class menuBMP : Page
    {
        public menuBMP()
        {
            InitializeComponent();
        }

        private void btnInicio_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PagePrincipal());
        }

        private void btnPartNumbers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new partNumbers());
        }

        private void btnAccesorios_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new programacionTrims());
        }

        private void btnTiemposTrims_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new controlTiemposTrims());
        }

        private void btnPreparacionCajas_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new preparacionCajasParciales());
        }
    }
}

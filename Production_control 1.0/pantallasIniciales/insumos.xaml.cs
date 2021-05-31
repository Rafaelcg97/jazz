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

namespace Production_control_1._0.pantallasIniciales
{
    public partial class insumos : UserControl
    {
        #region datosIniciales
        public insumos()
        {
            InitializeComponent();
        }
        #endregion
        #region calculos_generals
        private DependencyObject GetDependencyObjectFromVisualTree(DependencyObject startObject, Type type)
        {
            //dependencia hacia la pagina
            DependencyObject parent = startObject;
            while (parent != null)
            {
                if (type.IsInstanceOfType(parent))
                    break;
                else
                    parent = VisualTreeHelper.GetParent(parent);
            }
            return parent;
        }
        #endregion
        #region entrar
        private void Border_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid contenedor = (Grid)((Border)sender).Child;
            string areaSeleccionada = "";
            foreach(object objeto in contenedor.Children)
            {
                if (objeto.GetType() == typeof(Label))
                {
                    areaSeleccionada = ((Label)objeto).Content.ToString();
                }
            }

            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasInsumos.validacionUsuario(areaSeleccionada));
        }

        private void IngresarEstadoDeSolicitudes_Click(object sender, RoutedEventArgs e)
        {
            string areaSeleccionada = "Administración Bodega de Insumos";

            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasInsumos.validacionUsuario(areaSeleccionada));
        }
        #endregion
    }
}

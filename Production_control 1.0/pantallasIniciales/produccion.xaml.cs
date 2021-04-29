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
    public partial class produccion : UserControl
    {
        #region datos_iniciales
        public produccion()
        {
            InitializeComponent();
        }

        #endregion

        #region entrar_a_pantallas

        private void BorderConsultaSam_MouseUp(object sender, MouseButtonEventArgs e)
        {
            estadoPlantaProduccion estadoPlantaProduccion = new estadoPlantaProduccion();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new sam();
        }

        private void BorderAbrirBalance_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new abrir();
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

        private void BorderReporteProduccion_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new pantallasProduccion.reporteProduccion();
        }

        private void BorderEditarProduccion_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasProduccion.validacionUsuarioProduccion());
        }

        private void BorderBonos_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new pantallasProduccion.bonos();
        }
    }
}

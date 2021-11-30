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
using JazzCCO._0.pantallasKanban;

namespace JazzCCO._0.pantallasIniciales
{
    public partial class kanban : UserControl
    {
        public kanban()
        {
            InitializeComponent();
        }
        #region controlGeneral
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
        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Border borde = (Border)sender;
            borde.Opacity = 0.7;
        }
        private void border_MouseLeave(object sender, MouseEventArgs e)
        {
            Border borde = (Border)sender;
            borde.Opacity = 1;
        }
        #endregion
        private void borderKanban_MouseUp(object sender, MouseButtonEventArgs e)
        {
            estadoPlanta estadoPlanta = new estadoPlanta();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = estadoPlanta;
        }

        private void borderSolicitud_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new solicitudMateriales());
        }

        private void borderLote_MouseUp(object sender, MouseButtonEventArgs e)
        {
            estadoLotes estadoLotes = new estadoLotes();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = estadoLotes;
        }
    }
}

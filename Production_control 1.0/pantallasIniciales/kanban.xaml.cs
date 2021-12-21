using ClasesTexops;
using JazzCCO._0.pantallasKanban;
using LiveCharts;
using SQLConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace JazzCCO._0.pantallasIniciales
{
    public partial class kanban : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public kanban()
        {
            InitializeComponent();
            List<SolicitudKanban> wipTotal = new List<SolicitudKanban>();
            List<string> listaModulos = new List<string>();
            List<double> listaAccesorios = new List<double>();
            List<double> listaHilos= new List<double>();
            List<double> listaBinding = new List<double>();


            using (SqlConnection cn = ConexionTexopsServer.Kanban())
            {
                try
                {
                    string sql = "SELECT modulo, material, SUM(piezas) AS WIP FROM wipenkanban  GROUP BY modulo, material";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        wipTotal.Add(new SolicitudKanban { NombreModulo = dr["modulo"].ToString(), CategoryPartNumber = dr["material"].ToString(), RequeridoPartnumber = Convert.ToDouble(dr["wip"]) });
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            listaModulos = wipTotal.Select(x => x.NombreModulo).Distinct().ToList();

            Labels = listaModulos.ToArray();
            grafMaterialesDisponibles.AxisX.Add(new Axis() { Labels = modulos.ToArray(), LabelsRotation = 45, ShowLabels = true, Separator = { Step = 1 } });
            foreach(string item in listaModulos)
            {
                bool encontradoAccesorios = false;
                bool encontradoHilos = false;
                bool encontradoBinding = false;
                foreach (SolicitudKanban subitem in wipTotal)
                {
                    if(item==subitem.NombreModulo && subitem.CategoryPartNumber == "Accesorios")
                    {
                        listaAccesorios.Add(subitem.RequeridoPartnumber);
                        encontradoAccesorios = true;
                    }
                    if (item == subitem.NombreModulo && subitem.CategoryPartNumber == "Hilos")
                    {
                        listaHilos.Add(subitem.RequeridoPartnumber);
                        encontradoHilos = true;
                    }
                    if (item == subitem.NombreModulo && subitem.CategoryPartNumber == "Binding")
                    {
                        listaBinding.Add(subitem.RequeridoPartnumber);
                        encontradoBinding = true;
                    }
                }

                if (encontradoAccesorios == false)
                {
                    listaAccesorios.Add(0);
                }
                if (encontradoHilos == false)
                {
                    listaHilos.Add(0);
                }
                if (encontradoBinding == false)
                {
                    listaBinding.Add(0);
                }
            }






            StackedColumsGrafica nuevaGrafica = new StackedColumsGrafica();
            SeriesCollection = nuevaGrafica.StackedChart("Accesorios","Hilos","Binding",listaAccesorios.ToArray(),listaHilos.ToArray(),listaBinding.ToArray());
            DataContext = this;
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

        private void borderEntradas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasKanban.validacionUsuarioKanban());
        }

        private void borderCuadrarLotes_MouseUp(object sender, MouseButtonEventArgs e)
        {
            cuadraturaEntregaMateriales cuadraturaEntregaMateriales = new cuadraturaEntregaMateriales();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = cuadraturaEntregaMateriales;
        }
    }
}

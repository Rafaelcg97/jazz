using Production_control_1._0.clases;
using Production_control_1._0.kanban.NotificacionesDeTablaSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace Production_control_1._0.kanban
{
    public partial class estadoPlanta : Page
    {
        #region datosIniciales
        public estadoPlanta()
        {
            InitializeComponent();
            //se revisa cual es la distribucion de los modulos
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select id, modulo from orden_modulos";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                foreach (object objeto in areaDeTrabajo.Children)
                {
                    if (objeto.GetType() == typeof(Label))
                    {
                        if (((Label)objeto).Name == "l" + dr["id"].ToString())
                        {
                            ((Label)objeto).Content = dr["modulo"].ToString();
                        }
                    }
                }
            };
            dr.Close();
            cn.Close();
            this.CreatePermission();
            MessageModelPlanta model = new MessageModelPlanta(this.Dispatcher);
            this.DataContext = model;
        }
        public void CreatePermission()
        {
            // Make sure client has permissions 
            try
            {
                SqlClientPermission perm = new SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted);
                perm.Demand();
            }
            catch
            {
                throw new ApplicationException("No permission");
            }
        }
        #endregion
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            // navegarInicioKanBan
            PagePrincipal pagePrincipal = new PagePrincipal();
            Grid gridInicio = (Grid)pagePrincipal.Content;
            foreach (object objeto in gridInicio.Children)
            {
                if (objeto.GetType() == typeof(Grid))
                {
                    Grid grid = (Grid)objeto;
                    if (grid.Name == "gridListaAreas")
                    {
                        foreach (object objeto2 in grid.Children)
                        {
                            if (objeto2.GetType() == typeof(ListView))
                            {
                                ListView listviewMenu = (ListView)objeto2;
                                listviewMenu.SelectedIndex = 5;
                            }
                        }
                    }
                }
            }
            NavigationService.Navigate(pagePrincipal);
        }
        private void ButtonSalir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void ButtonMaximizar(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState == WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            };

        }
        private void ButtonMinimizar(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }
        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }
        #endregion
        #region tamanoLetra/solonumerosenTextBox
        private void letraAjustable1(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }
        private void letraAjustable2(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.6 / tmp.FontFamily.LineSpacing;
        }
        private void letraAjustable3(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.4 / tmp.FontFamily.LineSpacing;
        }

        #endregion
        #region popUpEstado
        private void mostrarPopUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBoxSeleccionado = (ListBox)sender;
            Uri iniciar_habilitado = new Uri("/imagenes/iniciar.png", UriKind.RelativeOrAbsolute);
            Uri terminar_habilitado = new Uri("/imagenes/terminar.png", UriKind.RelativeOrAbsolute);
            Uri iniciar_inhabilitado = new Uri("/imagenes/iniciar_in.png", UriKind.RelativeOrAbsolute);
            Uri terminar_inhabilitado = new Uri("/imagenes/terminar_in.png", UriKind.RelativeOrAbsolute);


            if (listBoxSeleccionado.SelectedIndex > -1)
            {
                solicitudKanban itemseleccionado = (solicitudKanban)listBoxSeleccionado.SelectedItem;
                popUpEstadoModulo.IsOpen = true;
                labelNumeroAccion.Content = itemseleccionado.solicitudKanbanId;
                labelMaterial.Content = itemseleccionado.material;
                labelModuloAccion.Content = itemseleccionado.modulo;
                labelTipoDeAccion.Content = itemseleccionado.tipo;
                labelHoraDeReporte.Content = Convert.ToDateTime(itemseleccionado.fechaSolicitud).ToString("yyyy-MM-dd HH:mm:ss");
                if (itemseleccionado.fechaInicio == "1900-01-01 12:00:00")
                {
                    labelHoraDeApertura.Content = "----";
                    labelEstadoDeAccion.Content = "Pendiente";
                    buttonTerminarAccion.IsEnabled = false;
                    buttonIniciarAccion.IsEnabled = true;
                    imageIniciar.Source =new BitmapImage(iniciar_habilitado);
                    imageTerminar.Source = new BitmapImage(terminar_inhabilitado);
                }
                else
                {
                    labelHoraDeApertura.Content = Convert.ToDateTime(itemseleccionado.fechaInicio).ToString("yyyy-MM-dd HH:mm:ss");
                    labelEstadoDeAccion.Content = "Abierta";
                    buttonIniciarAccion.IsEnabled = false;
                    buttonTerminarAccion.IsEnabled = true;
                    imageIniciar.Source = new BitmapImage(iniciar_inhabilitado);
                    imageTerminar.Source = new BitmapImage(terminar_habilitado);
                }
            }
        }
        private void ListBox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox list = (ListBox)sender;
            list.SelectedIndex = -1;
        }
        private void buttonCerrarPopUpEstadoModulo_Click(object sender, RoutedEventArgs e)
        {
            popUpEstadoModulo.IsOpen = false;
        }
        private void buttonIniciarAccion_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "update solicitudesKanban set fechaInicio='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where id='" + labelNumeroAccion.Content+"'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
            popUpEstadoModulo.IsOpen = false;
        }
        private void buttonTerminarAccion_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "update solicitudesKanban set fechaEntrega='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where id='" + labelNumeroAccion.Content + "'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
            popUpEstadoModulo.IsOpen = false;
        }
        #endregion
    }
}

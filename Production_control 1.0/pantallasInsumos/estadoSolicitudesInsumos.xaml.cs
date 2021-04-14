using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Production_control_1._0.clases;
using Production_control_1._0.pantallasInsumos.NotificacionesDeTablaSQL;

namespace Production_control_1._0.pantallasInsumos
{
    public partial class estadoSolicitudesInsumos : Page
    {

        #region conexionesConBasesSQL
        public SqlConnection cnMantenimiento = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public string sql; //Consulta que se hace en sql
        public SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
        public SqlDataReader dr; //leer los resultados del comando sql
        #endregion

        #region datosIniciales

        public estadoSolicitudesInsumos()
        {
            InitializeComponent();
            this.CreatePermission();

            MessageModel model = new MessageModel(this.Dispatcher);
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
        private void regresar(object sender, RoutedEventArgs e)
        {
            PagePrincipal PagePrincipal = new PagePrincipal();
            this.NavigationService.Navigate(PagePrincipal);
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

        #region tamanos_de_letra_/_tipo_de_texto

        private void tamanoLetrAutomatico(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }

        private void soloNumeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion

        #region ListBoxEmisorDeData

        #region ObtenerDatosDeListBox
        private static object GetDataFromListBox(ListBox source, Point point)
        {
            System.Windows.UIElement element = source.InputHitTest(point) as System.Windows.UIElement;
            if (element != null)
            {
                object data = DependencyProperty.UnsetValue;
                while (data == DependencyProperty.UnsetValue)
                {
                    data = source.ItemContainerGenerator.ItemFromContainer(element);

                    if (data == DependencyProperty.UnsetValue)
                    {
                        element = VisualTreeHelper.GetParent(element) as System.Windows.UIElement;
                    }

                    if (element == source)
                    {
                        return null;
                    }
                }

                if (data != DependencyProperty.UnsetValue)
                {
                    return data;
                }
            }

            return null;
        }
        #endregion
        ListBox dragSource = null;
        private void obtenerDatosListBox(object sender, MouseButtonEventArgs e)
        {
            ListBox parent = (ListBox)sender;
            dragSource = parent;
            object data = GetDataFromListBox(dragSource, e.GetPosition(parent));
            if (data != null)
            {
                DragDrop.DoDragDrop(parent, data, DragDropEffects.Move);
            }
        }
        #endregion

        #region listBoxReceptorDeDatos
        private void receptor(object sender, DragEventArgs e)
        {
            ListBox estacion = (ListBox)sender;
            string status = estacion.Name.ToString();
            object informacion = e.Data.GetData(typeof(solicitudInsumo));
            solicitudInsumo informacionElemento = informacion as solicitudInsumo;
            sql = "update ordenesBodegaInsumos set ordenStatus='"+status+"' where orden_id="+informacionElemento.ordenIdNum;
            cnMantenimiento.Open();
            cm = new SqlCommand(sql, cnMantenimiento);
            cm.ExecuteNonQuery();
            cnMantenimiento.Close();
        }
        #endregion

        #region abrirPopDetallesInsum
        private void Recibida_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox estadoSolicitud = ((ListBox)sender);
            if (estadoSolicitud.SelectedIndex > -1)
            {
               solicitudInsumo itemSeleccionado= (solicitudInsumo)estadoSolicitud.SelectedItem;
                //se abre la ventana emergente
                int numeroSolici = itemSeleccionado.ordenIdNum;
                List<solicitudInsumo> listaResumenSolicitudes = new List<solicitudInsumo>();
                cnMantenimiento.Open();
                sql = "select*from ordenesBodegaInsumosDetalles where ordenId="+numeroSolici;
                cm = new SqlCommand(sql, cnMantenimiento);
                dr = cm.ExecuteReader();
                //agregar solicitudes recibidas aprobadas entregadas a las listas
                while (dr.Read())
                {
                    listaResumenSolicitudes.Add(new solicitudInsumo() { description = dr["insumo"].ToString(), solicitado = Convert.ToInt32(dr["cantidad"]), costC = Convert.ToDouble(dr["total"]).ToString("C"), comentario=dr["comentario"].ToString() });    
                };
                dr.Close();
                cnMantenimiento.Close();
                listViewInsumosOrden.ItemsSource = listaResumenSolicitudes;

                detalles.IsOpen = true;
            }
        }

        #endregion

        #region botonesDescargadoCancelado

        private void buttonDescargas_Click(object sender, RoutedEventArgs e)
        {
            List<solicitudInsumo> listaSolicitudesDescargadas = new List<solicitudInsumo>();
            cnMantenimiento.Open();
            sql = "select top 5 [orden_id], [ordenStatus], [ordenCodigoSolicitante], [ordenNombreSolicitante], [CostoTotal] from dbo.ordenesBodegaInsumos where [ordenStatus]='Descargada' order by ordenFecha desc";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            //agregar solicitudes recibidas aprobadas entregadas a las listas
            while (dr.Read())
            {
                listaSolicitudesDescargadas.Add(new solicitudInsumo() { ordenIdNum= Convert.ToInt32(dr["orden_id"]), ordenNombreSolicitante=dr["ordenNombreSolicitante"].ToString(), costC = Convert.ToDouble(dr["CostoTotal"]).ToString("C") });
            };
            dr.Close();
            cnMantenimiento.Close();
            Descargada.ItemsSource = listaSolicitudesDescargadas;
        }

        private void buttonCanceladas_Click(object sender, RoutedEventArgs e)
        {
            List<solicitudInsumo> listaSolicitudesCanceladas = new List<solicitudInsumo>();
            cnMantenimiento.Open();
            sql = "select top 5 [orden_id], [ordenStatus], [ordenCodigoSolicitante], [ordenNombreSolicitante], [CostoTotal] from dbo.ordenesBodegaInsumos where [ordenStatus]='Cancelada' order by ordenFecha desc";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            //agregar solicitudes recibidas aprobadas entregadas a las listas
            while (dr.Read())
            {
                listaSolicitudesCanceladas.Add(new solicitudInsumo() { ordenIdNum = Convert.ToInt32(dr["orden_id"]), ordenNombreSolicitante = dr["ordenNombreSolicitante"].ToString(), costC = Convert.ToDouble(dr["CostoTotal"]).ToString("C") });
            };
            dr.Close();
            cnMantenimiento.Close();
            Cancelada.ItemsSource = listaSolicitudesCanceladas;

        }
        #endregion
    }
}

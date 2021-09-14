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
        int autoriza_ = 0;
        string cargo_ = "";
        #region conexionesConBasesSQL
        public SqlConnection cnMantenimiento = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public string sql; //Consulta que se hace en sql
        public SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
        public SqlDataReader dr; //leer los resultados del comando sql
        #endregion
        #region datosIniciales
        public estadoSolicitudesInsumos(int autoriza, string cargo="")
        {
            InitializeComponent();
            autoriza_ = autoriza;
            this.CreatePermission();

            MessageModel model = new MessageModel(this.Dispatcher);
            this.DataContext = model;
            cargo_ = cargo;
            if (cargo == "ADMINISTRADOR3")
            {
                Recibida.Opacity = 0.4;
                Aprobada.Opacity = 1;
                Entregada.Opacity = 1;
                Descargada.Opacity = 1;
                Cancelada.Opacity = 1;
            }
            else if (cargo == "")
            {
                Recibida.Opacity = 1;
                Aprobada.Opacity = 1;
                Entregada.Opacity = 0.4;
                Descargada.Opacity = 0.4;
                Cancelada.Opacity = 0.4;
            }
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
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = Brushes.White;
            PagePrincipal PagePrincipal = new PagePrincipal();
            this.NavigationService.Navigate(PagePrincipal);
        }
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


            switch (cargo_)
            {
                case "ADMINISTRADOR3":
                    if (informacionElemento.autorizado == "Recibida"|| status=="Recibida")
                    {

                    }
                    else
                    {
                        sql = "update ordenesBodegaInsumos set ordenStatus='" + status + "' where orden_id='" + informacionElemento.ordenId + "'";
                        cnMantenimiento.Open();
                        cm = new SqlCommand(sql, cnMantenimiento);
                        cm.ExecuteNonQuery();
                        cnMantenimiento.Close();
                    }
                    break;
                case "":
                    if (informacionElemento.autorizado == "Recibida" || (status == "Recibida" && informacionElemento.autorizado=="Aprobada"))
                    {
                        sql = "update ordenesBodegaInsumos set ordenStatus='" + status + "', autoriza='" + autoriza_ + "' where orden_id='" + informacionElemento.ordenId+"'";
                        cnMantenimiento.Open();
                        cm = new SqlCommand(sql, cnMantenimiento);
                        cm.ExecuteNonQuery();
                        cnMantenimiento.Close();
                    }
                    else
                    {
                    }
                    break;
            }
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
                int numeroSolici = itemSeleccionado.ordenId;
                labelNumeroDeSolicitud.Content = numeroSolici.ToString();
                List<solicitudInsumo> listaResumenSolicitudes = new List<solicitudInsumo>();
                cnMantenimiento.Open();
                sql = "select ordenId, insumo, descripcion, cantidad, total, comentario from ordenesBodegaInsumosDetalles where ordenId='"+numeroSolici+"'";
                cm = new SqlCommand(sql, cnMantenimiento);
                dr = cm.ExecuteReader();
                //agregar solicitudes recibidas aprobadas entregadas a las listas
                while (dr.Read())
                {
                    listaResumenSolicitudes.Add(new solicitudInsumo() { partNumber=dr["insumo"].ToString(), description = dr["descripcion"].ToString(), solicitado = Convert.ToInt32(dr["cantidad"]), costC = Convert.ToDouble(dr["total"]).ToString("C"), comentario=dr["comentario"].ToString() });    
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
                listaSolicitudesDescargadas.Add(new solicitudInsumo() { ordenId= Convert.ToInt32(dr["orden_id"]), ordenNombreSolicitante=dr["ordenNombreSolicitante"].ToString(), costC = Convert.ToDouble(dr["CostoTotal"]).ToString("C") });
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
                listaSolicitudesCanceladas.Add(new solicitudInsumo() { ordenId = Convert.ToInt32(dr["orden_id"]), ordenNombreSolicitante = dr["ordenNombreSolicitante"].ToString(), costC = Convert.ToDouble(dr["CostoTotal"]).ToString("C") });
            };
            dr.Close();
            cnMantenimiento.Close();
            Cancelada.ItemsSource = listaSolicitudesCanceladas;

        }
        #endregion
        #region popUpListaFueraDeLibros
        private void buttonCerrarDetalles_Click(object sender, RoutedEventArgs e)
        {
            detalles.IsOpen = false;
        }
        private void ButtonAgregarFueraDeSistema_Click(object sender, RoutedEventArgs e)
        {
            textBoxBuscarItem.Clear();
            listBoxItems.Items.Clear();
            textBoxCantidad.Clear();
            popUpAgregarItems.IsOpen = true;
            List<solicitudInsumo> listaFueraDeLibrosContables = new List<solicitudInsumo>();
            List<solicitudInsumo> listaItems = new List<solicitudInsumo>();
            cnMantenimiento.Open();
            sql = "select id, partNumber, cantidad from fueraDeLibrosContables";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            //agregar solicitudes recibidas aprobadas entregadas a las listas
            while (dr.Read())
            {
                listaFueraDeLibrosContables.Add(new solicitudInsumo() { ordenId = Convert.ToInt32(dr["id"]), partNumber=dr["partNumber"].ToString(), onHand=Convert.ToInt32(dr["cantidad"])});
            };
            dr.Close();
            listViewItemsActuales.ItemsSource = listaFueraDeLibrosContables;
            sql = "select top 100 PartNumber from spare_onhand";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            //agregar solicitudes recibidas aprobadas entregadas a las listas
            while (dr.Read())
            {
                listBoxItems.Items.Add(new solicitudInsumo() {partNumber = dr["partNumber"].ToString()});
            };
            dr.Close();
            cnMantenimiento.Close();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popUpAgregarItems.IsOpen = false;
        }
        private void textBoxBuscarItem_TextChanged(object sender, TextChangedEventArgs e)
        {
            listBoxItems.Items.Clear();
            cnMantenimiento.Open();
            sql = sql = "select top 100 PartNumber from spare_onhand where partNumber like '%" + textBoxBuscarItem.Text + "%'" ;
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            //agregar solicitudes recibidas aprobadas entregadas a las listas
            while (dr.Read())
            {
                listBoxItems.Items.Add(new solicitudInsumo() { partNumber = dr["partNumber"].ToString() });
            };
            dr.Close();
            cnMantenimiento.Close();
        }
        private void buttonAgregar_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxItems.SelectedIndex > -1 && string.IsNullOrEmpty(textBoxCantidad.Text) == false)
            {
                List<solicitudInsumo> listaFueraDeLibrosContables = new List<solicitudInsumo>();
                cnMantenimiento.Open();
                sql = sql = "insert into fueraDeLibrosContables(partNumber, cantidad) values('"+ ((solicitudInsumo)listBoxItems.SelectedItem).partNumber + "', '"+ textBoxCantidad.Text+"')" ;
                cm = new SqlCommand(sql, cnMantenimiento);
                cm.ExecuteNonQuery();

                // mostrar items que se han agregado

                sql = "select id, partNumber, cantidad from fueraDeLibrosContables";
                cm = new SqlCommand(sql, cnMantenimiento);
                dr = cm.ExecuteReader();
                //agregar solicitudes recibidas aprobadas entregadas a las listas
                while (dr.Read())
                {
                    listaFueraDeLibrosContables.Add(new solicitudInsumo() { ordenId = Convert.ToInt32(dr["id"]), partNumber = dr["partNumber"].ToString(), onHand = Convert.ToInt32(dr["cantidad"]) });
                };
                dr.Close();
                listViewItemsActuales.ItemsSource = listaFueraDeLibrosContables;
                cnMantenimiento.Close();
            }
            else
            {
            }
        }
        private void listViewItemsActuales_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewItemsActuales.SelectedIndex > -1)
            {
                List<solicitudInsumo> listaFueraDeLibrosContables = new List<solicitudInsumo>();
                cnMantenimiento.Open();
                sql = sql = "delete from fueraDeLibrosContables where id='" + ((solicitudInsumo)listViewItemsActuales.SelectedItem).ordenId + "'";
                cm = new SqlCommand(sql, cnMantenimiento);
                cm.ExecuteNonQuery();

                // mostrar items que se han agregado

                sql = "select id, partNumber, cantidad from fueraDeLibrosContables";
                cm = new SqlCommand(sql, cnMantenimiento);
                dr = cm.ExecuteReader();
                //agregar solicitudes recibidas aprobadas entregadas a las listas
                while (dr.Read())
                {
                    listaFueraDeLibrosContables.Add(new solicitudInsumo() { ordenId = Convert.ToInt32(dr["id"]), partNumber = dr["partNumber"].ToString(), onHand = Convert.ToInt32(dr["cantidad"]) });
                };
                dr.Close();
                listViewItemsActuales.ItemsSource = listaFueraDeLibrosContables;
                cnMantenimiento.Close();

            }
        }
        #endregion

        private void pagina_Loaded(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2A2C32");
        }
    }
}

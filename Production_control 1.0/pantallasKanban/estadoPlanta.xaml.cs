using Production_control_1._0.clases;
using Production_control_1._0.pantallasKanban.NotificacionesDeTablaSQL;
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

namespace Production_control_1._0.pantallasKanban
{
    public partial class estadoPlanta : Page
    {
        string modulo = "";
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
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = Brushes.White;
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
            Uri eliminar_habilitado = new Uri("/imagenes/eliminar.png", UriKind.RelativeOrAbsolute);
            Uri iniciar_inhabilitado = new Uri("/imagenes/iniciar_in.png", UriKind.RelativeOrAbsolute);
            Uri terminar_inhabilitado = new Uri("/imagenes/terminar_in.png", UriKind.RelativeOrAbsolute);
            Uri eliminar_inhabilitado = new Uri("/imagenes/eliminar_in.png", UriKind.RelativeOrAbsolute);


            if (listBoxSeleccionado.SelectedIndex > -1)
            {
                solicitudKanban itemseleccionado = (solicitudKanban)listBoxSeleccionado.SelectedItem;

                if (itemseleccionado.validadoSmed == true)
                {
                    popUpEstadoModulo.IsOpen = true;
                    labelNumeroAccion.Content = itemseleccionado.solicitudKanbanId;
                    labelModuloAccion.Content = itemseleccionado.modulo;
                    labelTipoDeAccion.Content = itemseleccionado.tipo;
                    labelHoraDeReporte.Content = Convert.ToDateTime(itemseleccionado.fechaSolicitud).ToString("yyyy-MM-dd HH:mm:ss");
                    passCodigoInicia.Clear();
                    labelNombreInicia.Content = "----";

                    //consultar materiales solicitados
                    consultarMaterialesSolicitados(itemseleccionado.solicitudKanbanId);

                    if (itemseleccionado.fechaInicio == "1900-01-01 12:00:00")
                    {
                        labelHoraDeApertura.Content = "----";
                        labelNombreAcargo.Content = "----";
                        labelEstadoDeAccion.Content = "Pendiente";
                        buttonTerminarAccion.IsEnabled = false;
                        buttonIniciarAccion.IsEnabled = true;
                        buttonEliminarAccion.IsEnabled = true;
                        passCodigoInicia.Visibility = Visibility.Visible;
                        labelIniciar.Visibility = Visibility.Visible;
                        labelNombreInicia.Visibility = Visibility.Visible;
                        imageIniciar.Source = new BitmapImage(iniciar_habilitado);
                        imageTerminar.Source = new BitmapImage(terminar_inhabilitado);
                        imageEliminar.Source = new BitmapImage(eliminar_habilitado);
                    }
                    else
                    {
                        #region obtenerNombreDePreparador
                        SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                        string sql = "select nombre from usuarios where codigo='" + itemseleccionado.atiendeSolicitud + "'";
                        cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        if (dr.Read())
                        {
                            labelNombreAcargo.Content = dr["nombre"].ToString();
                        };
                        dr.Close();
                        cn.Close();
                        #endregion
                        labelHoraDeApertura.Content = Convert.ToDateTime(itemseleccionado.fechaInicio).ToString("yyyy-MM-dd HH:mm:ss");
                        labelEstadoDeAccion.Content = "Abierta";
                        buttonIniciarAccion.IsEnabled = false;
                        buttonTerminarAccion.IsEnabled = true;
                        buttonEliminarAccion.IsEnabled = false;
                        passCodigoInicia.Visibility = Visibility.Hidden;
                        labelIniciar.Visibility = Visibility.Hidden;
                        labelNombreInicia.Visibility = Visibility.Hidden;
                        imageIniciar.Source = new BitmapImage(iniciar_inhabilitado);
                        imageTerminar.Source = new BitmapImage(terminar_habilitado);
                        imageEliminar.Source = new BitmapImage(eliminar_inhabilitado);

                    }
                }
                else
                {
                    popUpValidar.IsOpen = true;
                    //consultar materiales solicitados
                    consultarMaterialesSolicitados(itemseleccionado.solicitudKanbanId);
                    labelNumeroAccionSmed.Content = itemseleccionado.solicitudKanbanId;
                }
            }
        }
        private void consultarMaterialesSolicitados(int id)
        {
            listViewListaMateriales.Items.Clear();
            listViewListaMaterialesValidar.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select detalleKanbanId, solicitudKanbanId, lote, material, talla, cantidad, diferencia, entregado from detalleSolicitudeKanban where solicitudKanbanId="+id ;
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                bool habilitado = false;
                bool habilitadoEntrega = true;
                if (dr["material"].ToString().Contains("HANGER"))
                {
                    habilitado = true;
                }
                if (Convert.ToBoolean(dr["entregado"])==true)
                {
                    habilitadoEntrega = false;
                }
                listViewListaMateriales.Items.Add(new solicitudKanban {solicitudKanbanId=Convert.ToInt32(dr["detalleKanbanId"]), lote = dr["lote"].ToString(), material = dr["material"].ToString(), talla = dr["talla"].ToString(), cantidad = Convert.ToInt32(dr["cantidad"]), solicitado= Convert.ToInt32(dr["cantidad"]), habilitado=habilitado, diferencia=Convert.ToInt32(dr["diferencia"] is DBNull? 0: dr["diferencia"])+ Convert.ToInt32(dr["cantidad"]), chequeado= Convert.ToBoolean(dr["entregado"]), habilitadoEntrega=habilitadoEntrega });
                listViewListaMaterialesValidar.Items.Add(new solicitudKanban { solicitudKanbanId = Convert.ToInt32(dr["detalleKanbanId"]), lote = dr["lote"].ToString(), material = dr["material"].ToString(), talla = dr["talla"].ToString(), cantidad = Convert.ToInt32(dr["cantidad"]), solicitado = Convert.ToInt32(dr["cantidad"]), habilitado = habilitado, diferencia = Convert.ToInt32(dr["diferencia"] is DBNull ? 0 : dr["diferencia"]) + Convert.ToInt32(dr["cantidad"]), chequeado = Convert.ToBoolean(dr["entregado"]), habilitadoEntrega = habilitadoEntrega });
            }
            dr.Close();
            cn.Close();
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
            if(labelNombreInicia.Content.ToString() != "----")
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "update solicitudesKanban set fechaInicio='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', atiendeSolicitud='"+labelNombreInicia.Content+"' where solicitudKanbanid='" + labelNumeroAccion.Content + "'";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                foreach(solicitudKanban item in listViewListaMateriales.Items)
                {
                    if (item.material.Contains("HANGER"))
                    {
                        int diferenciaCantidadModificada = item.cantidad;
                        sql = "select cantidad from detalleSolicitudeKanban where detalleKanbanId='" + item.solicitudKanbanId + "'";
                        cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        if (dr.Read())
                        {
                            diferenciaCantidadModificada = diferenciaCantidadModificada - Convert.ToInt32(dr["cantidad"]);
                        }
                        dr.Close();

                        sql = "update detalleSolicitudeKanban set cantidad="+item.cantidad+", diferencia= diferencia-" + diferenciaCantidadModificada + " where detalleKanbanId='" + item.solicitudKanbanId + "'";
                        cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                    }

                }
                cn.Close();
                popUpEstadoModulo.IsOpen = false;
            }

        }
        private void buttonTerminarAccion_Click(object sender, RoutedEventArgs e)
        {
            passCerrar.Clear();
            labelCodigoAutoriza.Content = "*";
            labelNumeroAccionCerrar.Content = labelNumeroAccion.Content;
            popUpEstadoModulo.IsOpen = false;
            popUpCerrar.IsOpen = true;
            modulo = labelModuloAccion.Content.ToString();
        }
        #endregion
        #region popUpCerrar
        private void passCodigoInicia_PasswordChanged(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select codigo from usuarios where cargo='PREPARADOR' and contrasena='"+ passCodigoInicia.Password +"'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            if(dr.Read())
            {
                labelNombreInicia.Content = dr["codigo"].ToString();
            }
            else
            {
                labelNombreInicia.Content = "----";
            }
            cn.Close();
        }
        private void buttonCerrarPopUpCerrar_Click(object sender, RoutedEventArgs e)
        {
            popUpCerrar.IsOpen = false;
        }
        private void buttonCerrarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            if (labelCodigoAutoriza.Content.ToString()!="*")
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                bool terminarSolicitud = true;
                string ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cn.Open();
                foreach (solicitudKanban item in listViewListaMateriales.Items)
                {
                    if(item.habilitadoEntrega==true && item.chequeado == true)
                    {
                        string sql = "update detalleSolicitudeKanban set entregado="+ 1 + " where detalleKanbanId="+item.solicitudKanbanId ;
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();

                        sql = "update solicitudesKanban set fechaParcial='" + ahora + "', autorizaCierre='" + labelCodigoAutoriza.Content.ToString() + "' where solicitudKanbanId='" + labelNumeroAccion.Content + "'";
                        cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                    }
                    else if(item.chequeado == false)
                    {
                        terminarSolicitud = false;
                    }
                }
                if(terminarSolicitud == true)
                {
                    string sql = "update solicitudesKanban set fechaEntrega='" + ahora + "', autorizaCierre='" + labelCodigoAutoriza.Content.ToString() + "' where solicitudKanbanId='" + labelNumeroAccion.Content + "'";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                }
                cn.Close();
                popUpCerrar.IsOpen = false;
            }
            else
            {
                MessageBox.Show("Codigo no valido");
            }
        }
        private void passCerrar_PasswordChanged(object sender, RoutedEventArgs e)
        {
            labelCodigoAutoriza.Content = "*";
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select codigo from usuarios, produccion.dbo.modulosProduccion where (codigo=coordinadorCodigo or codigo=ingenieroProcesosCodigo or codigo=soporteCodigo or codigo=enganchadorCodigo or codigo=empacadorCodigo) AND contrasena='" + passCerrar.Password.ToString() + "' AND modulo='" + modulo + "' and kanban=1";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                labelCodigoAutoriza.Content = dr["codigo"].ToString();
            };
            dr.Close();
            cn.Close();
        }
        #endregion
        #region popEliminar
        private void buttonEliminarAccion_Click(object sender, RoutedEventArgs e)
        {
            labelNumeroAccionEliminar.Content = labelNumeroAccion.Content;
            passEliminar.Clear();
            labelCodigoAutorizaEliminar.Content = "*";
            popUpEstadoModulo.IsOpen = false;
            popUpEliminar.IsOpen = true;
            modulo = labelModuloAccion.Content.ToString();
        }
        private void buttonCerrarPopUpEliminar_Click(object sender, RoutedEventArgs e)
        {
            popUpEliminar.IsOpen = false;
        }
        private void passEliminar_PasswordChanged(object sender, RoutedEventArgs e)
        {
            labelCodigoAutorizaEliminar.Content = "*";
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select codigo from usuarios, produccion.dbo.modulosProduccion where (codigo=coordinadorCodigo or codigo=ingenieroProcesosCodigo or codigo=soporteCodigo or codigo=enganchadorCodigo or codigo=empacadorCodigo) AND contrasena='" + passEliminar.Password.ToString() + "' AND modulo='" + modulo + "' and kanban=1";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                labelCodigoAutorizaEliminar.Content = dr["codigo"].ToString();
            };
            dr.Close();
            cn.Close();
        }
        private void buttonEliminarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            if (labelCodigoAutorizaEliminar.Content.ToString() != "*")
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "delete from solicitudesKanban where solicitudKanbanId='"+ labelNumeroAccionEliminar.Content +"'";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                cn.Close();
                popUpEliminar.IsOpen = false;
            }
            else
            {
                MessageBox.Show("Codigo no valido");
            }
        }
        #endregion
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = Brushes.Black;
        }
        private void buttonCerrarPopUpValidarSmed_Click(object sender, RoutedEventArgs e)
        {
            popUpValidar.IsOpen = false;
        }
        private void buttonEliminarAccionSmed_Click(object sender, RoutedEventArgs e)
        {
            labelNumeroAccionEliminar.Content = labelNumeroAccionSmed.Content;
            passEliminar.Clear();
            labelCodigoAutorizaEliminar.Content = "*";
            popUpValidar.IsOpen = false;
            popUpEliminar.IsOpen = true;
            modulo = labelModuloAccion.Content.ToString();
        }
    }
}

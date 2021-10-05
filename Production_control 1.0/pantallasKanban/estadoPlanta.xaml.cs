using Production_control_1._0.clases;
using Production_control_1._0.pantallasKanban.NotificacionesDeTablaSQL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;

namespace Production_control_1._0.pantallasKanban
{
    public partial class estadoPlanta : Page
    {
        string modulo = "";
        string[] motivos_ = new string[4];
        string[] areas_ = new string[6];
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

            motivos_[0] = "-";
            motivos_[1] = "Pendiente de preparación";
            motivos_[2] = "Pendiente de aprobación";
            motivos_[3] = "Falta de materiales";

            areas_[0] = "-";
            areas_[1] = "BMP";
            areas_[2] = "Corte";
            areas_[3] = "Product Id";
            areas_[4] = "Receiving";
            areas_[5] = "Calidad";

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
                    modulo = itemseleccionado.modulo;
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
            string sql = "select detalleKanbanId, solicitudKanbanId, lote, material, talla, cantidad, diferencia, entregado, motivoEntregaParcial, areaResponsable from detalleSolicitudeKanban where solicitudKanbanId="+id ;
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
                listViewListaMateriales.Items.Add(new solicitudKanban {solicitudKanbanId=Convert.ToInt32(dr["detalleKanbanId"]), lote = dr["lote"].ToString(), material = dr["material"].ToString(), talla = dr["talla"].ToString(), cantidad = Convert.ToInt32(dr["cantidad"]), solicitado= Convert.ToInt32(dr["cantidad"]), habilitado=habilitado, diferencia=Convert.ToInt32(dr["diferencia"] is DBNull? 0: dr["diferencia"])+ Convert.ToInt32(dr["cantidad"]), chequeado= Convert.ToBoolean(dr["entregado"]), habilitadoEntrega=habilitadoEntrega, motivos=motivos_, areas=areas_, motivo=Convert.ToString(dr["motivoEntregaParcial"] is DBNull? "-": dr["motivoEntregaParcial"]), area= Convert.ToString(dr["areaResponsable"] is DBNull ? "-" : dr["areaResponsable"]) });
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
                        string sql = "update detalleSolicitudeKanban set entregado="+ 1 + ", horaEntrega='" + ahora + "' where detalleKanbanId="+item.solicitudKanbanId ;
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();

                    }
                    else if(item.chequeado == false)
                    {
                        terminarSolicitud = false;

                        string sql = "update detalleSolicitudeKanban set motivoEntregaParcial='" + item.motivo + "', areaResponsable='" + item.area + "' where detalleKanbanId=" + item.solicitudKanbanId;
                        SqlCommand cm = new SqlCommand(sql, cn);
                        cm.ExecuteNonQuery();
                    }
                }
                if(terminarSolicitud == true)
                {
                    string sql = "update solicitudesKanban set fechaEntrega='" + ahora + "', autorizaCierre='" + labelCodigoAutoriza.Content.ToString() + "' where solicitudKanbanId='" + labelNumeroAccion.Content + "'";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                }
                else
                {
                    string sql = "update solicitudesKanban set fechaParcial='" + ahora + "', autorizaCierre='" + labelCodigoAutoriza.Content.ToString() + "' where solicitudKanbanId='" + labelNumeroAccion.Content + "'";
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
        }

        private void buttonValidarAccionSmed_Click(object sender, RoutedEventArgs e)
        {
            popUpValidar.IsOpen = false;
            popUpAutorizar.IsOpen = true;
            labelNumeroAccionAutorizar.Content = labelNumeroAccionSmed.Content;
        }

        private void buttonCerrarPopUpAutorizar_Click(object sender, RoutedEventArgs e)
        {
            popUpAutorizar.IsOpen = false;
        }

        private void passAutorizar_PasswordChanged(object sender, RoutedEventArgs e)
        {
            labelCodigoAutorizaAuto.Content = "*";
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select codigo from usuarios where contrasena='" + passAutorizar.Password.ToString() + "' AND nivel=1 and [ingenieria/SMED]=1";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                labelCodigoAutorizaAuto.Content = dr["codigo"].ToString();
            };
            dr.Close();
            cn.Close();
        }
        private void buttonAutorizarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            if (labelCodigoAutorizaAuto.Content.ToString() != "*")
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                bool terminarSolicitud = true;
                string ahora = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                cn.Open();
                if (terminarSolicitud == true)
                {
                    string sql = "update solicitudesKanban set validadoSmed=" + 1 + " where solicitudKanbanId='" + labelNumeroAccionAutorizar.Content + "'";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    cm.ExecuteNonQuery();
                }
                cn.Close();
                popUpAutorizar.IsOpen = false;
            }
            else
            {
                MessageBox.Show("Codigo no valido");
            }
        }

        public void Print_WPF_Preview(FrameworkElement wpf_Element, string nomre)

        {

            //------------< WPF_Print_current_Window >------------

            //--< create xps document >--

            XpsDocument doc = new XpsDocument(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\" + nomre, FileAccess.ReadWrite);

            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(doc);

            SerializerWriterCollator preview_Document = writer.CreateVisualsCollator();

            preview_Document.BeginBatchWrite();

            preview_Document.Write(wpf_Element);  //*this or wpf xaml control

            preview_Document.EndBatchWrite();

            //--</ create xps document >--



            //var doc2 = new XpsDocument("Druckausgabe.xps", FileAccess.Read);



            FixedDocumentSequence preview = doc.GetFixedDocumentSequence();



            var window = new Window();

            window.Content = new DocumentViewer { Document = preview };

            window.ShowDialog();



            doc.Close();

            //------------</ WPF_Print_current_Window >------------





        }

        private void buttonImprimir_Click(object sender, RoutedEventArgs e)
        {
            buttonCerrarPopUpEstadoModulo.Visibility = Visibility.Hidden;
            buttonEliminarAccion.Visibility = Visibility.Hidden;
            buttonIniciarAccion.Visibility = Visibility.Hidden;
            buttonTerminarAccion.Visibility = Visibility.Hidden;
            buttonImprimir.Visibility = Visibility.Hidden;

            string nombre = labelModuloAccion.Content+"_"+ labelNumeroAccion.Content +"_"+ Guid.NewGuid().ToString("n").Substring(0, 8)+".xps";
            Print_WPF_Preview(area_imp,nombre);

            buttonCerrarPopUpEstadoModulo.Visibility = Visibility.Visible;
            buttonEliminarAccion.Visibility = Visibility.Visible;
            buttonIniciarAccion.Visibility = Visibility.Visible;
            buttonTerminarAccion.Visibility = Visibility.Visible;
            buttonImprimir.Visibility = Visibility.Visible;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            listViewListaMateriales.SelectedItem = checkBox.DataContext;

            solicitudKanban item = (solicitudKanban)listViewListaMateriales.SelectedItem;

            item.area = "-";
            item.motivo = "-";

            listViewListaMateriales.Items.Refresh();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            listViewListaMateriales.SelectedItem = comboBox.DataContext;

            solicitudKanban item = (solicitudKanban)listViewListaMateriales.SelectedItem;
        }
    }
}

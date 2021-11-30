using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JazzCCO._0.clases;
using JazzCCO._0.pantallasMantenimiento.NotificacionesDeTablaSQL;


namespace JazzCCO._0
{
    public partial class estadoPlantaProduccion : Page
    {
        string modulo_ = "";
        #region datos_iniciales()
        public estadoPlantaProduccion()
        {
            InitializeComponent();
            //se revisa cual es la distribucion de los modulos
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
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
            cnIngenieria.Open();
            sql = "select codigo from usuarios where cargo='MECANICO' or cargo='ELECTRICISTA'";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxAsignarMecanico.Items.Add(Convert.ToInt32(dr["codigo"]));
                codigo_mec_re.Items.Add(Convert.ToInt32(dr["codigo"]));
            }
            dr.Close();
            cnIngenieria.Close();

            this.CreatePermission();
            MessageModelPlanta model = new MessageModelPlanta(this.Dispatcher);
            this.DataContext = model;
            UIGlobal.MainPage = this;
        }
        public static class UIGlobal
        {
            public static estadoPlantaProduccion MainPage { get; set; }
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
                                listviewMenu.SelectedIndex = 2;
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
        #region tamanos_de_letra_/_tipo_de_texto
        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height*0.8 / tmp.FontFamily.LineSpacing;
        }
        private void letra_modulos_nombres(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.6 / tmp.FontFamily.LineSpacing;
        }
        private void letra_priori_solicitudes (object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
        }
        private void letra_pop_cerrar (object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
        }
        private void letra_pequena(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.06 / tmp.FontFamily.LineSpacing;
        }
        private void letra_mediana(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.3 / tmp.FontFamily.LineSpacing;
        }
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion
        #region abrir_pop_up_con_datos_de_modulo
        private void mostrarPopUp(object sender, MouseButtonEventArgs e)
        {
            ListBox modulo = ((ListBox)sender);
            //strings generales para la conexion a la base y la direccion de las imagenes de los botones (se cambian para hacer evidente si estan habilitados o no)
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            Uri iniciarMeca_habilitado = new Uri("/imagenes/iniciarMeca.png", UriKind.RelativeOrAbsolute);
            Uri iniciarMeca_inhabilitado = new Uri("/imagenes/iniciarMeca _in.png", UriKind.RelativeOrAbsolute);
            Uri iniciar_habilitado = new Uri("/imagenes/iniciar.png", UriKind.RelativeOrAbsolute);
            Uri pausar_habilitado = new Uri("/imagenes/pausa.png", UriKind.RelativeOrAbsolute);
            Uri reanudar_habilitado = new Uri("/imagenes/reanudar.png", UriKind.RelativeOrAbsolute);
            Uri terminar_habilitado = new Uri("/imagenes/terminar.png", UriKind.RelativeOrAbsolute);
            Uri iniciar_inhabilitado = new Uri("/imagenes/iniciar_in.png", UriKind.RelativeOrAbsolute);
            Uri pausar_inhabilitado = new Uri("/imagenes/pausa_in.png", UriKind.RelativeOrAbsolute);
            Uri reanudar_inhabilitado = new Uri("/imagenes/reanudar_in.png", UriKind.RelativeOrAbsolute);
            Uri terminar_inhabilitado = new Uri("/imagenes/terminar_in.png", UriKind.RelativeOrAbsolute);

            //si no hay ningun elemento seleccionado no se muestran la ventana emergente
            if (modulo.SelectedIndex > -1)
            {
                //limpiar en caso de que se haya seleccionado uno antes
                codigo_mecanico.Content = "----";
                mecanico.Content = "----";
                hora_asignacion.Content = "----";
                hora_apertura.Content = "----";

                //se determina el tamano de la ventana emergente
                #region tamano_de_pop
                datos_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                datos_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                datos_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                datos_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                #endregion
                //se abre la ventana emergente
                datos_solicitud.IsOpen = true;
                //se evalua el problema seleccionado

                solicitudMaquina item = (solicitudMaquina)modulo.SelectedItem;

                // se agregan el problema, maquina, solicitud y la hora a la que se hizo del problema seleccionado
                problema.Content = item.problema_reportado.ToString();
                maquina.Content = item.maquina.ToString();
                solicitud.Content = item.id_solicitud.ToString();
                hora_reporte.Content = item.hora_reportada.ToString();
                modulo_ = item.modulo;

                //se evalua si ya esta abierto
                if (item.hora_asignacion.ToString() != "0")
                {
                    hora_asignacion.Content = item.hora_asignacion.ToString();
                    estado.Content = "Asignada";
                    img_iniciarMeca.Source = new BitmapImage(iniciarMeca_inhabilitado);
                    buttonAsignarMecanico.IsEnabled = false;
                    iniciar.IsEnabled = true;
                    pausar.IsEnabled = false;
                    reanudar.IsEnabled = false;
                    terminar.IsEnabled = false;

                    //se revisa en la tabla de tiempos por mecanico quien abrio el problema 
                    string sql = "select top 1 mecanico, usuarios.nombre from tiempos_por_mecanico left join ingenieria.dbo.usuarios on usuarios.codigo = mecanico where (mecanico<>'----' or mecanico is not null) and num_solicitud=" + item.id_solicitud + "order by hora desc";
                    cn.Open();
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    if(dr.Read())
                    {
                        mecanico.Content = Convert.ToString(dr["nombre"] is DBNull ? "----" : dr["nombre"]);
                        codigo_mecanico.Content = Convert.ToString(dr["mecanico"] is DBNull ? "----" : dr["mecanico"]);
                    };
                    dr.Close();
                    cn.Close();


                    if (item.hora_apertura.ToString() != "0")
                    {
                        //si ya esta abierto se coloca la hora de apertura, por defecto el estado se coloca en abierto y se inabilita el boton de iniciar
                        hora_apertura.Content = item.hora_apertura.ToString();
                        estado.Content = "Abierta";
                        iniciar.IsEnabled = false;
                        pausar.IsEnabled = true;
                        reanudar.IsEnabled = false;
                        terminar.IsEnabled = true;

                        //se cargan las imagenes de acuerdo a si estan o no habilitados
                        img_iniciar.Source = new BitmapImage(iniciar_inhabilitado);
                        img_pausar.Source = new BitmapImage(pausar_habilitado);
                        img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                        img_terminar.Source = new BitmapImage(terminar_habilitado);

                        //se revisa si existen pausas abiertas de la solicitud (eso se ve en la tabla pausas: 1 es inicio de pausa un -1 es un cierre de pausa)
                        cn.Open();
                        string sql2 = "select top 1 tipo from pausas where num_solicitud=" + item.id_solicitud + "order by hora desc";
                        SqlCommand cm2 = new SqlCommand(sql2, cn);
                        SqlDataReader dr2 = cm2.ExecuteReader();
                        while (dr2.Read())
                        {
                            if (dr2["tipo"].ToString() == "1")
                            {
                                //si esta pausado se coloca el estado y la imagen correspondiente a cada boton
                                estado.Content = "Pausado";
                                iniciar.IsEnabled = false;
                                pausar.IsEnabled = false;
                                reanudar.IsEnabled = true;
                                terminar.IsEnabled = false;

                                img_iniciar.Source = new BitmapImage(iniciar_inhabilitado);
                                img_pausar.Source = new BitmapImage(pausar_inhabilitado);
                                img_reanudar.Source = new BitmapImage(reanudar_habilitado);
                                img_terminar.Source = new BitmapImage(terminar_inhabilitado);
                            }
                            else
                            {
                                //si no esta pausado se coloca el estado y la imagen correspondiente a cada boton
                                estado.Content = "Abierta";
                                iniciar.IsEnabled = false;
                                pausar.IsEnabled = true;
                                reanudar.IsEnabled = false;
                                terminar.IsEnabled = true;

                                img_iniciar.Source = new BitmapImage(iniciar_inhabilitado);
                                img_pausar.Source = new BitmapImage(pausar_habilitado);
                                img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                                img_terminar.Source = new BitmapImage(terminar_habilitado);
                            }
                        };
                        cn.Close();
                    }
                    else
                    {
                        //si esta sin abrir se coloca el estado y la imagen que corresponde a cada boton
                        hora_apertura.Content = "----";
                        iniciar.IsEnabled = true;
                        pausar.IsEnabled = false;
                        reanudar.IsEnabled = false;
                        terminar.IsEnabled = false;

                        img_iniciar.Source = new BitmapImage(iniciar_habilitado);
                        img_pausar.Source = new BitmapImage(pausar_inhabilitado);
                        img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                        img_terminar.Source = new BitmapImage(terminar_inhabilitado);
                    }
                }
                else
                {
                    estado.Content = "Sin Asignar";
                    mecanico.Content = "----";
                    codigo_mecanico.Content = "----";
                    buttonAsignarMecanico.IsEnabled = true;
                    iniciar.IsEnabled = false;
                    pausar.IsEnabled = false;
                    reanudar.IsEnabled = false;
                    terminar.IsEnabled = false;

                    img_iniciarMeca.Source = new BitmapImage(iniciarMeca_habilitado);
                    img_iniciar.Source = new BitmapImage(iniciar_inhabilitado);
                    img_pausar.Source = new BitmapImage(pausar_inhabilitado);
                    img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                    img_terminar.Source = new BitmapImage(terminar_inhabilitado);
                }
            }
        }
        #endregion
        #region botones_pop_uo
        #region botones_pop_principal
        private void buttonAsignarMecanico_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            asignar_solicitud.Width = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            asignar_solicitud.Height = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            comboBoxAsignarMecanico.Width = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            comboBoxAsignarMecanico.Height = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;
            #endregion
            comboBoxAsignarMecanico.SelectedIndex = -1;
            id_asignar.Content = solicitud.Content.ToString();
            asignar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }
        private void iniciar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            abrir_solicitud.Width = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            abrir_solicitud.Height = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            passwordBoxCodigoAbre.Width = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            passwordBoxCodigoAbre.Height = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;
            #endregion
            passwordBoxCodigoAbre.Password = "";
            id_1.Content = solicitud.Content.ToString();
            Uri iniciar_inhabilitado = new Uri("/imagenes/iniciar_in.png", UriKind.RelativeOrAbsolute);
            img_verificar_inicio.Source = img_iniciarMeca.Source = new BitmapImage(iniciar_inhabilitado);
            labelNombreAbre.Content = "*";
            btn_iniciar.IsEnabled = false;
            abrir_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }
        private void pausar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            pausar_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            pausar_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            pausar_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            pausar_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;

            #endregion
            //se limpian los items de motivos de pausa
            motivo_de_pausa.Items.Clear();
            id_2.Content = solicitud.Content.ToString();
            meca_.Content = codigo_mecanico.Content.ToString();
            //se consulta en la base la lista y se agregan
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select motivo from motivos_de_pausa";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                motivo_de_pausa.Items.Add(dr["motivo"].ToString());
            };
            dr.Close();
            cn.Close();

            //se abre el pop_up para reportar motivo de pausa
            pausar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }
        private void reanudar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            reanudar_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            reanudar_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            reanudar_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            reanudar_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;

            codigo_mec_re.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            codigo_mec_re.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 8;
            codigo_mec_re.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;
            codigo_mec_re.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 30;

            #endregion
            codigo_mec_re.SelectedIndex=-1;
            id_3.Content = solicitud.Content.ToString();
            reanudar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }
        private void terminar_Click(object sender, RoutedEventArgs e)
        {
            #region tamano_de_pop
            terminar_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            terminar_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
            terminar_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
            terminar_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;

            #endregion
            //se limpian los items de motivos de pausa
            id_4.Content = solicitud.Content.ToString();
            meca_2.Content = codigo_mecanico.Content.ToString();
            motivo_real.Items.Clear();
            textBoxComentario.Clear();

            //se consulta en la base la lista y se agregan
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select falla from defectos_totales";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                motivo_real.Items.Add(dr["falla"].ToString());
            };
            dr.Close();
            cn.Close();

            //se abre el pop_up para reportar motivo de pausa
            terminar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }
        #endregion
        #region botones_por_pop_up
        private void comboBoxAsignarMecanico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxAsignarMecanico.SelectedIndex > -1)
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "update solicitudes set hora_asignacion='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "'  where id_solicitud= '" + id_asignar.Content.ToString() + "'";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_asignar.Content.ToString() + "', '" + comboBoxAsignarMecanico.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn.Close();
                asignar_solicitud.IsOpen = false;
            }
        }
        private void passwordBoxCodigoAbre_PasswordChanged(object sender, RoutedEventArgs e)
        {
            labelNombreAbre.Content = "*";
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select nombre from usuarios, produccion.dbo.modulosProduccion where (codigo=coordinadorCodigo or codigo=ingenieroProcesosCodigo or codigo=soporteCodigo) AND contrasena='" + passwordBoxCodigoAbre.Password + "' AND modulo='" + modulo_ + "' and mantenimiento=1";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            if (dr.Read())
            {
                Uri iniciar_habilitado = new Uri("/imagenes/iniciar.png", UriKind.RelativeOrAbsolute);
                img_verificar_inicio.Source = img_iniciarMeca.Source = new BitmapImage(iniciar_habilitado);
                labelNombreAbre.Content = dr["nombre"].ToString();
                btn_iniciar.IsEnabled = true;
            }
            else
            {
                Uri iniciar_inhabilitado = new Uri("/imagenes/iniciar_in.png", UriKind.RelativeOrAbsolute);
                img_verificar_inicio.Source = img_iniciarMeca.Source = new BitmapImage(iniciar_inhabilitado);
                labelNombreAbre.Content = "*";
                btn_iniciar.IsEnabled = false;
            };
            dr.Close();
            cn.Close();
        }
        private void btn_iniciar_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "update solicitudes set hora_apertura='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', abre='"+labelNombreAbre.Content+"'  where id_solicitud= '" + id_1.Content.ToString() + "'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
            abrir_solicitud.IsOpen = false;
        }
        private void motivo_de_pausa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (motivo_de_pausa.SelectedIndex>=0)
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "insert into pausas (num_solicitud, hora, tipo, motivo) values('" + id_2.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1', '" + motivo_de_pausa.SelectedItem.ToString() + "')";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_2.Content.ToString() + "', '" + meca_.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn.Close();
                pausar_solicitud.IsOpen = false;
            }
            else
            {

            }
        }
        private void btn_reanudar_Click(object sender, RoutedEventArgs e)
        {
            if (codigo_mec_re.SelectedIndex>-1)
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "insert into pausas (num_solicitud, hora, tipo) values('" + id_3.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_3.Content.ToString() + "', '" + codigo_mec_re.SelectedItem.ToString()+ "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn.Close();
                codigo_mec_re.Text = "";
                reanudar_solicitud.IsOpen = false;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (motivo_real.SelectedIndex>= 0 & nombre_autoriza.Content.ToString() !="*")
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "update solicitudes set hora_cierre='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', problema_real= '" + motivo_real.SelectedItem.ToString() + "', autoriza= '" + nombre_autoriza.Content +"', comentario='"+textBoxComentario.Text+"' where id_solicitud= '" + id_4.Content.ToString() + "'";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_4.Content.ToString() + "', '" + meca_2.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn.Close();
                codigo_autoriza.Password = "";
                nombre_autoriza.Content = "*";
                terminar_solicitud.IsOpen = false;
            }
            else
            {

            }
        }
        private void buscar_motivo_real_TextChanged(object sender, TextChangedEventArgs e)
        {
            //se consultan las coincidencias de problemas
            motivo_real.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select falla from defectos_totales where falla like '%" + buscar_motivo_real.Text.ToString()  +  "%'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                motivo_real.Items.Add(dr["falla"].ToString());
            };
            dr.Close();
            cn.Close();
        }
        private void codigo_autoriza_PasswordChanged(object sender, RoutedEventArgs e)
        {
            nombre_autoriza.Content = "*";
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select nombre from usuarios, produccion.dbo.modulosProduccion where (codigo=coordinadorCodigo or codigo=ingenieroProcesosCodigo or codigo=soporteCodigo) AND contrasena='"+codigo_autoriza.Password.ToString()+"' AND modulo='"+modulo_+"' and mantenimiento=1";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nombre_autoriza.Content = dr["nombre"].ToString();
            };
            dr.Close();
            cn.Close();
        }
        #endregion
        #endregion

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            datos_solicitud.IsOpen = false;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            asignar_solicitud.IsOpen = false;
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            abrir_solicitud.IsOpen = false;
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            pausar_solicitud.IsOpen = false;
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            reanudar_solicitud.IsOpen = false;
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            terminar_solicitud.IsOpen = false;
        }

        private void buttonMecanicoUbicacion_Click(object sender, RoutedEventArgs e)
        {
            mecanicosUbicacion.IsOpen = true;
            listViewMecanicoUbicacion.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select id_solicitud, problema_reportado, modulo, nombre from mecanicosUbicacion";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewMecanicoUbicacion.Items.Add(new solicitudMaquina { id_solicitud = Convert.ToInt32(dr["id_solicitud"]), problema_reportado = dr["problema_reportado"].ToString(), modulo = dr["modulo"].ToString(), corresponde = dr["nombre"].ToString() });
            }
            dr.Close();
            cn.Close();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            mecanicosUbicacion.IsOpen = false;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF2A2C32");
        }
    }
}

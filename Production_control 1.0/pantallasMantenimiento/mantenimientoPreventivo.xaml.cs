using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Configuration;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;
using Production_control_1._0.pantallasMantenimiento.NotificacionesDeTablaSQL;
using Production_control_1._0.pantallasMantenimiento.NotificacionesMantenimientoPreventivo;
using Production_control_1._0.clases;
using System.Windows.Media;

namespace Production_control_1._0
{
    public partial class mantenimientoPreventivo : UserControl
    {
        #region clases_especiales()
        public class item_solicitud
        {
            public int id_solicitud { get; set; }
            public string modulo { get; set; }
            public string maquina { get; set; }
            public float operario { get; set; }
            public string problema_reportado { get; set; }
            public string hora_reportada { get; set; }
            public string hora_apertura { get; set; }
            public string hora_cierre { get; set; }
            public string corresponde { get; set; }
        }
        public class item_actualizacion
        {
            public int id { get; set; }
        }
        public string cs = "Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"];
        #endregion
        #region datos_iniciales()
        public mantenimientoPreventivo()
        {
            InitializeComponent();
            //se cargan los datos de las listas de modulos y de problemas
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select modulo from orden_modulos group by modulo";
            string sql3 = "select falla from defectos_totales where categoria='MANTENIMIENTO PREVENTIVO'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulo.Items.Add(dr["modulo"].ToString());
            };
            dr.Close();
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                accion.Items.Add(dr3["falla"].ToString());
            };
            dr3.Close();
            cn.Close();

            //llenar lista de mecanicos
            cnIngenieria.Open();
            sql3 = "select codigo from usuarios where cargo='MECANICO' or cargo='ELECTRICISTA'";
            cm3 = new SqlCommand(sql3, cnIngenieria);
            dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                codigo.Items.Add(dr3["codigo"].ToString());
            };
            dr3.Close();
            cnIngenieria.Close();


            //se habilita o ibhabilita el boton
            habilitar_boton();
            this.CreatePermission();
            mensaje model = new mensaje(this.Dispatcher);
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
        #region tamanos_de_letra_/_tipo_de_texto
        private void letra_grande(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.8 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letra_grande_2(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.47 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letra_pequena(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.15 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letra_pequena_2(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.03 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letra_pequena_3(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.07 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letra_mediana(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.3 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letra_pop_cerrar(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.8 / tmp.FontFamily.LineSpacing;
        }
        #endregion
        #region calculos_generales
        private void habilitar_boton()
        {
            Uri habilitado = new Uri("/imagenes/flecha.png", UriKind.RelativeOrAbsolute);
            Uri inhabilitado = new Uri("/imagenes/flecha_in.png", UriKind.RelativeOrAbsolute);

            if (modulo.SelectedIndex > -1 & codigo.SelectedIndex > -1 & accion.SelectedIndex > -1 )
            {
                enviar.IsEnabled = true;
                img_enviar.Source = new BitmapImage(habilitado);
            }
            else
            {
                enviar.IsEnabled = false;
                img_enviar.Source = new BitmapImage(inhabilitado);
            }
        }
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasIniciales.mantenimiento());
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
        #region control_de_formulario
        private void modulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //revisar la ubicacion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            String sql = "select id from orden_modulos where modulo='" + modulo.SelectedItem.ToString() + "'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            //si hay cambios cerrados hace menos de 2 horas se ke asigna el reporte a SMED
            if (dr.Read())
            {
                labelUbicacion.Content = dr["id"].ToString();
            };
            dr.Close();
            cn.Close();
            habilitar_boton();
        }
        private void codigo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitar_boton();
        }
        private void accion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitar_boton();
        }
        private void enviar_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "insert into solicitudes (modulo, ubicacion, maquina, problema_reportado, hora_asignacion, hora_reportada, hora_apertura, corresponde, prioridad)  values('" + modulo.SelectedItem.ToString() + "', '" + labelUbicacion.Content.ToString() +"', 'manto', '" + accion.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '"+ DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '" + DateTime.Now.ToString() + "', 'MANTENIMIENTO', 'preventivo')  SELECT SCOPE_IDENTITY()";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            dr.Read();
            int id_ingresado = Convert.ToInt32(dr[0]);
            dr.Close();
            string sql3 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_ingresado + "', '" + codigo.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            cm3.ExecuteNonQuery();
            cn.Close();
        }
        #endregion
        #region pop_reportar_problema
        #region abrir_pop_prin
        private void abiertos_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //strings generales para la conexion a la base y la direccion de las imagenes de los botones (se cambian para hacer evidente si estan habilitados o no)
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            Uri iniciar_habilitado = new Uri("/imagenes/iniciar.png", UriKind.RelativeOrAbsolute);
            Uri pausar_habilitado = new Uri("/imagenes/pausa.png", UriKind.RelativeOrAbsolute);
            Uri reanudar_habilitado = new Uri("/imagenes/reanudar.png", UriKind.RelativeOrAbsolute);
            Uri terminar_habilitado = new Uri("/imagenes/terminar.png", UriKind.RelativeOrAbsolute);
            Uri iniciar_inhabilitado = new Uri("/imagenes/iniciar_in.png", UriKind.RelativeOrAbsolute);
            Uri pausar_inhabilitado = new Uri("/imagenes/pausa_in.png", UriKind.RelativeOrAbsolute);
            Uri reanudar_inhabilitado = new Uri("/imagenes/reanudar_in.png", UriKind.RelativeOrAbsolute);
            Uri terminar_inhabilitado = new Uri("/imagenes/terminar_in.png", UriKind.RelativeOrAbsolute);

            if (abiertos.SelectedIndex > -1)
            {
                //se determina el tamano de la ventana emergente
                #region tamano_de_pop
                datos_solicitud.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                datos_solicitud.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                datos_solicitud.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                datos_solicitud.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                #endregion

                datos_solicitud.IsOpen = true;
                //se evalua el problema seleccionado
                solicitudMaquina item = (solicitudMaquina)abiertos.SelectedItem;

                    // se agregan el problema, maquina, solicitud y la hora a la que se hizo del problema seleccionado
                    problema.Content = item.problema_reportado.ToString();
                    maquina.Content = item.maquina.ToString();
                    solicitud.Content = item.id_solicitud.ToString();
                    hora_reporte.Content = item.hora_reportada.ToString();

                    //se evalua si ya esta abierto
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

                    //se revisa en la tabla de tiempos por mecanico quien abrio el problema 
                    string sql = "select top 1 mecanico, usuarios.nombre from tiempos_por_mecanico left join ingenieria.dbo.usuarios on usuarios.codigo = mecanico where (mecanico<>'----' or mecanico is not null) and num_solicitud=" + item.id_solicitud + "order by hora desc";
                    cn.Open();
                        SqlCommand cm = new SqlCommand(sql, cn);
                        SqlDataReader dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            mecanico.Content = Convert.ToString(dr["nombre"] is DBNull ? "----" : dr["nombre"]);
                            codigo_mecanico.Content = Convert.ToString(dr["mecanico"] is DBNull ? "----" : dr["mecanico"]);
                        };
                        dr.Close();

                        //se revisa si existen pausas abiertas de la solicitud (eso se ve en la tabla pausas: 1 es inicio de pausa un -1 es un cierre de pausa)
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
                        estado.Content = "Sin Abrir";
                        mecanico.Content = "----";
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
        }
        #endregion
        #region botones_pop_principal
        private void iniciar_Click(object sender, RoutedEventArgs e)
        {

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
            codigo_mec_re.Text = "";
            id_3.Content = solicitud.Content.ToString();
            reanudar_solicitud.IsOpen = true;
            datos_solicitud.IsOpen = false;
        }
        private void terminar_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "update solicitudes set hora_cierre='" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "' where id_solicitud= '" + solicitud.Content.ToString() + "'";
            string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + solicitud.Content.ToString() + "', '" + codigo_mecanico.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") +"', '1')";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            cm.ExecuteNonQuery();
            cm2.ExecuteNonQuery();
            cn.Close();
            datos_solicitud.IsOpen = false;
        }
        #endregion
        #region botones_por_pop_up
        private void motivo_de_pausa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (motivo_de_pausa.SelectedIndex >= 0)
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
            if (string.IsNullOrEmpty(codigo_mec_re.Text))
            {

            }
            else
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "insert into pausas (num_solicitud, hora, tipo) values('" + id_3.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
                string sql2 = "insert into tiempos_por_mecanico (num_solicitud, mecanico, hora, tipo) values( '" + id_3.Content.ToString() + "', '" + codigo_mec_re.Text.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
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
        #endregion
        #endregion
    }
}

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
using System.Data.SqlClient;
using System.Configuration;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace Production_control_1._0
{
    /// <summary>
    /// Interaction logic for preventivo.xaml
    /// </summary>
    public partial class preventivo : Page
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
        public preventivo()
        {
            InitializeComponent();
            //se cargan los datos de las listas de modulos y de problemas
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select modulo from modulos";
            string sql2 = "select codigo from mecanicos order by codigo";
            string sql3 = "select falla from defectos_totales where categoria='MANTENIMIENTO PREVENTIVO'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulo.Items.Add(dr["modulo"].ToString());
            };
            dr.Close();
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                codigo.Items.Add(dr2["codigo"].ToString());
            };
            dr2.Close();
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                accion.Items.Add(dr3["falla"].ToString());
            };
            dr3.Close();
            cn.Close();

            //se habilita o ibhabilita el boton
            habilitar_boton();

            //actualizar datos de los modulos en revision
            datos_llamados();
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


        #endregion

        #region control_general_del_programa

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            inicio inicio = new inicio();
            this.NavigationService.Navigate(inicio);

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
        #endregion

        #region control_de_formulario

        private void modulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
            string sql = "insert into solicitudes (modulo, maquina, problema_reportado, hora_reportada, hora_apertura, corresponde)  values('" + modulo.SelectedItem.ToString() + "', 'manto', '" + accion.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '" + DateTime.Now.ToString() + "', 'MANTENIMIENTO')";
            string sql2 = "insert into actualizacion(evento) values(1)";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            cm.ExecuteNonQuery();
            cm2.ExecuteNonQuery();
        }

        #endregion

        #region actualizacion_planta()

        public void monitorear_tabla()
        {
            try
            {
            var connectionString = cs;
            var tableName = "actualizacion";
            var tableDependency = new SqlTableDependency<item_actualizacion>(connectionString, tableName);
            tableDependency.OnChanged += OnNotificationReceived;
            tableDependency.Start();
            }
            catch
            {

            }
        }

        private void OnNotificationReceived(object sender, RecordChangedEventArgs<item_actualizacion> e)
        {
            this.Dispatcher.Invoke(() =>
            {
                datos_llamados();
            });
        }

        private void datos_llamados()
        {
            try
            {
                abiertos.Items.Clear();
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select id_solicitud, modulo, maquina, hora_reportada, hora_apertura, hora_cierre, problema_reportado, corresponde  from solicitudes where maquina='manto' and hora_cierre is null";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    abiertos.Items.Add(new item_solicitud { id_solicitud = Convert.ToInt32(dr["id_solicitud"]), modulo = dr["modulo"].ToString(), maquina = dr["maquina"].ToString(), hora_reportada = dr["hora_reportada"].ToString(), hora_apertura = dr["hora_apertura"].ToString(), hora_cierre = dr["hora_cierre"].ToString(), problema_reportado = dr["problema_reportado"].ToString(), corresponde = dr["corresponde"].ToString()});
                };
                dr.Close();

                monitorear_tabla();
            }
            catch
            {
                monitorear_tabla();
            }
        }


        #endregion

        #region pop_reportar_problema

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
                foreach (item_solicitud item in abiertos.SelectedItems)
                {
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
                        string sql = "select top 1 mecanico, mecanicos.nombre from tiempos_por_mecanico left join mecanicos on mecanicos.codigo = mecanico where num_solicitud=" + item.id_solicitud + "order by hora desc";
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
        }

        private void iniciar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void pausar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void reanudar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void terminar_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using JazzCCO._0.clases;

namespace JazzCCO._0.pantallasMantenimiento
{
    public partial class formularioTPM : Page
    {
        #region varibalesConexion
        public SqlConnection cnMantenimiento = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        #region datosIniciales
        public formularioTPM()
        {
            InitializeComponent();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;

            //llenar lista de maquinas
            cnMantenimiento.Open();
            sql = "select codigo from inventario_maquinas";
            cm =new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxMaquina.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();
            cnMantenimiento.Close();

            //llenar lista de mecanicos
            cnIngenieria.Open();
            sql = "select codigo from usuarios where cargo='MECANICO' or cargo='ELECTRICISTA'";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxMecanico.Items.Add(dr["codigo"].ToString());
                codigo_mec_re.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();
            cnIngenieria.Close();


            habilitarBotonEnviar();

            consultarReportes();
        }
        #endregion
        #region tamanos_de_letra_/_tipo_de_texto

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height/ tmp.FontFamily.LineSpacing;
        }

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void letra_pop_cerrar(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.5 / tmp.FontFamily.LineSpacing;
        }
        #endregion
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = Brushes.White;
            this.NavigationService.GoBack();
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
        #region datosFomrularioIniciar
        private void textBoxMaquina_TextChanged(object sender, TextChangedEventArgs e)
        {
            //se limpian los items cargados en la lista de maquinas
            listBoxMaquina.Items.Clear();
            string sql = "select codigo from inventario_maquinas where codigo like '%" + textBoxMaquina.Text.ToString() + "%'";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxMaquina.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();
            cnMantenimiento.Close();
        }

        private void textBoxMecanico_TextChanged(object sender, TextChangedEventArgs e)
        {
            //se limpian los items cargados en la lista de mecanicos
            listBoxMecanico.Items.Clear();
            string sql = "select codigo from usuarios where cargo='MECANICO' and codigo like '%" + textBoxMecanico.Text.ToString() + "%'";
            cnIngenieria.Open();
            SqlCommand cm = new SqlCommand(sql, cnIngenieria);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxMecanico.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();
            cnIngenieria.Close();

        }

        private void listBoxMaquina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitarBotonEnviar();
        }

        private void listBoxMecanico_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitarBotonEnviar();
        }

        private void buttonEnviarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            string tipo = "PREVENTIVO";
            if (radioButtonCorrectivo.IsChecked == true)
            {
                tipo = "CORRECTIVO";
            }
            else
            {
                tipo = "PREVENTIVO";
            }
            string sql = "insert into solicitudesTPM(maquina, tipo, problema, fechaInicio) values('" + listBoxMaquina.SelectedItem.ToString() + "', '" + tipo + "', '" + textBoxProblema.Text + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') SELECT SCOPE_IDENTITY()";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
            SqlDataReader dr = cm.ExecuteReader();
            dr.Read();
            int id_ingresado = Convert.ToInt32(dr[0]);
            dr.Close();
            sql= "insert into tiemposPorMecanicoTPM (num_solicitud, mecanico, hora, tipo) values( '" + id_ingresado+ "', '" + listBoxMecanico.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
            cm = new SqlCommand(sql, cnMantenimiento);
            cm.ExecuteNonQuery();
            cnMantenimiento.Close();

            MessageBox.Show("Reporte Ingresado");
            textBoxProblema.Text = "";
            listBoxMaquina.SelectedIndex = -1;
            listBoxMecanico.SelectedIndex = -1;

            consultarReportes();
        }

        private void buttonActualizarLista_Click(object sender, RoutedEventArgs e)
        {
            consultarReportes();
        }

        #endregion
        #region calculos_generales
        private void habilitarBotonEnviar()
        {
            Uri habilitado = new Uri("/imagenes/enviar.png", UriKind.RelativeOrAbsolute);
            Uri inhabilitado = new Uri("/imagenes/enviar_in.png", UriKind.RelativeOrAbsolute);

            if (listBoxMaquina.SelectedIndex > -1 & listBoxMecanico.SelectedIndex > -1)
            {
                buttonEnviarSolicitud.IsEnabled = true;
                imagenButton.Source = new BitmapImage(habilitado);
            }
            else
            {
                buttonEnviarSolicitud.IsEnabled = false;
                imagenButton.Source = new BitmapImage(inhabilitado);
            }
        }
        private void consultarReportes()
        {
            List<solicitudTPM> maquinasEnManto = new List<solicitudTPM>();
            string sql = "select id, maquina, tipo, problema, fechaInicio, minutosPausa, mecanico from resumenTiemposTPM where fechaFin is null";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                string direccionImagen = "/imagenes/pausa.png";
                if (Convert.ToInt32(dr["minutosPausa"]) < 0)
                {
                    direccionImagen = "/imagenes/reanudar.png";
                }

                maquinasEnManto.Add(new solicitudTPM { id = Convert.ToInt32(dr["id"]), maquina = dr["maquina"].ToString(), problema = dr["problema"].ToString(), fechaInicio = Convert.ToDateTime(dr["fechaInicio"]).ToString("yyyy-MM-dd HH:mm:ss"), imagen = direccionImagen, codigoMecanico= Convert.ToInt32(dr["mecanico"]) });
            };
            dr.Close();
            cnMantenimiento.Close();

            listViewPendientes.ItemsSource = maquinasEnManto;

        }
        #endregion
        #region mostrarPop
        private void listViewPendientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

            if (listViewPendientes.SelectedIndex > -1)
            {
                solicitudTPM problemaSeleccionado = (solicitudTPM)listViewPendientes.SelectedItem;
                //se determina el tamano de la ventana emergente
                #region tamano_de_pop
                popEstadoSoli.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                popEstadoSoli.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                popEstadoSoli.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                popEstadoSoli.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                #endregion
                //se abre la ventana emergente
                popEstadoSoli.IsOpen = true;
                //se evalua el problema seleccionado
                problema.Content = problemaSeleccionado.problema;
                maquina.Content = problemaSeleccionado.maquina;
                solicitud.Content = problemaSeleccionado.id;
                horaInicio.Content = problemaSeleccionado.fechaInicio;

                //se revisa en la tabla de tiempos por mecanico quien abrio el problema 
                string sql = "select top 1 mecanico, a.nombre from tiemposPorMecanicoTPM left join (select codigo, nombre from ingenieria.dbo.usuarios where cargo='MECANICO') a on a.codigo = mecanico where num_solicitud=" + problemaSeleccionado.id + " order by hora desc";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    mecanico.Content = Convert.ToString(dr["nombre"] is DBNull ? "----" : dr["nombre"]);
                    codigo_mecanico.Content = Convert.ToString(dr["mecanico"] is DBNull ? "----" : dr["mecanico"]);
                };
                dr.Close();

                //se evalua si esta pausado
                if (problemaSeleccionado.imagen== "/imagenes/reanudar.png")
                {
                    pausar.IsEnabled = false;
                    reanudar.IsEnabled = true;
                    terminar.IsEnabled = false;
                    estado.Content = "Pausado";

                    //se cargan las imagenes de acuerdo a si estan o no habilitados
                    img_pausar.Source = new BitmapImage(pausar_inhabilitado);
                    img_reanudar.Source = new BitmapImage(reanudar_habilitado);
                    img_terminar.Source = new BitmapImage(terminar_inhabilitado);
                }
                else
                {
                    pausar.IsEnabled = true;
                    reanudar.IsEnabled = false;
                    terminar.IsEnabled = true;
                    estado.Content = "En Proceso";

                    //se cargan las imagenes de acuerdo a si estan o no habilitados
                    img_pausar.Source = new BitmapImage(pausar_habilitado);
                    img_reanudar.Source = new BitmapImage(reanudar_inhabilitado);
                    img_terminar.Source = new BitmapImage(terminar_habilitado);
                }
            }
        }
        private void pausar_Click(object sender, RoutedEventArgs e)
        {
            string sql = "insert into pausasTPM(IdSolicitud, hora, accion) values('"+ solicitud.Content.ToString() +"', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '-1')";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
            cm.ExecuteNonQuery();
            sql= "insert into tiemposPorMecanicoTPM (num_solicitud, mecanico, hora, tipo) values( '" + solicitud.Content + "', '" + codigo_mecanico.Content + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
            cm = new SqlCommand(sql, cnMantenimiento);
            cm.ExecuteNonQuery();
            cnMantenimiento.Close();
            popEstadoSoli.IsOpen = false;
            consultarReportes();
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
            codigo_mec_re.SelectedIndex = -1;
            id_3.Content = solicitud.Content.ToString();
            reanudar_solicitud.IsOpen = true;
            popEstadoSoli.IsOpen = false;
        }
        private void terminar_Click(object sender, RoutedEventArgs e)
        {
            reporteFinalTPM reporteFinalTPM = new reporteFinalTPM(Convert.ToInt32(solicitud.Content), Convert.ToInt32(codigo_mecanico.Content));
            this.NavigationService.Navigate(reporteFinalTPM);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popEstadoSoli.IsOpen = false;
            consultarReportes();            
        }
        #endregion
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            reanudar_solicitud.IsOpen= false;
            consultarReportes();
        }
        private void btn_reanudar_Click(object sender, RoutedEventArgs e)
        {
            if (codigo_mec_re.SelectedIndex > -1)
            {
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "insert into pausasTPM(IdSolicitud, hora, accion) values('" + solicitud.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '1')";
                string sql2 = "insert into tiemposPorMecanicoTPM (num_solicitud, mecanico, hora, tipo) values( '" + id_3.Content.ToString() + "', '" + codigo_mec_re.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '-1')";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn.Close();
                codigo_mec_re.Text = "";
                reanudar_solicitud.IsOpen = false;
                consultarReportes();
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window ventana = GetDependencyObjectFromVisualTree(this, typeof(Window)) as Window;
            ventana.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF1B1A2C");
        }
    }
}

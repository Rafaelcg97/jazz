using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasMantenimiento
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
            sql = "select codigo from usuarios where cargo='MECANICO'";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxMecanico.Items.Add(dr["codigo"].ToString());
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
            string sql = "insert into solicitudesTPM(maquina, tipo, problema, mecanico, fechaInicio) values('" + listBoxMaquina.SelectedItem.ToString() + "', '" + tipo + "', '" + textBoxProblema.Text + "', '" + listBoxMecanico.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
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
            string sql = "select id, maquina, tipo, problema, codigoMecanico, nombreMecanico, fechaInicio, minutosPausa from resumenTiemposTPM where fechaFin is null";
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

                maquinasEnManto.Add(new solicitudTPM { id = Convert.ToInt32(dr["id"]), maquina = dr["maquina"].ToString(), problema = dr["problema"].ToString(), fechaInicio = Convert.ToDateTime(dr["fechaInicio"]).ToString("yyyy-MM-dd HH:mm:ss"), nombreMecanico = dr["nombreMecanico"].ToString(), codigoMecanico=Convert.ToInt32(dr["codigoMecanico"]), imagen = direccionImagen });
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
                mecanico.Content = problemaSeleccionado.nombreMecanico;
                codigo_mecanico.Content = problemaSeleccionado.codigoMecanico;
                horaInicio.Content = problemaSeleccionado.fechaInicio;
                
                //se evalua si ya esta abierto
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
            cnMantenimiento.Close();
            popEstadoSoli.IsOpen = false;
            consultarReportes();
        }

        private void reanudar_Click(object sender, RoutedEventArgs e)
        {
            string sql = "insert into pausasTPM(IdSolicitud, hora, accion) values('" + solicitud.Content.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "', '1')";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
            cm.ExecuteNonQuery();
            cnMantenimiento.Close();
            popEstadoSoli.IsOpen = false;
            consultarReportes();
        }

        private void terminar_Click(object sender, RoutedEventArgs e)
        {
            reporteFinalTPM reporteFinalTPM = new reporteFinalTPM(Convert.ToInt32(solicitud.Content));
            this.NavigationService.Navigate(reporteFinalTPM);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            popEstadoSoli.IsOpen = false;
            consultarReportes();            
        }

        #endregion
    }
}

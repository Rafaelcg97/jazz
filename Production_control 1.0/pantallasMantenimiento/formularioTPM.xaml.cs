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
        public SqlConnection cnMantenimiento = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);

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

        #endregion



        private void buttonActualizarLista_Click(object sender, RoutedEventArgs e)
        {
            consultarReportes();
        }

        private void consultarReportes()
        {
            List<solicitudMaquina> maquinasEnManto = new List<solicitudMaquina>();
            string sql = "select id, maquina, problema, mecanico, fechaInicio from solicitudesTPM where fechaFin is null";
            cnMantenimiento.Open();
            SqlCommand cm = new SqlCommand(sql, cnMantenimiento);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                maquinasEnManto.Add(new solicitudMaquina { id_solicitud = Convert.ToInt32(dr["id"]), maquina = dr["maquina"].ToString(), problema_reportado = dr["problema"].ToString(), hora_reportada = Convert.ToDateTime(dr["fechaInicio"]).ToString("yyyy-MM-dd HH:mm:ss"), mecanico=dr["mecanico"].ToString() });
            };
            dr.Close();
            cnMantenimiento.Close();

            listViewPendientes.ItemsSource = maquinasEnManto;
            
        }
    }
}

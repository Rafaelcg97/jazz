using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Configuration;
using System.Data.SqlClient;
using Production_control_1._0.clases;

namespace Production_control_1._0
{
    public partial class sam : Page
    {
        #region variablesDeConexion
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        #region datosIniciales
        public sam()
        {
            InitializeComponent();
            string sql = "";
            SqlCommand cm;
            SqlDataReader dr;

            // se bloquean listBox que no pueden seleccionarse
            listBoxCliente.IsEnabled = false;
            listBoxEmpaque.IsEnabled = false;
            listBoxTipo.IsEnabled = false;
            // se carga la imagen inicial
            Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
            imageEstilo.Source = new BitmapImage(fileUri);

            // se cargan todas las tempordas 
            sql = "select temporada from operaciones group by temporada";
            cnIngenieria.Open();
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
               listBoxTemporada.Items.Add(dr["temporada"].ToString());
            }
            dr.Close();

            // se cargan todas los clientes
            sql = "select cliente from operaciones group by cliente";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxCliente.Items.Add(dr["cliente"].ToString());
            }
            dr.Close();

            // se cargan todas los tipos
            sql = "select tipo from operaciones group by tipo";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxTipo.Items.Add(dr["tipo"].ToString());
            }
            dr.Close();

            // se cargan todas los empaques
            sql = "select tipo_empaque from empaques group by tipo_empaque";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
               listBoxEmpaque.Items.Add(dr["tipo_empaque"].ToString());
            }
            dr.Close();

            cnIngenieria.Close();
        }
        #endregion
        #region control_general_del programa
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
        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        #endregion
        #region formularioConsultSam
        private void listBoxTemporada_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            #region limpiarListBox
            listBoxCliente.Items.Clear();
            ListViewOperaciones.Items.Clear();
            listBoxCliente.IsEnabled = true;
            listBoxTipo.SelectedIndex = -1;
            listBoxTipo.IsEnabled = false;
            listBoxEmpaque.SelectedIndex = -1;
            listBoxEmpaque.IsEnabled = false;
            ListViewEstilos.SelectedIndex = -1;
            ListViewEstilos.IsEnabled = false;
            textBoxBuscar.IsEnabled = false;
            ListViewEstilos.Items.Clear();
            #endregion
            #region limpiarTextBox
            labelEstado.Content = "----";
            labelEtapa.Content = "----";
            labelMuestra.Content = "----";
            labelSamCostura.Content = "0";
            labelSamEmpaque.Content = "0";
            labelSamTota.Content = "0";
            textBlockElementosEmpaques.Text = "----";
            textBoxBuscar.Text = "";
            #endregion
            #region limpiarImagen
            // se carga la imagen inicial
            Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
            imageEstilo.Source = new BitmapImage(fileUri);
            #endregion
            string sql = "";
            SqlCommand cm;
            SqlDataReader dr;
            // se cargan todas los clientes
            cnIngenieria.Open();
            sql = "select cliente from operaciones where temporada='"+ listBoxTemporada.SelectedItem.ToString() +"' group by cliente";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxCliente.Items.Add(dr["cliente"].ToString());
            }
            dr.Close();
            cnIngenieria.Close();
        }
        private void listBoxCliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(listBoxTemporada.SelectedIndex>-1 && listBoxCliente.SelectedIndex > -1)
            {
                #region limpiarListBox
                ListViewOperaciones.Items.Clear();
                listBoxTipo.Items.Clear();
                listBoxTipo.IsEnabled = true;
                listBoxEmpaque.SelectedIndex = -1;
                listBoxEmpaque.IsEnabled = false;
                ListViewEstilos.SelectedIndex = -1;
                ListViewEstilos.IsEnabled = false;
                textBoxBuscar.IsEnabled = false;
                ListViewEstilos.Items.Clear();
                #endregion
                #region limpiarTextBox
                labelEstado.Content = "----";
                labelEtapa.Content = "----";
                labelMuestra.Content = "----";
                labelSamCostura.Content = "0";
                labelSamEmpaque.Content = "0";
                labelSamTota.Content = "0";
                textBlockElementosEmpaques.Text = "----";
                textBoxBuscar.Text = "";
                #endregion
                #region limpiarImagen
                // se carga la imagen inicial
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                imageEstilo.Source = new BitmapImage(fileUri);
                #endregion
                string sql = "";
                SqlCommand cm;
                SqlDataReader dr;
                // se cargan todas los tipos de ese cliente temporada
                cnIngenieria.Open();
                sql = "select tipo from operaciones where temporada='" + listBoxTemporada.SelectedItem.ToString() + "' and cliente='" + listBoxCliente.SelectedItem.ToString() + "' group by tipo";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listBoxTipo.Items.Add(dr["tipo"].ToString());
                }
                dr.Close();
                cnIngenieria.Close();
            }
        }
        private void listBoxTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxTemporada.SelectedIndex > -1 && listBoxCliente.SelectedIndex > -1 && listBoxTipo.SelectedIndex>-1)
            {
                #region limpiarListBox
                ListViewOperaciones.Items.Clear();
                listBoxEmpaque.Items.Clear();
                listBoxEmpaque.IsEnabled = true;
                ListViewEstilos.Items.Clear();
                ListViewEstilos.IsEnabled = true;
                textBoxBuscar.IsEnabled = true;
                #endregion
                #region limpiarTextBox
                labelEstado.Content = "----";
                labelEtapa.Content = "----";
                labelMuestra.Content = "----";
                labelSamCostura.Content = "0";
                labelSamEmpaque.Content = "0";
                labelSamTota.Content = "0";
                textBlockElementosEmpaques.Text = "----";
                textBoxBuscar.Text = "";
                #endregion
                #region limpiarImagen
                // se carga la imagen inicial
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                imageEstilo.Source = new BitmapImage(fileUri);
                #endregion
                string sql = "";
                SqlCommand cm;
                SqlDataReader dr;
                // se cargan todos los empaques de ese cliente temporada tipo
                cnIngenieria.Open();
                sql = "select tipo_empaque from empaques where tipo='"+ listBoxTipo.SelectedItem.ToString()+"' and cliente='"+listBoxCliente.SelectedItem.ToString()+"' and temporada='"+listBoxTemporada.SelectedItem.ToString()+"'"; ;
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listBoxEmpaque.Items.Add(dr["tipo_empaque"].ToString());
                }
                dr.Close();
                cnIngenieria.Close();

                // se cargan todos los estilos de ese cliente temporada tipo
                cnIngenieria.Open();
                sql = "select estilo from operaciones where temporada='" + listBoxTemporada.SelectedItem.ToString() + "' and cliente='" + listBoxCliente.SelectedItem.ToString() + "' and tipo='" + listBoxTipo.SelectedItem.ToString() + "' group by estilo";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    ListViewEstilos.Items.Add(new estilo { nombre = dr["estilo"].ToString() });
                }
                dr.Close();
                cnIngenieria.Close();
            }
        }
        private void listBoxEmpaque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxTemporada.SelectedIndex > -1 && listBoxCliente.SelectedIndex > -1 && listBoxTipo.SelectedIndex > -1 & listBoxEmpaque.SelectedIndex>-1)
            {
                string sql = "";
                SqlCommand cm;
                SqlDataReader dr;
                // se cargan todas los clientes
                cnIngenieria.Open();
                sql = "select tipo_empaque, sam, descripcion from empaques where tipo='" + listBoxTipo.SelectedItem.ToString() + "' and cliente='" + listBoxCliente.SelectedItem.ToString() + "' and temporada='" + listBoxTemporada.SelectedItem.ToString() + "' and tipo_empaque='" + listBoxEmpaque.SelectedItem.ToString() + "'";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                dr.Read();
                labelSamEmpaque.Content = dr["sam"].ToString();
                labelSamTota.Content = (Convert.ToDouble(labelSamCostura.Content) + Convert.ToDouble(dr["sam"]));
                textBlockElementosEmpaques.Text = dr["descripcion"].ToString();
                dr.Close();
                cnIngenieria.Close();
            }

        }
        private void textBoxBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (listBoxTemporada.SelectedIndex > -1 && listBoxCliente.SelectedIndex > -1 && listBoxTipo.SelectedIndex > -1)
            {
                #region limpiarListBox
                ListViewOperaciones.Items.Clear();
                ListViewEstilos.Items.Clear();
                ListViewEstilos.IsEnabled = true;
                #endregion
                #region limpiarTextBox
                labelEstado.Content = "----";
                labelEtapa.Content = "----";
                labelMuestra.Content = "----";
                labelSamCostura.Content = "0";
                labelSamTota.Content = labelSamEmpaque.Content.ToString();
                #endregion
                #region limpiarImagen
                // se carga la imagen inicial
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                imageEstilo.Source = new BitmapImage(fileUri);
                #endregion
                string sql = "";
                SqlCommand cm;
                SqlDataReader dr;
                // se cargan todos los estilos de ese cliente temporada tipo
                cnIngenieria.Open();
                sql = "select estilo from operaciones where temporada='" + listBoxTemporada.SelectedItem.ToString() + "' and cliente='" + listBoxCliente.SelectedItem.ToString() + "' and tipo='" + listBoxTipo.SelectedItem.ToString() + "' and estilo like '"+textBoxBuscar.Text +"%' group by estilo";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    ListViewEstilos.Items.Add(new estilo { nombre = dr["estilo"].ToString() });
                }
                dr.Close();
                cnIngenieria.Close();
            }

        }
        private void ListViewEstilos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxTemporada.SelectedIndex > -1 && listBoxCliente.SelectedIndex > -1 && listBoxTipo.SelectedIndex > -1 && ListViewEstilos.SelectedIndex>-1)
            {
                #region limpiarListBox
                ListViewOperaciones.Items.Clear();
                #endregion
                string sql = "";
                estilo item = (estilo)ListViewEstilos.SelectedItem;
                string _estilo = item.nombre;
                SqlCommand cm;
                SqlDataReader dr;
                // se consulta los datos de estilo
                cnIngenieria.Open();
                sql = "select sum(sam) as sam, max(etapa) as etapa, max(muestra) as muestra, max(estatus) as estatus from operaciones where temporada='" + listBoxTemporada.SelectedItem.ToString() + "' and cliente='" + listBoxCliente.SelectedItem.ToString() + "' and tipo='" + listBoxTipo.SelectedItem.ToString() + "' and estilo='" +_estilo+ "' group by estilo";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                dr.Read();
                labelEstado.Content = dr["estatus"].ToString();
                labelEtapa.Content = dr["etapa"].ToString();
                labelMuestra.Content = dr["muestra"].ToString();
                labelSamCostura.Content = dr["sam"].ToString();
                labelSamTota.Content = Math.Round(Convert.ToDouble(labelSamEmpaque.Content) + Convert.ToDouble(dr["sam"]),4);
                dr.Close();

                //se llena la lista de Operaciones
                sql = "select titulo, maquina, sam from operaciones where temporada='" + listBoxTemporada.SelectedItem.ToString() + "' and cliente='" + listBoxCliente.SelectedItem.ToString() + "' and tipo='" + listBoxTipo.SelectedItem.ToString() + "' and estilo='" + _estilo + "' and sam is not null";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    ListViewOperaciones.Items.Add(new Operacion {tituloOperacion=dr["titulo"].ToString(), ajusteMaquina=dr["maquina"].ToString(), samOperacion=Convert.ToDouble(dr["sam"])});
                }
                dr.Close();
                cnIngenieria.Close();
                #region limpiarImagen
                // se carga la imagen desde archivo
                try
                {
                    Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + listBoxTemporada.SelectedItem.ToString() + "/" + _estilo + ".jpg");
                    imageEstilo.Source = new BitmapImage(fileUri);
                }

                // si no encuentra la imagen del estilo carga la imagen inicial
                catch
                {
                    Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                    imageEstilo.Source = new BitmapImage(fileUri);
                }
                #endregion
            }
        }
        #endregion
        #region crearNuevoBalance
        private void buttonCrearPlantilla_Click(object sender, RoutedEventArgs e)
        {
            if (ListViewEstilos.SelectedIndex< 0 || listBoxTemporada.SelectedIndex < 0 || listBoxEmpaque.SelectedIndex < 0)
            {
                MessageBox.Show("Datos seleccionados incompletos");
            }

            else
            {
                estilo item = (estilo)ListViewEstilos.SelectedItem;
                string _estilo = item.nombre;
                //datos a pasar para generar nuevo balance
                clases.balance nuevoBalance = new clases.balance();
                nuevoBalance.nombre = _estilo;
                nuevoBalance.temporada = listBoxTemporada.SelectedItem.ToString();
                nuevoBalance.samEmpaque = Convert.ToDouble(labelSamEmpaque.Content);
                nuevoBalance.samOperarcional = Convert.ToDouble(labelSamCostura.Content);
                nuevoBalance.sam = Convert.ToDouble(labelSamTota.Content);
                nuevoBalance.version = 1;
                nuevoBalance.tipo = "nuevo";
                nuevoBalance.nombreEmpaque = listBoxEmpaque.SelectedItem.ToString();
                nuevoBalance.fechaCreacion = DateTime.Now;
                this.NavigationService.Navigate(new balance(nuevoBalance));
            }
        }
        #endregion
    }
}





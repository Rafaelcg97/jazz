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
            this.NavigationService.Navigate(new PagePrincipal());
        }
        #endregion

        private void nueva_plantilla_Click(object sender, RoutedEventArgs e)
        {
            //if (estilo.SelectedIndex < 0 || temporada.SelectedIndex < 0 || empaque.SelectedIndex < 0)
            //{
            //    MessageBox.Show("Datos seleccionados incompletos");
            //}

            //else
            //{
            //    //datos a pasar para generar nuevo balance
            //    clases.balance nuevoBalance = new clases.balance();
            //    nuevoBalance.nombre = estilo.SelectedItem.ToString();
            //    nuevoBalance.temporada = temporada.SelectedItem.ToString();
            //    nuevoBalance.samEmpaque = Convert.ToDouble(empaque_s.Content);
            //    nuevoBalance.samOperarcional = Convert.ToDouble(sam_op.Content);
            //    nuevoBalance.sam= Convert.ToDouble(sam_op.Content)+ Convert.ToDouble(empaque_s.Content); ;
            //    nuevoBalance.version = 1;
            //    nuevoBalance.tipo = "nuevo";
            //    nuevoBalance.nombreEmpaque= empaque.SelectedItem.ToString();
            //    nuevoBalance.fechaCreacion = DateTime.Now;
            //    this.NavigationService.Navigate(new balance(nuevoBalance));
                
            //}
        }

        private void listBoxTemporada_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            listBoxCliente.Items.Clear();
            listBoxCliente.IsEnabled = true;
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
            listBoxTipo.Items.Clear();
            listBoxTipo.IsEnabled = true;
            string sql = "";
            SqlCommand cm;
            SqlDataReader dr;
            // se cargan todas los clientes
            cnIngenieria.Open();
            sql = "select tipo from operaciones where temporada='" + listBoxTemporada.SelectedItem.ToString() + "' and cliente='"+ listBoxCliente.SelectedItem.ToString()+ "' group by tipo";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxCliente.Items.Add(dr["tipo"].ToString());
            }
            dr.Close();
            cnIngenieria.Close();

        }
    }
}





using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Configuration;
using System.Data.SqlClient;

namespace Production_control_1._0
{
    public partial class sam : Page
    {
        public sam()
        {
            InitializeComponent();

            // se ocultan todos los circulos rojos que indican los elementos seleccionados
            s1.Visibility = Visibility.Hidden;
            s2.Visibility = Visibility.Hidden;
            s3.Visibility = Visibility.Hidden;
            s4.Visibility = Visibility.Hidden;
            s5.Visibility = Visibility.Hidden;

            // se bloquean los filtros que no pueden seleccionarse
            cliente.IsEnabled = false;
            tipo.IsEnabled = false;
            empaque.IsEnabled = false;
            buscar.IsEnabled = false;
            estilo.IsEnabled = false;

            // se carga la imagen inicial
            Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
            foto.Source = new BitmapImage(fileUri);

            // se declaran las variables de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select temporada from operaciones group by temporada";
            string sql2 = "select cliente from operaciones group by cliente";
            string sql3 = "select tipo from operaciones group by tipo";
            string sql4 = "select tipo_empaque from empaques group by tipo_empaque";
            cn.Open();

            // se cargan todas las tempordas 
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                temporada.Items.Add(dr["temporada"].ToString());
            }
            dr.Close();

            // se cargan todos los clientes
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                cliente.Items.Add(dr2["cliente"].ToString());
            }
            dr2.Close();

            // se cargan todos los tipos
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                tipo.Items.Add(dr3["tipo"].ToString());
            }
            dr3.Close();

            // se cargan todos los empaques
            SqlCommand cm4 = new SqlCommand(sql4, cn);
            SqlDataReader dr4 = cm4.ExecuteReader();
            while (dr4.Read())
            {
                empaque.Items.Add(dr4["tipo_empaque"].ToString());
            }
            dr4.Close();
            cn.Close();

        }


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
        #endregion


        private void temporada_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // habilitar los filtros que pueden seleccionarse ya se ha escofigo la temporada del estilo
            cliente.IsEnabled = true;
            tipo.IsEnabled = false;
            empaque.IsEnabled = false;
            buscar.IsEnabled = true;
            estilo.IsEnabled = true;

            // se limpian los demas filtros por si ya se habia cargado algo
            cliente.Items.Clear();
            tipo.Items.Clear();
            estilo.Items.Clear();
            buscar.Text = "";
            sam_op.Content = "----";
            empaque_s.Content = "----";
            elementos.Text = "----";
            estatus.Content = "----";
            muestra.Content = "----";
            total.Content = "----";

            // se carga la imagen inicial
            Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
            foto.Source = new BitmapImage(fileUri);

            // se declaran las variables de conexion
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select cliente from operaciones where temporada= '" + temporada.SelectedItem.ToString() + "' group by cliente";
            string sql2 = "select tipo from operaciones where temporada= '" + temporada.SelectedItem.ToString() + "' group by tipo";
            string sql3 = "select estilo from operaciones where temporada= '" + temporada.SelectedItem.ToString() + "' group by estilo";
            cn.Open();

            // se cargan los clientes existentes en la temporada seleccionada
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cliente.Items.Add(dr["cliente"].ToString());
            }
            dr.Close();

            // se cargan los tipos existentes en la temporada seleccionada
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                tipo.Items.Add(dr2["tipo"].ToString());
            }
            dr2.Close();

            //se cargan los estilos existentes en la temporada seleccionada
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                estilo.Items.Add(dr3["estilo"].ToString());
            }
            dr3.Close();
            cn.Close();

            // se muestra el circulo rojo de temporada y se ocultan los demas
            s1.Visibility = Visibility.Visible;
            s2.Visibility = Visibility.Hidden;
            s3.Visibility = Visibility.Hidden;
            s4.Visibility = Visibility.Hidden;
            s5.Visibility = Visibility.Hidden;
        }

        private void cliente_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // para evitar error por evento no declarado se revisa si ya se selecciono cliente y temporada
            if (cliente.SelectedIndex == -1 || temporada.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                // habilitar los filtros que pueden seleccionarse cuando se escoge el cliente
                tipo.IsEnabled = true;
                empaque.IsEnabled = false;
                buscar.IsEnabled = true;
                estilo.IsEnabled = true;

                // se limpian los filtros por si ya se habia seleccionado algo
                tipo.Items.Clear();
                estilo.Items.Clear();
                empaque.Items.Clear();
                buscar.Text = "";
                muestra.Content = "----";
                estatus.Content = "----";
                sam_op.Content = "----";
                empaque_s.Content = "----";
                elementos.Text = "----";
                total.Content = "----";

                // se carga la imagen inicial
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                foto.Source = new BitmapImage(fileUri);

                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select tipo from operaciones where cliente= '" + cliente.SelectedItem.ToString() + "' and temporada= '" + temporada.SelectedItem.ToString() + "' group by tipo";
                string sql2 = "select estilo from operaciones where cliente= '" + cliente.SelectedItem.ToString() + "' and temporada= '" + temporada.SelectedItem.ToString() + "' group by estilo";
                string sql3 = "select tipo_empaque from empaques where cliente= '" + cliente.SelectedItem.ToString() + "'";
                cn.Open();


                // se cargan los tipos existentes en la temporada-cliente seleccionado
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tipo.Items.Add(dr["tipo"].ToString());
                }
                dr.Close();

                // se cargan los estilos existentes en la temporada-cliente seleccionado
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    estilo.Items.Add(dr2["estilo"].ToString());
                }
                dr2.Close();

                // se cargan los empaques existentes en el cliente seleccionado
                SqlCommand cm3 = new SqlCommand(sql3, cn);
                SqlDataReader dr3 = cm3.ExecuteReader();
                while (dr3.Read())
                {
                    empaque.Items.Add(dr3["tipo_empaque"].ToString());
                }
                dr3.Close();
                cn.Close();

                // se muestra el circulo rojo de cliente y se ocultan los demas
                s2.Visibility = Visibility.Visible;
                s3.Visibility = Visibility.Hidden;
                s4.Visibility = Visibility.Hidden;
                s5.Visibility = Visibility.Hidden;
            }
        }
    
        private void tipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // para evitar error por evento no declarado se revisa si ya se selecciono tipo cliente y temporada
            if (tipo.SelectedIndex == -1 ||  cliente.SelectedIndex == -1 || temporada.SelectedIndex == -1  )
            {
                return;
            }
            else
            {
                // habilitar los filtros que pueden seleccionarse cuando se escoge el tipo
                empaque.IsEnabled = true;

                // se limpian los filtros por si ya se habia seleccionado algo
                estilo.Items.Clear();
                empaque.Items.Clear();
                buscar.Text = "";
                muestra.Content = "----";
                estatus.Content = "----";
                sam_op.Content = "----";
                empaque_s.Content = "----";
                elementos.Text = "----";
                total.Content = "----";

                // se carga la imagen inicial
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                foto.Source = new BitmapImage(fileUri);

                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select estilo from operaciones where cliente= '" + cliente.SelectedItem.ToString() + "' and temporada= '" + temporada.SelectedItem.ToString() + "' and tipo= '"+ tipo.SelectedItem.ToString()  +"' group by estilo";
                string sql2 = "select tipo_empaque from empaques where cliente= '" + cliente.SelectedItem.ToString() + "' and tipo= '" + tipo.SelectedItem.ToString() +"'";
                cn.Open();

                // se cargan los estilos existentes en temporada-cliente-tipo seleccionado
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    estilo.Items.Add(dr["estilo"].ToString());
                }
                dr.Close();

                // se cargan los empaques existentes en cliente-tipo seleccionado
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    empaque.Items.Add(dr2["tipo_empaque"].ToString());
                }
                dr2.Close();
                cn.Close();

                // se muestra el circulo rojo de tipo y se ocultan los demas
                s3.Visibility = Visibility.Visible;
                s4.Visibility = Visibility.Hidden;
                s5.Visibility = Visibility.Hidden;
            }
        }

        private void empaque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             // para evitar error por evento no declarado se revisa si ya se selecciono empaque
            if (empaque.SelectedIndex == -1)
           {
              return;
           }
           else
           {
                // se declaran las variables de conexion
               SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
               string sql = "select sam, descripcion from empaques where cliente= '" + cliente.SelectedItem.ToString() + "' and tipo= '" + tipo.SelectedItem.ToString() + "' and tipo_empaque= '" + empaque.SelectedItem.ToString() + "' group by sam, descripcion";
               cn.Open();

               // se cargan los sam de empaque y la descripcion de los elementos de empaque   
               SqlCommand cm = new SqlCommand(sql, cn);
               SqlDataReader dr = cm.ExecuteReader();
               while (dr.Read())
               {
               empaque_s.Content = dr["sam"].ToString();
               elementos.Text = dr["descripcion"].ToString();
               }
               cn.Close();

                // se muestra el circulo rojo de empaque
                s4.Visibility = Visibility.Visible;
           }
        }
    
        private void estilo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //limpiar la lista de operaciones
            operaciones.Items.Clear();

            // para evitar error por evento no declarado se pone condicional de que si no existe seleccion en el estilo no haga nada
            if( temporada.SelectedIndex==-1 || estilo.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                // se declaran las variables de conexion 
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select nombre, titulo, sam from operaciones where temporada= '" + temporada.SelectedItem.ToString() + "' and estilo= '" + estilo.SelectedItem.ToString() + "' and sam is not null";
                string sql2 = "select sum(sam) as sam, max(estatus) as estatus, max(muestra) as muestra from operaciones where estilo ='" + estilo.SelectedItem.ToString() + "' and temporada= '" + temporada.SelectedItem.ToString() + "'";
                cn.Open();

                // se llenan la lista de operaciones con los datos de la consulta
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    operaciones.Items.Add(new _item() { nombre = dr["nombre"].ToString(), titulo = dr["titulo"].ToString(), sam = Convert.ToDecimal(dr["sam"]) });
                }
                dr.Close();

                // se llenan los datos de sam del estilo, estatus y muestra 
                SqlCommand cm2 = new SqlCommand(sql2, cn);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    sam_op.Content = dr2["sam"].ToString();
                    estatus.Content = dr2["estatus"].ToString();
                    muestra.Content = dr2["muestra"].ToString();
                }
                dr2.Close();
                cn.Close();

                // se carga la imagen desde archivo
                try
                {
                Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + temporada.SelectedItem.ToString() + "/" + estilo.SelectedItem.ToString() + ".jpg");
                foto.Source = new BitmapImage(fileUri);
                 }

                // si no encuentra la imagen del estilo carga la imagen inicial
                catch
                {
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                foto.Source = new BitmapImage(fileUri);
                }

                // hace visible el circulo rojo que indica que hay un estilo seleccionado 
                 s5.Visibility = Visibility.Visible;

                // se hace la suma de los sam si ya hay empaque seleccionado
                if (empaque.SelectedIndex == -1)
                {
                    empaque_s.Content = "----";
                    total.Content = "----";
                    elementos.Text = "----";
                }
                else
                {
                    total.Content = Convert.ToDecimal(sam_op.Content.ToString()) + Convert.ToDecimal(empaque_s.Content.ToString());
                }
            }
        }

        private void buscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // para evitar error por evento no declarado se revisa si ya se selecciono temporada
            if (temporada.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                //se habilitan los filtros que pueden utilizarse
                empaque.IsEnabled = true;

                //se limpian los datos cargados previamente
                estilo.Items.Clear();
                sam_op.Content = "----";
                estatus.Content = "----";
                muestra.Content = "----";
                elementos.Text = "----";
                total.Content = "----";

                // se carga la imagen inicial
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                foto.Source = new BitmapImage(fileUri);

                // se declaran las variables de conexion
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql;
                if (cliente.SelectedIndex == -1 && tipo.SelectedIndex == -1)
                {
                    sql = "select estilo from operaciones where temporada= '" + temporada.SelectedItem.ToString() + "' and estilo like '%" + buscar.Text + "%' group by estilo";
                    cn.Open();
                    // se cargan los estilos que cumplen con la temporada y la busqueda
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        estilo.Items.Add(dr["estilo"].ToString());
                    }
                    dr.Close();

                    // se oculta el circulo rojo de estilo
                    s5.Visibility = Visibility.Hidden;
                }

                else if (tipo.SelectedIndex > -1 && cliente.SelectedIndex > -1)
                {
                    sql = "select estilo from operaciones where temporada= '" + temporada.SelectedItem.ToString() + "' and cliente= '" + cliente.SelectedItem.ToString() + "' and tipo= '" + tipo.SelectedItem.ToString() + "' and estilo like '%" + buscar.Text + "%' group by estilo";
                    cn.Open();
                    // se cargan los estilos que cumplen con la temporada y la busqueda
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        estilo.Items.Add(dr["estilo"].ToString());
                    }
                    dr.Close();

                    // se oculta el circulo rojo de estilo
                    s5.Visibility = Visibility.Hidden;
                }

                else if (cliente.SelectedIndex > -1) 
                {
                    sql = "select estilo from operaciones where temporada= '" + temporada.SelectedItem.ToString() + "' and cliente= '" + cliente.SelectedItem.ToString() + "' and estilo like '%" + buscar.Text + "%' group by estilo";
                    cn.Open();
                    // se cargan los estilos que cumplen con la temporada y la busqueda
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        estilo.Items.Add(dr["estilo"].ToString());
                    }
                    dr.Close();

                    // se oculta el circulo rojo de estilo
                    s5.Visibility = Visibility.Hidden;
                }
              
            }
        }

        private void nueva_plantilla_Click(object sender, RoutedEventArgs e)
        {
            if (estilo.SelectedIndex < 0 || temporada.SelectedIndex < 0 || empaque.SelectedIndex < 0)
            {
                MessageBox.Show("Datos seleccionados incompletos");
            }

            else
            {
                //datos a pasar para generar nuevo balance
                clases.balance nuevoBalance = new clases.balance();
                nuevoBalance.nombre = estilo.SelectedItem.ToString();
                nuevoBalance.temporada = temporada.SelectedItem.ToString();
                nuevoBalance.samEmpaque = Convert.ToDouble(empaque_s.Content);
                nuevoBalance.samOperarcional = Convert.ToDouble(sam_op.Content);
                nuevoBalance.sam= Convert.ToDouble(sam_op.Content)+ Convert.ToDouble(empaque_s.Content); ;
                nuevoBalance.version = 1;
                nuevoBalance.tipo = "nuevo";
                nuevoBalance.nombreEmpaque= empaque.SelectedItem.ToString();
                nuevoBalance.fechaCreacion = DateTime.Now;
                this.NavigationService.Navigate(new balance(nuevoBalance));
                
            }
        }



    }
    public class _item
    {
        public string nombre { get; set; }

        public string empaqclase { get; set; }

        public string titulo { get; set; }

        public decimal sam { get; set; }

        public decimal samemp { get; set; }
    }

   static class Global
    {
        private static string _temporadaselec = "";
        private static string _estiloselec = "";
        private static string _samselec = "";
        private static string _samemp = "";
        private static string _empaqclase = "";
        private static string _identificador = "";

        public static string temporadaselec
        {
            get { return _temporadaselec; }
            set { _temporadaselec = value; }
        }

        public static string estiloselec
        {
            get { return _estiloselec; }
            set { _estiloselec = value; }
        }

        public static string samselec
        {
            get { return _samselec; }
            set { _samselec = value; }
        }


        public static string samemp
        {
            get { return _samemp; }
            set { _samemp = value; }
        }

        public static string empaqclase
        {
            get { return _empaqclase; }
            set { _empaqclase = value; }
        }

        public static string identificador
        {
            get { return _identificador; }
            set { _identificador= value; }
        }
    }

}





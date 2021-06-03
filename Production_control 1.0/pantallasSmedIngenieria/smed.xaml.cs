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

namespace Production_control_1._0.pantallasSmedIngenieria
{
    public partial class smed : Page
    {
        int ubicacion_ = 0;
        int codigo_ = 0;
        #region clases_especiales
        public SqlConnection cn_smed = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_smed"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cn_manto = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public class nuevo_movimiento
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
            public string movimiento { get; set; }
            public DateTime hora { get; set; }
        }
        public class nuevo_cambio
        {
            public int id_solicitud{ get; set; }
            public string modulo { get; set; }
            public DateTime hora_reportada { get; set; }
        }
        #endregion
        #region datos_iniciales
        public smed()
        {
            InitializeComponent();
            string sql = "select nombre from usuarios where [ingenieria/SMED]='1' and (cargo='SOPORTE' or cargo='MECANICO') order by nombre";
            string sql2 = "select accion from acciones";
            string sql3 = "select movimientos.codigo, personal.nombre, movimientos.movimiento, movimientos.hora from movimientos inner join(select codigo, max(id_movimiento) as id_maximo from movimientos group by codigo) as max_actividad on max_actividad.id_maximo=movimientos.id_movimiento inner join personal on movimientos.codigo = personal.codigo order by hora desc";
            string sql4 = "select modulo from orden_modulos group by modulo";
            cnIngenieria.Open();
            SqlCommand cm = new SqlCommand(sql, cnIngenieria);
            //agregar codigos de soportes smed
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                codigo.Items.Add(dr["nombre"].ToString());
            };
            dr.Close();
            cnIngenieria.Close();
            List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
            cn_smed.Open(); 
            SqlCommand cm2 = new SqlCommand(sql2, cn_smed);
            SqlCommand cm3 = new SqlCommand(sql3, cn_smed);
            SqlCommand cm4 = new SqlCommand(sql4, cn_manto);
            //afregar lista de acciones
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                accion.Items.Add(dr2["accion"].ToString());
            };
            dr2.Close();

            //agregar ultimas acciones
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                ultimos_movimientos.Add(new nuevo_movimiento { codigo = Convert.ToInt32(dr3["codigo"]), nombre = dr3["nombre"].ToString(), movimiento = dr3["movimiento"].ToString(), hora = Convert.ToDateTime(dr3["hora"]) });
            };
            dr3.Close();
            cn_smed.Close();
            movimientos.ItemsSource = ultimos_movimientos;

            //agregar lista de modulos
            cn_manto.Open();
            SqlDataReader dr4 = cm4.ExecuteReader();
            while (dr4.Read())
            {
                modulo.Items.Add(dr4["modulo"].ToString());
            };
            dr4.Close();
            cn_manto.Close();
            comboBoxArteria.Items.Add("1");
            comboBoxArteria.Items.Add("2");

            //habilitar boton
            habilitar_boton();
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
                tmp.FontSize = e.NewSize.Height * 0.022 / tmp.FontFamily.LineSpacing;
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
        #region control_general_del_programa
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
        #region calculos_generales
        private void habilitar_boton()
        {
            if (codigo.SelectedIndex == -1 || accion.SelectedIndex == -1)
            {
                actividad_smed.IsEnabled = false;
            }
            else
            {
                actividad_smed.IsEnabled = true;
            }
        }

        #endregion
        #region formulario_nueva_actividad_smed
        private void actividad_smed_Click(object sender, RoutedEventArgs e)
        {
            if (accion.SelectedItem.ToString() == "Falta de Trabajo")
            {
                passWordBoxValidarUsuario.Clear();
                labelNombreAutoriza.Content = "*";
                buttonIngresarActividad.IsEnabled = false;
                popUpValidarUsuario.IsOpen = true;
            }
            else
            {
                //ingresar nueva actividad de soporte smed
                string sql = "insert into movimientos  values('" + codigo_ + "', '" + accion.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
                cn_smed.Open();
                SqlCommand cm = new SqlCommand(sql, cn_smed);
                cm.ExecuteNonQuery();

                //refrescar tabla de ultimas actividades
                string sql2 = "select movimientos.codigo, personal.nombre, movimientos.movimiento, movimientos.hora from movimientos inner join(select codigo, max(id_movimiento) as id_maximo from movimientos group by codigo) as max_actividad on max_actividad.id_maximo=movimientos.id_movimiento inner join personal on movimientos.codigo = personal.codigo order by hora desc";
                List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
                SqlCommand cm2 = new SqlCommand(sql2, cn_smed);
                SqlDataReader dr2 = cm2.ExecuteReader();
                while (dr2.Read())
                {
                    ultimos_movimientos.Add(new nuevo_movimiento { codigo = Convert.ToInt32(dr2["codigo"]), nombre = dr2["nombre"].ToString(), movimiento = dr2["movimiento"].ToString(), hora = Convert.ToDateTime(dr2["hora"]) });
                };
                dr2.Close();
                cn_smed.Close();
                movimientos.ItemsSource = ultimos_movimientos;
            }
        }
        private void codigo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (codigo.SelectedIndex > -1)
            {
                string sql = "select codigo from usuarios where nombre='" + codigo.SelectedItem.ToString() + "'";
                cnIngenieria.Open();
                SqlCommand cm = new SqlCommand(sql, cnIngenieria);
                //agregar codigos de soportes smed
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    codigo_ = Convert.ToInt32(dr["codigo"].ToString());
                };
                dr.Close();
                cnIngenieria.Close();
                habilitar_boton();
            }
        }
        private void accion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitar_boton();
        }
        private void modulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sql = "select id from orden_modulos where modulo='" + modulo.SelectedItem.ToString() + "'";
            cn_manto.Open();
            SqlCommand cm = new SqlCommand(sql, cn_manto);
            SqlDataReader dr = cm.ExecuteReader();
            if (dr.Read())
            {
                ubicacion_ = Convert.ToInt32(dr["id"]);
            };
            dr.Close();
            cn_manto.Close();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string sql = "select nombre from usuarios where [ingenieria/SMED]='1' and nivel='1' and contrasena='" + passWordBoxValidarUsuario.Password + "'";
            cnIngenieria.Open();
            SqlCommand cm = new SqlCommand(sql, cnIngenieria);
            SqlDataReader dr = cm.ExecuteReader();
            if (dr.Read())
            {
                labelNombreAutoriza.Content = dr["nombre"].ToString();
                buttonIngresarActividad.IsEnabled = true;
            }
            else
            {
                labelNombreAutoriza.Content = "*";
                buttonIngresarActividad.IsEnabled = false;
            };
            dr.Close();
            cnIngenieria.Close();
        }
        private void buttonIngresarActividad_Click(object sender, RoutedEventArgs e)
        {
            //ingresar nueva actividad de soporte smed
            string sql = "insert into movimientos  values('" + codigo_ + "', '" + accion.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
            cn_smed.Open();
            SqlCommand cm = new SqlCommand(sql, cn_smed);
            cm.ExecuteNonQuery();

            //refrescar tabla de ultimas actividades
            string sql2 = "select movimientos.codigo, personal.nombre, movimientos.movimiento, movimientos.hora from movimientos inner join(select codigo, max(id_movimiento) as id_maximo from movimientos group by codigo) as max_actividad on max_actividad.id_maximo=movimientos.id_movimiento inner join personal on movimientos.codigo = personal.codigo order by hora desc";
            List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
            SqlCommand cm2 = new SqlCommand(sql2, cn_smed);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                ultimos_movimientos.Add(new nuevo_movimiento { codigo = Convert.ToInt32(dr2["codigo"]), nombre = dr2["nombre"].ToString(), movimiento = dr2["movimiento"].ToString(), hora = Convert.ToDateTime(dr2["hora"]) });
            };
            dr2.Close();
            cn_smed.Close();
            movimientos.ItemsSource = ultimos_movimientos;
            popUpValidarUsuario.IsOpen = false;
        }
        private void ButtonCerrarPopup2_Click(object sender, RoutedEventArgs e)
        {
            popUpValidarUsuario.IsOpen = false;
        }
        #endregion
        #region formulario_nuevo_cambio
        private void cambio_Click(object sender, RoutedEventArgs e)
        {
            if (modulo.SelectedIndex > -1 && comboBoxArteria.SelectedIndex>-1)
            {
                string sql = "insert into solicitudes (modulo, arteria, ubicacion, problema_reportado, hora_reportada, hora_apertura, corresponde)  values('" + modulo.SelectedItem.ToString() + "', '"+comboBoxArteria.SelectedItem.ToString()+"', '"+ ubicacion_ + "', 'CAMBIO', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', 'SMED')";
                cn_manto.Open();
                SqlCommand cm = new SqlCommand(sql, cn_manto);
                cm.ExecuteNonQuery();
                cn_manto.Close();
                this.NavigationService.Navigate(new estadoPlantaProduccion());
            }
            else
            {
                MessageBox.Show("Seleccione todos los datos");
            }
        }
        #endregion

    }
}

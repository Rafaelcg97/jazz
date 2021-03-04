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
    /// Interaction logic for smed.xaml
    /// </summary>
    public partial class smed : Page
    {
        #region clases_especiales
        public SqlConnection cn_smed = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_smed"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);

        public SqlConnection cn_manto = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);

        public string cs = "Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"];

        public class nuevo_movimiento
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
            public string movimiento { get; set; }
            public DateTime hora { get; set; }
        }

        public class item_actualizacion
        {
            public int id { get; set; }
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
            string sql = "select codigo from personal";
            string sql2 = "select accion from acciones";
            string sql3 = "select movimientos.codigo, personal.nombre, movimientos.movimiento, movimientos.hora from movimientos inner join(select codigo, max(id_movimiento) as id_maximo from movimientos group by codigo) as max_actividad on max_actividad.id_maximo=movimientos.id_movimiento inner join personal on movimientos.codigo = personal.codigo order by hora desc";
            string sql4 = "select modulo from modulos";
            List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
            cn_smed.Open();
            SqlCommand cm = new SqlCommand(sql, cn_smed);
            SqlCommand cm2 = new SqlCommand(sql2, cn_smed);
            SqlCommand cm3 = new SqlCommand(sql3, cn_smed);
            SqlCommand cm4 = new SqlCommand(sql4, cn_manto);

            //agregar codigos de soportes smed
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                codigo.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();

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

            //habilitar boton
            habilitar_boton();

            //revisar si se actualizan modulos del piso
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
            inicio inicio = new inicio();
            this.NavigationService.Navigate(inicio);
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
            List<nuevo_cambio> cambios_pendientes = new List<nuevo_cambio>();
            try
            {
                string sql = "select id_solicitud, hora_reportada, modulo from solicitudes where corresponde= 'SMED' AND hora_cierre IS NULL and problema_reportado='CAMBIO'";
                cn_manto.Open();
                SqlCommand cm = new SqlCommand(sql, cn_manto);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cambios_pendientes.Add(new nuevo_cambio { id_solicitud = Convert.ToInt32(dr["id_solicitud"]), modulo = dr["modulo"].ToString(), hora_reportada = Convert.ToDateTime(dr["hora_reportada"])});
                };
                dr.Close();
                cn_manto.Close();
                cambios_abiertos.ItemsSource = cambios_pendientes;
                monitorear_tabla();
            }
            catch
            {
                monitorear_tabla();
            }
        }


        #endregion

        #region formulario_nueva_actividad_smed

        private void actividad_smed_Click(object sender, RoutedEventArgs e)
        {
            if (accion.SelectedItem.ToString() == "Falta de Trabajo")
            {
                #region tamano_de_pop
                verificar.MaxWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                verificar.MinWidth = (System.Windows.SystemParameters.PrimaryScreenWidth) / 4;
                verificar.MaxHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                verificar.MinHeight = (System.Windows.SystemParameters.PrimaryScreenHeight) / 3;
                #endregion
                verificar.IsOpen = true;
                codigo_autorizacion.Password = "";
                validado.Content = "*";
                enviar.IsEnabled = false;
            }
            else
            {
                //ingresar nueva actividad de soporte smed
                string sql = "insert into movimientos  values('" + codigo.SelectedItem.ToString() + "', '" + accion.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
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
            habilitar_boton();
        }

        private void accion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitar_boton();
        }

        private void codigo_autorizacion_PasswordChanged(object sender, RoutedEventArgs e)
        {
            string sql = "select nombre from soportes where contrasena= '" + codigo_autorizacion.Password + "'";
            cn_manto.Open();
            SqlCommand cm = new SqlCommand(sql, cn_manto);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                validado.Content = dr["nombre"].ToString();
            };
            dr.Close();
            cn_manto.Close();
            if (validado.Content.ToString() == "Coordinacion SMED")
            {
                enviar.IsEnabled = true;
            }
            else
            {
                enviar.IsEnabled = false;
            }
        }

        private void enviar_Click(object sender, RoutedEventArgs e)
        {
            //ingresar nueva actividad de soporte smed
            string sql = "insert into movimientos  values('" + codigo.SelectedItem.ToString() + "', '" + accion.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '1')";
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
            verificar.IsOpen = false;
        }

        #endregion

        #region formulario_nuevo_cambio
        private void cambio_Click(object sender, RoutedEventArgs e)
        {
            if (modulo.SelectedIndex > -1)
            {
                string sql = "insert into solicitudes (modulo, problema_reportado, hora_reportada, hora_apertura, corresponde)  values('" + modulo.SelectedItem.ToString() + "', 'CAMBIO', " + "'" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', 'SMED')";
                string sql2 = "insert into actualizacion(evento) values(1)";
                cn_manto.Open();
                SqlCommand cm = new SqlCommand(sql, cn_manto);
                SqlCommand cm2 = new SqlCommand(sql2, cn_manto);
                cm.ExecuteNonQuery();
                cm2.ExecuteNonQuery();
                cn_manto.Close();
            }
        }

        private void cambios_abiertos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (cambios_abiertos.SelectedIndex > -1)
            {
               foreach(nuevo_cambio item in cambios_abiertos.SelectedItems)
                {
                    string sql = "update solicitudes set hora_cierre= '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "' where id_solicitud= '" + item.id_solicitud.ToString() + "'";
                    string sql2 = "insert into actualizacion(evento) values(1)";
                    cn_manto.Open();
                    SqlCommand cm = new SqlCommand(sql, cn_manto);
                    SqlCommand cm2 = new SqlCommand(sql2, cn_manto);
                    cm.ExecuteNonQuery();
                    cm2.ExecuteNonQuery();
                    cn_manto.Close();
                }



               
            }
        }

        #endregion


    }
}

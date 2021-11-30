using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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

namespace JazzCCO._0.pantallasMantenimiento
{

    public partial class controlTiemposMecanicos : Page
    {
        #region clases_especiales
        public SqlConnection cn_manto = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public class nuevo_movimiento
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
            public string movimiento { get; set; }
            public DateTime hora { get; set; }
        }
        #endregion
        public controlTiemposMecanicos()
        {
            InitializeComponent();
            string sql = "SELECT codigo, nombre FROM usuarios WHERE mantenimiento=1 AND CARGO IN ('MECANICO') order by nombre";
            string sql2 = "select accion from accionesMecanicos";
            string sql3 = "select movimientosMecanicos.codigo, usuarios.nombre, movimientosMecanicos.movimiento, movimientosMecanicos.hora from movimientosMecanicos inner join(select codigo, max(id) as id_maximo from movimientosMecanicos group by codigo) as max_actividad on max_actividad.id_maximo = movimientosMecanicos.id inner join ingenieria.dbo.usuarios on movimientosMecanicos.codigo = usuarios.codigo order by hora desc";
            cnIngenieria.Open();
            SqlCommand cm = new SqlCommand(sql, cnIngenieria);
            //agregar codigos de soportes smed
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                codigo.Items.Add(new nuevo_movimiento { nombre = dr["nombre"].ToString(), codigo = Convert.ToInt32(dr["codigo"] is DBNull ? 0 : dr["codigo"]) });
            };
            dr.Close();
            cnIngenieria.Close();
            List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
            cn_manto.Open();
            SqlCommand cm2 = new SqlCommand(sql2, cn_manto);
            SqlCommand cm3 = new SqlCommand(sql3, cn_manto);
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
            cn_manto.Close();
            movimientos.ItemsSource = ultimos_movimientos;

            //habilitar boton
            habilitar_boton();
        }
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
            nuevo_movimiento item = (nuevo_movimiento)codigo.SelectedItem;
            //ingresar nueva actividad de soporte smed
            string sql = "insert into movimientosMecanicos  values('" + item.codigo + "', '" + accion.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "')";
            cn_manto.Open();
            SqlCommand cm = new SqlCommand(sql, cn_manto);
            cm.ExecuteNonQuery();

            //refrescar tabla de ultimas actividades
            string sql2 = "select movimientosMecanicos.codigo, usuarios.nombre, movimientosMecanicos.movimiento, movimientosMecanicos.hora from movimientosMecanicos inner join(select codigo, max(id) as id_maximo from movimientosMecanicos group by codigo) as max_actividad on max_actividad.id_maximo = movimientosMecanicos.id inner join ingenieria.dbo.usuarios on movimientosMecanicos.codigo = usuarios.codigo order by hora desc";
            List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
            SqlCommand cm2 = new SqlCommand(sql2, cn_manto);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                ultimos_movimientos.Add(new nuevo_movimiento { codigo = Convert.ToInt32(dr2["codigo"]), nombre = dr2["nombre"].ToString(), movimiento = dr2["movimiento"].ToString(), hora = Convert.ToDateTime(dr2["hora"]) });
            };
            dr2.Close();
            cn_manto.Close();
            movimientos.ItemsSource = ultimos_movimientos;
        }
        private void codigo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitar_boton();
        }
        private void accion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitar_boton();
        }
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            cn_manto.Open();
            foreach (nuevo_movimiento item in codigo.Items)
            {
                string sql = "select codigo from usuarios where nombre='" + item.codigo + "'";
                //ingresar nueva actividad de soporte smed
                sql = "insert into movimientosMecanicos  values('" + item.codigo + "', 'Final de Jornada', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "')";
                SqlCommand cm = new SqlCommand(sql, cn_manto);
                cm.ExecuteNonQuery();
            }

            //refrescar tabla de ultimas actividades
            string sql2 = "select movimientosMecanicos.codigo, usuarios.nombre, movimientosMecanicos.movimiento, movimientosMecanicos.hora from movimientosMecanicos inner join(select codigo, max(id) as id_maximo from movimientosMecanicos group by codigo) as max_actividad on max_actividad.id_maximo = movimientosMecanicos.id inner join ingenieria.dbo.usuarios on movimientosMecanicos.codigo = usuarios.codigo order by hora desc";
            List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
            SqlCommand cm2 = new SqlCommand(sql2, cn_manto);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                ultimos_movimientos.Add(new nuevo_movimiento { codigo = Convert.ToInt32(dr2["codigo"]), nombre = dr2["nombre"].ToString(), movimiento = dr2["movimiento"].ToString(), hora = Convert.ToDateTime(dr2["hora"]) });
            };
            dr2.Close();
            cn_manto.Close();
            movimientos.ItemsSource = ultimos_movimientos;
            cn_manto.Close();
            habilitar_boton();
        }
    }
}

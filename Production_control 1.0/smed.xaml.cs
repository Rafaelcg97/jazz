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

        public class nuevo_movimiento
        {
            public int codigo { get; set; }
            public string nombre { get; set; }
            public string movimiento { get; set; }
            public DateTime hora { get; set; }
        }
        #endregion

        #region datos_iniciales
        public smed()
        {
            InitializeComponent();
            string sql = "select codigo from personal";
            string sql2 = "select accion from acciones";
            string sql3 = "select movimientos.codigo, personal.nombre, movimientos.movimiento, movimientos.hora from movimientos inner join(select codigo, max(id_movimiento) as id_maximo from movimientos group by codigo) as max_actividad on max_actividad.id_maximo=movimientos.id_movimiento inner join personal on movimientos.codigo = personal.codigo order by hora desc";
            List<nuevo_movimiento> ultimos_movimientos = new List<nuevo_movimiento>();
            cn_smed.Open();
            SqlCommand cm = new SqlCommand(sql, cn_smed);
            SqlCommand cm2 = new SqlCommand(sql2, cn_smed);
            SqlCommand cm3 = new SqlCommand(sql3, cn_smed);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                codigo.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
                accion.Items.Add(dr2["accion"].ToString());
            };
            dr2.Close();
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                ultimos_movimientos.Add(new nuevo_movimiento { codigo = Convert.ToInt32(dr3["codigo"]), nombre = dr3["nombre"].ToString(), movimiento = dr3["movimiento"].ToString(), hora = Convert.ToDateTime(dr3["hora"]) });
            };
            dr2.Close();
            cn_smed.Close();
            movimientos.ItemsSource = ultimos_movimientos;
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

    }
}

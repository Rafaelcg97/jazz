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
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasProduccion
{
    public partial class cuadrarLotes : Page
    {
        #region stringsGenerales
        List<horaProduccion> listaTodosLotes = new List<horaProduccion>();
        List<horaProduccion> lotesPorModulo = new List<horaProduccion>();
        SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        #region datosIniciales
        public cuadrarLotes()
        {
            InitializeComponent();
            string sql = "select lote, xxs, xs, s, m, l, xl, xxl, xxxl, totalDePiezas from lotesAgrupados";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                listaTodosLotes.Add(new horaProduccion { lote = dr["lote"].ToString(), xxs = Convert.ToInt32(dr["xxs"]), xs = Convert.ToInt32(dr["xs"]), s = Convert.ToInt32(dr["s"]), m = Convert.ToInt32(dr["m"]), l = Convert.ToInt32(dr["l"]), xl = Convert.ToInt32(dr["xl"]), xxl = Convert.ToInt32(dr["xxl"]), xxxl = Convert.ToInt32(dr["xxxl"]), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]) });
            };
            //se termina la conexion a la base
            dr.Close();

            sql = "select modulo from modulosProduccion";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                comboBoxModulo.Items.Add(dr["modulo"].ToString());
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
            foreach(horaProduccion item in listaTodosLotes)
            {
                listViewLotesLista.Items.Add(item);
            }
        }
        #endregion
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PagePrincipal());

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
        #region tamanos_de_letra_/_tipo_de_texto
        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void soloNumerosDecimales(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Decimal) || (e.Key == Key.Tab))
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
        #region controlesDeConsulta
        private void textBoxLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewLotesLista.Items.Clear();
            listViewLotesDetalles.Items.Clear();
            if (String.IsNullOrEmpty(textBoxLote.Text.Trim()) == false)
            {
                foreach (horaProduccion item in listaTodosLotes)
                {
                    if (item.lote.StartsWith(textBoxLote.Text.Trim()))
                    {
                        listViewLotesLista.Items.Add(item);
                    }
                }
            }
            else if (textBoxLote.Text.Trim() == "")
            {
                foreach (horaProduccion item in listaTodosLotes)
                {
                    listViewLotesLista.Items.Add(item);
                }
            }
        }
        private void listViewLotesLista_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewLotesLista.SelectedIndex > -1)
            {
                string loteSeleccionado = ((horaProduccion)listViewLotesLista.SelectedItem).lote;
                listViewLotesDetalles.Items.Clear();
                string sql = "select fecha, modulo, [2XS] as xxs, xs, s, m, l, xl, [2XL] as xxl, [3XL] as xxxl, totalDePiezas from horahora where lote='" + loteSeleccionado + "'";
                cnProduccion.Open();
                SqlCommand cm = new SqlCommand(sql, cnProduccion);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewLotesDetalles.Items.Add(new horaProduccion { fecha= Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"), modulo=dr["modulo"].ToString(), xxs = Convert.ToInt32(dr["xxs"]), xs = Convert.ToInt32(dr["xs"]), s = Convert.ToInt32(dr["s"]), m = Convert.ToInt32(dr["m"]), l = Convert.ToInt32(dr["l"]), xl = Convert.ToInt32(dr["xl"]), xxl = Convert.ToInt32(dr["xxl"]), xxxl = Convert.ToInt32(dr["xxxl"]), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]) });
                };
                //se termina la conexion a la base
                dr.Close();
                cnProduccion.Close();
                foreach (horaProduccion item in listaTodosLotes)
                {
                    listViewLotesLista.Items.Add(item);
                }

            }
        }
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            textBoxLotePorModulo.Clear();
            listViewLotesPorModulo.Items.Clear();
            lotesPorModulo.Clear();
            string sql = "SELECT Modulo, Lote, SUM([2XS]) AS xxs, SUM(XS) AS xs, SUM(S) AS s, SUM(M) AS m, SUM(L) AS l, SUM(XL) AS xl, SUM([2XL]) AS xxl, SUM([3XL]) AS xxxl, SUM(TotalDePiezas) AS totalDePiezas FROM dbo.horahora where Modulo='"+comboBoxModulo.SelectedItem.ToString()+"' GROUP BY Lote, Modulo";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                lotesPorModulo.Add(new horaProduccion { modulo=dr["modulo"].ToString(), lote = dr["lote"].ToString(), xxs = Convert.ToInt32(dr["xxs"]), xs = Convert.ToInt32(dr["xs"]), s = Convert.ToInt32(dr["s"]), m = Convert.ToInt32(dr["m"]), l = Convert.ToInt32(dr["l"]), xl = Convert.ToInt32(dr["xl"]), xxl = Convert.ToInt32(dr["xxl"]), xxxl = Convert.ToInt32(dr["xxxl"]), totalDePiezas = Convert.ToInt32(dr["totalDePiezas"]) });
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
            foreach(horaProduccion item in lotesPorModulo)
            {
                listViewLotesPorModulo.Items.Add(item);
            }
        }
        private void textBoxLotePorModulo_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewLotesPorModulo.Items.Clear();
            if (String.IsNullOrEmpty(textBoxLotePorModulo.Text.Trim()) == false)
            {
                foreach (horaProduccion item in lotesPorModulo)
                {
                    if (item.lote.StartsWith(textBoxLotePorModulo.Text.Trim()))
                    {
                        listViewLotesPorModulo.Items.Add(item);
                    }
                }
            }
            else if (textBoxLotePorModulo.Text.Trim() == "")
            {
                foreach (horaProduccion item in lotesPorModulo)
                {
                    listViewLotesPorModulo.Items.Add(item);
                }
            }

        }
        #endregion
    }
}

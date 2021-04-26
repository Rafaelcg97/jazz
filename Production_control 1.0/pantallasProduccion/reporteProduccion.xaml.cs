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
    public partial class reporteProduccion : Page
    {
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public int piezasLote = 0;
        public int piezasReportadas = 0;
        #endregion

        #region datosInciales
        public reporteProduccion()
        {
            InitializeComponent();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #region datosDeCombBox
            //llenar lista de modulos
            cnProduccion.Open();
            sql = "select modulo from modulosProduccion";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxModulo.Items.Add(dr["modulo"].ToString());
            };
            dr.Close();
            cnProduccion.Close();

            //llenar lista de horas
            cnProduccion.Open();
            sql = "select hora from horas";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxHora.Items.Add(dr["hora"].ToString());
            };
            dr.Close();
            cnProduccion.Close();

            //llenar lista de arterias
            cnProduccion.Open();
            sql = "select arteria from arterias";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxArteria.Items.Add(dr["arteria"].ToString());
            };
            dr.Close();
            cnProduccion.Close();

            #endregion

            DateTime fechaSieteAM = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddHours(7);
            DateTime fechaCincoTreintaPM = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddHours(17).AddMinutes(30);
            Uri luna = new Uri("/imagenes/luna.png", UriKind.RelativeOrAbsolute);
            Uri sol = new Uri("/imagenes/sol.png", UriKind.RelativeOrAbsolute);

            //Determinar el turno
            string turno = "Diurno";
            if (DateTime.Now < fechaSieteAM || DateTime.Now > fechaCincoTreintaPM)
            {
                turno = "Nocturno";
                imageTurno.Source = new BitmapImage(luna);
            }
            else
            {
                turno = "Diurno";
                imageTurno.Source = new BitmapImage(sol);
            }
            labelTurno.Content = turno;

            //Determinar la fecha
            string fecha = "Diurno";
            if (DateTime.Now < fechaSieteAM)
            {
                fecha = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
            }
            else
            {
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }

            labelFecha.Content = fecha;
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

        #region tamanos_de_letra_/_tipo_de_texto

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void soloNumerosDecimales(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Decimal))
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

        #region formularioGener
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sql;
            SqlCommand cm;
            SqlDataReader dr;

            //colocar coordinador
            cnProduccion.Open();
            sql = "select coordinador from modulosProduccion where modulo='" + comboBoxModulo.SelectedItem.ToString() + "'";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            dr.Read();
            labelCoordinador.Content = dr["coordinador"].ToString();
            dr.Close();
            cnProduccion.Close();
        }

        private void checkBoxExtra_Checked(object sender, RoutedEventArgs e)
        {
            Uri cometa = new Uri("/imagenes/cometa.png", UriKind.RelativeOrAbsolute);
            imageTurno.Source = new BitmapImage(cometa);

        }

        private void checkBoxExtra_Unchecked(object sender, RoutedEventArgs e)
        {
            DateTime fechaSieteAM = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddHours(7);
            DateTime fechaCincoTreintaPM = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")).AddHours(17).AddMinutes(30);
            Uri luna = new Uri("/imagenes/luna.png", UriKind.RelativeOrAbsolute);
            Uri sol = new Uri("/imagenes/sol.png", UriKind.RelativeOrAbsolute);

            //Determinar el turno
            if (DateTime.Now < fechaSieteAM || DateTime.Now > fechaCincoTreintaPM)
            {
                imageTurno.Source = new BitmapImage(luna);
            }
            else
            {
                imageTurno.Source = new BitmapImage(sol);
            }

        }

        #endregion

        #region FomrularioPop
        private void ButtonCerrarPopup_Click(object sender, RoutedEventArgs e)
        {
            popUpLote.IsOpen = false;
        }

        private void buttonAgregarLote_Click(object sender, RoutedEventArgs e)
        {
            listBoxLote.Items.Clear();
            textBoxLote.Clear();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            //llenar lista de lotes
            cnIngenieria.Open();
            sql = "select top 10 lote from lotes_unicos_poly";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxLote.Items.Add(dr["lote"].ToString());
            };
            dr.Close();
            cnIngenieria.Close();
            popUpLote.IsOpen = true;
        }

        private void textBoxLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            listBoxLote.Items.Clear();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            //llenar lista de lotes
            cnIngenieria.Open();
            sql = "select top 10 lote from lotes_unicos_poly where lote like '" + textBoxLote.Text + "%'";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxLote.Items.Add(dr["lote"].ToString());
            };
            dr.Close();
            cnIngenieria.Close();
        }

        private void ButtonIngresarContrasena_Click(object sender, RoutedEventArgs e)
        {
            //agregar registro uno de lote
            if (listBoxLote.SelectedIndex > -1)
            {
                listViewLotes.Items.Add(new horaProduccion { lote = listBoxLote.SelectedItem.ToString(), piezas = piezasLote, terminadas = piezasReportadas });
                listViewPiezas.Items.Add(new horaProduccion { lote = listBoxLote.SelectedItem.ToString(), piezas = piezasLote, terminadas = piezasReportadas });
                popUpLote.IsOpen = false;
            }
            else
            {
                MessageBox.Show("No seleccionaste ningun lote");
            }

        }

        private void listBoxLote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                //llenar lista de lotes
                cnIngenieria.Open();
                sql = "select sum (cantidad) from lotespoly where lote='" + listBoxLote.SelectedItem.ToString() + "'";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                dr.Read();
                piezasLote = Convert.ToInt32(dr[0]);
                dr.Close();
                cnIngenieria.Close();

                cnProduccion.Open();
                sql = "select sum(totalDePiezas) from horahora where lote='" + listBoxLote.SelectedItem.ToString() + "'";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                dr.Read();
                piezasReportadas = Convert.ToInt32(dr[0] is DBNull ? 0 : dr[0]);
                dr.Close();
                cnProduccion.Close();
            }
        }

        #endregion

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            calculosLotes();
        }
        private void calculosLotes()
        {
            List<horaProduccion> listaCalculada = new List<horaProduccion>();
            double minutosLote = 0;
            double minutosHora = 0;
            string _color = "";
            int piezasLote = 0;
            int totalPiezasTerminadas = 0;
            double _sam = 0;
            foreach(horaProduccion item in listViewLotes.Items)
            {
                #region consultaSAM
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                //consultar
                cnProduccion.Open();
                sql = "select sam from sam where codigo='" + item.codigo + "'";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    _sam = Convert.ToDouble(dr["sam"]);
                }
                dr.Close();
                cnProduccion.Close();
                #endregion
                piezasLote= item.xxs + item.xs + item.s + item.m + item.l + item.xl + item.xxl + item.xxxl;
                totalPiezasTerminadas = item.terminadas + piezasLote;
                if (totalPiezasTerminadas > item.piezas) { _color = "Red"; } else { _color = "Green"; }
                minutosLote = _sam * piezasLote;
                minutosHora = minutosHora + minutosLote;
                listaCalculada.Add(new horaProduccion {lote=item.lote, colorLote=_color, piezas=item.piezas, terminadas=totalPiezasTerminadas, minutosEfectivos=minutosLote, tiempoParo=item.tiempoParo, sam=_sam, xxs=item.xxs, xs=item.xs, s=item.s, m=item.m, l=item.l, xl=item.xl, xxl=item.xxl, xxxl=item.xxxl});
            }
            listViewPiezas.Items.Clear();
            foreach(horaProduccion item2 in listaCalculada)
            {
                listViewPiezas.Items.Add(new horaProduccion {lote = item2.lote, colorLote=_color, piezas = item2.piezas, terminadas = item2.terminadas, minutosEfectivos = (60*(item2.minutosEfectivos/minutosHora))-item2.tiempoParo, sam=item2.sam });
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            calculosLotes();
        }
    }

}


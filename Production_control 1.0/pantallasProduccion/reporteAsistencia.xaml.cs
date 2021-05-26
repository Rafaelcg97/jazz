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
    public partial class reporteAsistencia : Page
    {
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public int piezasReportadas = 0;
        #endregion
        List<string> modulos_ = new List<string>();
        string[] turnos_ = new string[3];
        int[] arterias_ = new int[3];
        string[] movimientos_ = new string[4];
        string[] puestos_ = new string[2];
        #region datosIniciales
        public reporteAsistencia()
        {
            InitializeComponent();
            //agregar modulos
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            cnProduccion.Open();
            sql = "select modulo from modulosProduccion where coordinadorNombre<>'-'";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulos_.Add(dr["modulo"].ToString());
            };
            dr.Close();
            cnProduccion.Close();
            //agregar turnos 
            comboBoxModulo.ItemsSource = modulos_;
            turnos_[0] = "Diurno";
            turnos_[1] = "Nocturno";
            turnos_[2] = "Extra";
            comboBoxTurno.ItemsSource = turnos_;
            arterias_[0] = 1;
            arterias_[1] = 2;
            movimientos_[0] = "PERMISO PERSONAL/CLINICA EMPRESARIAL";
            movimientos_[1] = "PERMISO PERSONAL/TRAMITE PERSONAL";
            movimientos_[2] = "AUSENCIA INJUSTIFICADA";
            movimientos_[3] = "PERMISO PERSONAL/CITA ISSS";
            puestos_[0] = "OPERARIO(A)";
            puestos_[1] = "CORREDOR";
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
        private void consultarDatosAsistencia()
        {
            if(comboBoxTurno.SelectedIndex>-1 && comboBoxModulo.SelectedIndex>-1 && labelFecha.Content.ToString() != "----")
            {
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                //llenar datos de asistencia guardada
                cnProduccion.Open();
                sql = "select codigo_at, nombre_at, modulo_at, arteria_at, tiempo_at, base_at, puesto_at, observaciones_at, movimiento_at from asistencia where asignado_at='" + comboBoxModulo.SelectedItem.ToString() + "' and turno_at='" + comboBoxTurno.SelectedItem.ToString() + "' and fecha_at='" + labelFecha.Content.ToString() + "'";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                listViewAsistencia.Items.Clear();
                while (dr.Read())
                {
                    listViewAsistencia.Items.Add(new asistencia {codigo=Convert.ToInt32(dr["codigo_at"]), modulos=modulos_, arterias=arterias_, puestos=puestos_, nombre=dr["nombre_at"].ToString(), modulo=dr["modulo_at"].ToString(), arteria=Convert.ToInt32(dr["arteria_at"]), tiempo=Convert.ToInt32(dr["tiempo_at"]), movimiento=dr["movimiento_at"].ToString(), puesto=dr["puesto_at"].ToString(), observaciones=dr["observaciones_at"].ToString(), movimientos=movimientos_ });
                };
                dr.Close();
                cnProduccion.Close();
            }
        }
        private void calendarAsistenciaRetractil_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            labelFecha.Content = ((DateTime)calendarAsistenciaRetractil.SelectedDate).ToString("yyyy-MM-dd");
            expanderControl.IsExpanded = false;
            consultarDatosAsistencia();
        }
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            consultarDatosAsistencia();
        }
        private void comboBoxTurno_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            consultarDatosAsistencia();
        }
    }
}
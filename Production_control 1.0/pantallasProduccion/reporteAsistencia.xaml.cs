using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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
using Microsoft.Win32;
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
        #region listasGenerales
        List<string> modulos_ = new List<string>();
        List<string> modulosCompletos = new List<string>();
        string[] turnos_ = new string[3];
        int[] arterias_ = new int[3];
        string[] movimientos_ = new string[6];
        string[] puestos_ = new string[3];
        #endregion
        #region datosIniciales
        public reporteAsistencia(int codigoCoordinador)
        {
            InitializeComponent();
            //agregar modulos
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            cnProduccion.Open();
            sql = "select modulo, coordinadorCodigo from modulosProduccion where coordinadorNombre<>'-'";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                if(Convert.ToInt32(dr["coordinadorCodigo"] is DBNull ? 0 : dr["coordinadorCodigo"]) == codigoCoordinador)
                {
                    modulos_.Add(dr["modulo"].ToString());
                }
                modulosCompletos.Add(dr["modulo"].ToString());
            };
            dr.Close();
            cnProduccion.Close();
            //agregar turnos 
            comboBoxModulo.ItemsSource = modulos_;
            comboBoxAsignado.ItemsSource = modulos_;
            turnos_[0] = "Diurno";
            turnos_[1] = "Nocturno";
            turnos_[2] = "Extra";
            comboBoxTurno.ItemsSource = turnos_;
            comboBoxTurnoLista.ItemsSource = turnos_;
            arterias_[0] = 1;
            arterias_[1] = 2;
            arterias_[2] = 3;
            movimientos_[0] = "PERMISO PERSONAL/CLINICA EMPRESARIAL";
            movimientos_[1] = "PERMISO PERSONAL/TRAMITE PERSONAL";
            movimientos_[2] = "AUSENCIA INJUSTIFICADA";
            movimientos_[3] = "PERMISO PERSONAL/CITA ISSS";
            movimientos_[4] = "INASISTENCIA";
            movimientos_[5] = "-";
            puestos_[0] = "OPERARIO(A)";
            puestos_[1] = "CORREDOR";
            puestos_[2] = "LAVADO";
            comboBoxBase.Items.Add("5.1");
            comboBoxBase.Items.Add("8");
            comboBoxBase.Items.Add("8.8");
            comboBoxBase.Items.Add("9");
            comboBoxBase.Items.Add("13.9");
        }
        #endregion
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();

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
        #region calculosGenerales
        private void consultarDatosAsistencia()
        {
            if(comboBoxTurno.SelectedIndex>-1 && comboBoxModulo.SelectedIndex>-1 && labelFecha.Content.ToString() != "----")
            {
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                comboBoxBase.SelectedIndex = -1;
                comboBoxAsignado.SelectedIndex = -1;
                comboBoxTurnoLista.SelectedIndex = -1;
                calendarAsistencia.SelectedDate = DateTime.Now;
                //llenar datos de asistencia guardada
                cnProduccion.Open();
                sql = "select fecha_at, turno_at, asignado_at, codigo_at, nombre_at, modulo_at, arteria_at, tiempo_at, base_at, puesto_at, observaciones_at, movimiento_at from asistencia where asignado_at='" + comboBoxModulo.SelectedItem.ToString() + "' and turno_at='" + comboBoxTurno.SelectedItem.ToString() + "' and fecha_at='" + labelFecha.Content.ToString() + "'";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                listViewAsistencia.Items.Clear();
                while (dr.Read())
                {
                    listViewAsistencia.Items.Add(new asistencia {codigo=Convert.ToInt32(dr["codigo_at"]), modulos=modulosCompletos, arterias=arterias_, puestos=puestos_, nombre=dr["nombre_at"].ToString(), modulo=dr["modulo_at"].ToString(), arteria=Convert.ToInt32(dr["arteria_at"]), tiempo=Convert.ToDouble(dr["tiempo_at"]), movimiento=dr["movimiento_at"].ToString(), puesto=dr["puesto_at"].ToString(), observaciones=dr["observaciones_at"].ToString(), movimientos=movimientos_ });
                    comboBoxTurnoLista.SelectedItem = dr["turno_at"].ToString();
                    comboBoxBase.SelectedItem = dr["base_at"].ToString();
                    comboBoxAsignado.SelectedItem = dr["asignado_at"].ToString();
                    calendarAsistencia.SelectedDate = Convert.ToDateTime(dr["fecha_at"]);
                    calendarAsistencia.DisplayDate= Convert.ToDateTime(dr["fecha_at"]);
                };
                dr.Close();
                cnProduccion.Close();
            }
        }
        #endregion
        #region controlesDeFiltro
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
        #endregion
        #region controlDatos
        private void textBoxCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            labelNombre.Content = "----";
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            cnProduccion.Open();
            sql = "select nombre from nomina where codigo='"+textBoxCodigo.Text+"'";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            if (dr.Read())
            {
                labelNombre.Content = dr["nombre"].ToString();
                buttonAgregarColaborador.IsEnabled = true;
            }
            else
            {
                labelNombre.Content = "----";
                buttonAgregarColaborador.IsEnabled = false;
            }
            dr.Close();
            cnProduccion.Close();
        }
        private void buttonAgregarColaborador_Click(object sender, RoutedEventArgs e)
        {
            listViewAsistencia.Items.Add(new asistencia {modulos=modulos_, arterias=arterias_, puestos=puestos_, movimientos=movimientos_, codigo=Convert.ToInt32(textBoxCodigo.Text), nombre=labelNombre.Content.ToString(), arteria=1, puesto="OPERARIO(A)"   });
        }
        private void listViewAsistencia_KeyDown(object sender, KeyEventArgs e)
        {
            // eliminar el operario con ctrl y d
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                List<asistencia> items = new List<asistencia>();
                foreach (asistencia item in listViewAsistencia.SelectedItems)
                {
                    items.Add(item);
                }
                foreach (asistencia item in items)
                {
                    listViewAsistencia.Items.Remove(item);
                }
            }
        }
        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            string stringNombres = "Las siguientes personas no tienen modulo: \n \n";
            int conteoVacio = 0;
            foreach(asistencia item in listViewAsistencia.Items)
            {
                if (string.IsNullOrEmpty(item.modulo))
                {
                    conteoVacio = conteoVacio + 1;
                    stringNombres = stringNombres + item.nombre + "\n \n";
                }
            }
            if (conteoVacio > 0)
            {
                MessageBox.Show(stringNombres);
            }
            else
            {
                if(comboBoxAsignado.SelectedIndex==-1|| comboBoxTurnoLista.SelectedIndex==-1 || comboBoxBase.SelectedIndex == -1|| Convert.ToDateTime(calendarAsistencia.SelectedDate).ToString("yyyy-MM-dd")=="0001-01-01")
                {
                    MessageBox.Show("Por Favor seleccione modulo asignado, turno, base y fecha");
                }
                else
                {
                    String sql;
                    SqlCommand cm;
                    cnProduccion.Open();
                    sql = "delete from asistencia where fecha_at='"+ Convert.ToDateTime(calendarAsistencia.SelectedDate).ToString("yyyy-MM-dd")+"' and asignado_at='"+comboBoxAsignado.SelectedItem.ToString()+"' and turno_at='"+comboBoxTurnoLista.SelectedItem.ToString()+"'";
                    cm = new SqlCommand(sql, cnProduccion);
                    cm.ExecuteNonQuery();
                    foreach(asistencia item in listViewAsistencia.Items)
                    {
                        sql = "insert into asistencia(fecha_at, asignado_at, codigo_at, nombre_at, modulo_at, arteria_at, tiempo_at, turno_at, base_at, puesto_at, observaciones_at, movimiento_at) values('"+ Convert.ToDateTime(calendarAsistencia.SelectedDate).ToString("yyyy-MM-dd")+"', '"+comboBoxAsignado.SelectedItem.ToString()+"', '"+item.codigo+"', '"+item.nombre+"', '"+item.modulo+"', '"+item.arteria+"', '"+item.tiempo+"', '"+ comboBoxTurnoLista.SelectedItem.ToString()+"', '"+comboBoxBase.SelectedItem.ToString()+"', '"+item.puesto+"', '"+item.observaciones+"', '"+item.movimiento+"')";
                        cm = new SqlCommand(sql, cnProduccion);
                        cm.ExecuteNonQuery();
                    }
                    cnProduccion.Close();
                    MessageBoxResult result = MessageBox.Show("¿Desea descargar la asistencia guardada?", "Jazz-CCO", MessageBoxButton.YesNo);
                    switch (result)
                    {
                        case MessageBoxResult.Yes:
                            descargarAsistencia();
                            listViewAsistencia.Items.Clear();
                            comboBoxAsignado.SelectedIndex = -1;
                            comboBoxModulo.SelectedIndex = -1;
                            comboBoxTurno.SelectedIndex = -1;
                            comboBoxTurnoLista.SelectedIndex = -1;
                            comboBoxBase.SelectedIndex = -1;
                            calendarAsistencia.SelectedDate = Convert.ToDateTime("0001-01-01");
                            calendarAsistenciaRetractil.SelectedDate = Convert.ToDateTime("0001-01-01");
                            labelNombre.Content = "----";
                            labelFecha.Content = "----";
                            textBoxCodigo.Clear();
                            break;
                        case MessageBoxResult.No:
                            listViewAsistencia.Items.Clear();
                            comboBoxAsignado.SelectedIndex = -1;
                            comboBoxModulo.SelectedIndex = -1;
                            comboBoxTurno.SelectedIndex = -1;
                            comboBoxTurnoLista.SelectedIndex = -1;
                            comboBoxBase.SelectedIndex = -1;
                            calendarAsistencia.SelectedDate = Convert.ToDateTime("0001-01-01");
                            calendarAsistenciaRetractil.SelectedDate = Convert.ToDateTime("0001-01-01");
                            labelNombre.Content = "----";
                            labelFecha.Content = "----";
                            textBoxCodigo.Clear();
                            break;
                    }

                }
            }
        }
        private void descargarAsistencia()
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("FECHA");
            buffer.Append(",");
            buffer.Append("ASIGNADO");
            buffer.Append(",");
            buffer.Append("TURNO");
            buffer.Append(",");
            buffer.Append("BASE");
            buffer.Append(",");
            buffer.Append("CODIGO");
            buffer.Append(",");
            buffer.Append("NOMBRE");
            buffer.Append(",");
            buffer.Append("MODULO");
            buffer.Append(",");
            buffer.Append("ARTERIA");
            buffer.Append(",");
            buffer.Append("HORAS");
            buffer.Append(",");
            buffer.Append("PUESTO");
            buffer.Append(",");
            buffer.Append("OBSERVACIONES");
            buffer.Append(",");
            buffer.Append("MOVIMIENTO");
            buffer.Append("\n");
            #endregion
            foreach (asistencia item in listViewAsistencia.Items)
            {
                buffer.Append(Convert.ToDateTime(calendarAsistencia.SelectedDate).ToString("yyyy-MM-dd"));
                buffer.Append(",");
                buffer.Append(comboBoxAsignado.SelectedItem.ToString());
                buffer.Append(",");
                buffer.Append(comboBoxTurnoLista.SelectedItem.ToString());
                buffer.Append(",");
                buffer.Append(comboBoxBase.SelectedItem.ToString());
                buffer.Append(",");
                buffer.Append(item.codigo);
                buffer.Append(",");
                buffer.Append(item.nombre);
                buffer.Append(",");
                buffer.Append(item.modulo);
                buffer.Append(",");
                buffer.Append(item.arteria);
                buffer.Append(",");
                buffer.Append(item.tiempo);
                buffer.Append(",");
                buffer.Append(item.puesto);
                buffer.Append(",");
                buffer.Append(item.observaciones);
                buffer.Append(",");
                buffer.Append(item.movimiento);
                buffer.Append(",");
                buffer.Append("\n");
            }
            String result = buffer.ToString();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV (*.csv)|*.csv";
                string fileName = "";
                if (saveFileDialog.ShowDialog() == true)
                {
                    fileName = saveFileDialog.FileName;
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine(result);
                    sw.Close();

                }

                Process.Start(fileName);
            }
            catch (Exception)
            { }
        }
        #endregion
    }
}
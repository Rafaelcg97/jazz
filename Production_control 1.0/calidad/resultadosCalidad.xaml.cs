using Microsoft.Win32;
using Production_control_1._0.clases;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
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

namespace Production_control_1._0.calidad
{
    public partial class resultadosCalidad : UserControl
    {
        #region varibalesConexion
        public SqlConnection cnCalidad = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_calidad"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        public resultadosCalidad()
        {
            InitializeComponent();
            calendarCalidad.SelectedDate = DateTime.Now;
        }
        private void letraAjustable(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void calendarCalidad_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            listViewDatosCalidad.Items.Clear();
            listViewDetallesRechazos.Items.Clear();
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear((DateTime)calendarCalidad.SelectedDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = ((DateTime)calendarCalidad.SelectedDate).Year;
            string sql = "select anio, semana, modulo, arteria, muestraFinal, rechazoFinal, aqlFinal, muestraProceso, rechazoProcesos, aqlProcesos, muestraEmpaque, rechazoEmpaque, aqlEmpaque from resumen_calidad where anio=" + anio + " and semana="+semana +" order by modulo";
            cnCalidad.Open();
            SqlCommand cm = new SqlCommand(sql, cnCalidad);
            cm.ExecuteNonQuery();
            cm = new SqlCommand(sql, cnCalidad);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewDatosCalidad.Items.Add(
                    new registroCalidad 
                    {
                        anio=Convert.ToInt32(dr["anio"] is DBNull ? 0 : dr["anio"]), 
                        semana= Convert.ToInt32(dr["semana"] is DBNull ? 0 : dr["semana"]),
                        modulo=dr["modulo"].ToString(), 
                        arteria= Convert.ToInt32(dr["arteria"] is DBNull ? 0 : dr["arteria"]),
                        muestraF= Convert.ToInt32(dr["muestraFinal"] is DBNull ? 0 : dr["muestraFinal"]), 
                        rechazosF= Convert.ToInt32(dr["rechazoFinal"] is DBNull ? 0 : dr["rechazoFinal"]),
                        aqlF= Convert.ToDouble(dr["aqlFinal"] is DBNull ? 0 : dr["aqlFinal"]),
                        muestraP = Convert.ToInt32(dr["muestraProceso"] is DBNull ? 0 : dr["muestraProceso"]),
                        rechazosP = Convert.ToInt32(dr["rechazoProcesos"] is DBNull ? 0 : dr["rechazoProcesos"]),
                        aqlP = Convert.ToDouble(dr["aqlProcesos"] is DBNull ? 0 : dr["aqlProcesos"]),
                        muestraE = Convert.ToInt32(dr["muestraEmpaque"] is DBNull ? 0 : dr["muestraEmpaque"]),
                        rechazosE = Convert.ToInt32(dr["rechazoEmpaque"] is DBNull ? 0 : dr["rechazoEmpaque"]),
                        aqlE = Convert.ToDouble(dr["aqlEmpaque"] is DBNull ? 0 : dr["aqlEmpaque"])
                    });
            }
            cnCalidad.Close();
        }
        private void listViewDatosCalidad_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewDatosCalidad.SelectedIndex > -1)
            {
                listViewDetallesRechazos.Items.Clear();
                registroCalidad item = (registroCalidad)listViewDatosCalidad.SelectedItem;
                int semana = item.semana;
                int anio = item.anio;
                string modulo = item.modulo;
                string sql = "select " +
                    "fecha, " +
                    "Cod_Operario, " +
                    "Cod_Maquina, " +
                    "Cod_Defecto, " +
                    "Muestra_Auditada, " +
                    "Qty_Defectuoso " +
                    "from RegistroConfeccion " +
                    "where Resolucion_Defecto='RECHAZO' and " +
                    "Cod_Proceso='CPR' and " +
                    "year(fecha)='"+anio+"' " +
                    "and DATEPART(week, fecha)='"+semana +"' and " +
                    "Concat('MODULO ',Cod_Estacion)='"+ modulo +"'";
                cnCalidad.Open();
                SqlCommand cm = new SqlCommand(sql, cnCalidad);
                cm.ExecuteNonQuery();
                cm = new SqlCommand(sql, cnCalidad);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewDetallesRechazos.Items.Add(
                        new registroCalidad
                        {
                            fecha = Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"),
                            codigo = dr["Cod_Operario"].ToString(),
                            defecto = dr["Cod_Defecto"].ToString(),
                            muestraP = Convert.ToInt32(dr["Muestra_Auditada"] is DBNull ? 0 : dr["Muestra_Auditada"]),
                            rechazosP = Convert.ToInt32(dr["Qty_defectuoso"] is DBNull ? 0 : dr["Qty_Defectuoso"]),
                        }); ;
                }
                cnCalidad.Close();
            }
        }
        private void buttonDescargar_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("ANIO");
            buffer.Append(",");
            buffer.Append("SEMANA");
            buffer.Append(",");
            buffer.Append("MODULO");
            buffer.Append(",");
            buffer.Append("ARTERIA");
            buffer.Append(",");
            buffer.Append("MUESTRAS PROCESOS");
            buffer.Append(",");
            buffer.Append("RECHAZOS PROCESOS");
            buffer.Append(",");
            buffer.Append("AQL PROCESOS");
            buffer.Append(",");
            buffer.Append("MUESTRAS EMPAQUE");
            buffer.Append(",");
            buffer.Append("RECHAZOS EMPAQUE");
            buffer.Append(",");
            buffer.Append("AQL EMPAQUE");
            buffer.Append(",");
            buffer.Append("MUESTRAS FINAL");
            buffer.Append(",");
            buffer.Append("RECHAZOS FINAL");
            buffer.Append(",");
            buffer.Append("AQL FINAL");
            buffer.Append("\n");
            #endregion
            foreach (registroCalidad item in listViewDatosCalidad.Items)
            {
                buffer.Append(item.anio);
                buffer.Append(",");
                buffer.Append(item.semana);
                buffer.Append(",");
                buffer.Append(item.modulo);
                buffer.Append(",");
                buffer.Append(item.arteria);
                buffer.Append(",");
                buffer.Append(item.muestraP);
                buffer.Append(",");
                buffer.Append(item.rechazosP);
                buffer.Append(",");
                buffer.Append(item.aqlP);
                buffer.Append(",");
                buffer.Append(item.muestraE);
                buffer.Append(",");
                buffer.Append(item.rechazosE);
                buffer.Append(",");
                buffer.Append(item.aqlE);
                buffer.Append(",");
                buffer.Append(item.muestraF);
                buffer.Append(",");
                buffer.Append(item.rechazosF);
                buffer.Append(",");
                buffer.Append(item.aqlF);
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasIniciales.calidad());
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
    }
}

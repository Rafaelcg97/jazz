using Microsoft.Win32;
using Production_control_1._0.clases;
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

namespace Production_control_1._0.calidad
{
    public partial class calidadIndividual : Page
    {
        #region varibalesConexion
        public SqlConnection cnCalidad = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_calidad"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        string fechaInicial = DateTime.Now.ToString("yyyy-MM-dd");
        string fechaFinal= DateTime.Now.ToString("yyyy-MM-dd");
        #endregion
        public calidadIndividual()
        {
            InitializeComponent();
            consultarDatos();
        }
        #region control_general_del_programa()
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            // navegarInicioKanBan
            PagePrincipal pagePrincipal = new PagePrincipal();
            Grid gridInicio = (Grid)pagePrincipal.Content;
            foreach (object objeto in gridInicio.Children)
            {
                if (objeto.GetType() == typeof(Grid))
                {
                    Grid grid = (Grid)objeto;
                    if (grid.Name == "gridListaAreas")
                    {
                        foreach (object objeto2 in grid.Children)
                        {
                            if (objeto2.GetType() == typeof(ListView))
                            {
                                ListView listviewMenu = (ListView)objeto2;
                                listviewMenu.SelectedIndex = 6;
                            }
                        }
                    }
                }
            }
            NavigationService.Navigate(pagePrincipal);
        }
        #endregion
        #region tamanoLetra/solonumerosenTextBox
        private void letraAjustable1(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height / tmp.FontFamily.LineSpacing;
        }
        private void letraAjustable2(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.6 / tmp.FontFamily.LineSpacing;
        }
        private void letraAjustable3(object sender, SizeChangedEventArgs e)
        {
            Control tmp = sender as Control;
            tmp.FontSize = e.NewSize.Height * 0.4 / tmp.FontFamily.LineSpacing;
        }
        #endregion
        private void consultarDatos(int tipoConsulta=0, string fechaInicial_="", string fechaFinal_="", string operarioNombre="", int operarioCodigo=0)
        {
            listViewAqlOperarios.Items.Clear();
            string sql="";

            switch (tipoConsulta)
            {
                    //ningun filtro
                case 0:
                    sql = "SELECT TOP 500 a.fecha_p1, a.operario_p1, b.nombre, a.piezas_p1, a.rechazos_p1, a.aql_f1 FROM [dbo].[aql_i1] a LEFT JOIN produccion.dbo.nomina b on a.operario_p1=b.codigo order by a.fecha_p1";
                    break;
                    //solo filtro de fecha
                case 1:
                    sql = "SELECT a.fecha_p1, a.operario_p1, b.nombre, a.piezas_p1, a.rechazos_p1, a.aql_f1 FROM [dbo].[aql_i1] a " +
                          "LEFT JOIN produccion.dbo.nomina b on a.operario_p1=b.codigo " +
                          "where a.fecha_p1>='" + fechaInicial_ + "' and a.fecha_p1<='" + fechaFinal_ + "' order by a.fecha_p1";
                    break;
                    //solo codigo
                case 2:
                    sql = "SELECT a.fecha_p1, a.operario_p1, b.nombre, a.piezas_p1, a.rechazos_p1, a.aql_f1 FROM [dbo].[aql_i1] a " +
                          "LEFT JOIN produccion.dbo.nomina b on a.operario_p1=b.codigo " +
                          "where a.operario_p1='" + operarioCodigo + "' order by a.fecha_p1";
                    break;
                    //solo nombre
                case 3:
                    sql = "SELECT a.fecha_p1, a.operario_p1, b.nombre, a.piezas_p1, a.rechazos_p1, a.aql_f1 FROM [dbo].[aql_i1] a " +
                          "LEFT JOIN produccion.dbo.nomina b on a.operario_p1=b.codigo " +
                          "where b.nombre LIKE '%" + operarioNombre + "%' order by a.fecha_p1";
                    break;
                    //fecha y nombre
                case 4:
                    sql = "SELECT a.fecha_p1, a.operario_p1, b.nombre, a.piezas_p1, a.rechazos_p1, a.aql_f1 FROM [dbo].[aql_i1] a " +
                          "LEFT JOIN produccion.dbo.nomina b on a.operario_p1=b.codigo " +
                          "where b.nombre LIKE '%" + operarioNombre + "%' and a.fecha_p1>='" + fechaInicial_ + "' and a.fecha_p1<='" + fechaFinal_ + "' order by a.fecha_p1";
                    break;
                    //fecha y codigo
                case 5:
                    sql = "SELECT a.fecha_p1, a.operario_p1, b.nombre, a.piezas_p1, a.rechazos_p1, a.aql_f1 FROM [dbo].[aql_i1] a " +
                          "LEFT JOIN produccion.dbo.nomina b on a.operario_p1=b.codigo " +
                          "where a.operario_p1='" + operarioCodigo + "' and a.fecha_p1>='" + fechaInicial_ + "' and a.fecha_p1<='" + fechaFinal_ + "' order by a.fecha_p1";
                    break;

            }


            SqlCommand cm = new SqlCommand(sql, cnCalidad);
            cnCalidad.Open();
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listViewAqlOperarios.Items.Add(
                    new registroCalidad
                    {
                        fechaDt = Convert.ToDateTime(dr["fecha_p1"]),
                        fecha = Convert.ToDateTime(dr["fecha_p1"]).ToString("yyyy-MM-dd"),
                        codigo = dr["operario_p1"].ToString(),
                        nombre = dr["nombre"].ToString(),
                        muestraP = Convert.ToInt32(dr["piezas_p1"] is DBNull ? 0 : dr["piezas_p1"]),
                        rechazosP = Convert.ToInt32(dr["rechazos_p1"] is DBNull ? 0 : dr["rechazos_p1"]),
                        aqlP = Convert.ToDouble(dr["aql_f1"] is DBNull ? 0 : dr["aql_f1"])
                    });
            }
            cnCalidad.Close();
            indicador.Fill = Brushes.Green;
        }

        private void buttonMostrarPopUp_Click(object sender, RoutedEventArgs e)
        {
            popUpFecha.IsOpen = true;
        }

        private void calendarFecha_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            fechaInicial = calendarFecha.SelectedDates.ToArray().First().ToString("yyyy-MM-dd");
            fechaFinal = calendarFecha.SelectedDates.ToArray().Last().ToString("yyyy-MM-dd");
            labelRangoFechas.Content = fechaInicial + " --> " + fechaFinal;
            indicador.Fill = Brushes.Red;
        }

        private void buttonBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (labelRangoFechas.Content.ToString() == "ALL" && string.IsNullOrWhiteSpace(textBoxBuscar.Text))
            {
                consultarDatos(0);
            }
            else if (labelRangoFechas.Content.ToString() != "ALL" && string.IsNullOrWhiteSpace(textBoxBuscar.Text))
            {
                consultarDatos(1, fechaInicial, fechaFinal);
            }
            else if (labelRangoFechas.Content.ToString() == "ALL" &&  textBoxBuscar.Text.ToString().All(char.IsDigit))
            {
                consultarDatos(2,"","","",Convert.ToInt32(textBoxBuscar.Text));
            }
            else if (labelRangoFechas.Content.ToString() == "ALL" && textBoxBuscar.Text.ToString().All(char.IsDigit)==false)
            {
                consultarDatos(3, "", "", textBoxBuscar.Text,0);
            }
            else if (labelRangoFechas.Content.ToString() != "ALL" && textBoxBuscar.Text.ToString().All(char.IsDigit) == false)
            {
                consultarDatos(4, fechaInicial, fechaFinal, textBoxBuscar.Text, 0);
            }
            else if (labelRangoFechas.Content.ToString() != "ALL" && textBoxBuscar.Text.ToString().All(char.IsDigit))
            {
                consultarDatos(5, fechaInicial, fechaFinal, "", Convert.ToInt32(textBoxBuscar.Text));
            }
        }

        private void textBoxBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            indicador.Fill = Brushes.Red;
        }

        private void buttonDescargarAql_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("FECHA");
            buffer.Append(",");
            buffer.Append("CODIGO");
            buffer.Append(",");
            buffer.Append("NOMBRE");
            buffer.Append(",");
            buffer.Append("AUDITADO");
            buffer.Append(",");
            buffer.Append("RECHAZADO");
            buffer.Append(",");
            buffer.Append("AQL");
            buffer.Append(",");
            buffer.Append("\n");
            #endregion
            foreach (registroCalidad item in listViewAqlOperarios.Items)
            {
                buffer.Append(item.fecha);
                buffer.Append(",");
                buffer.Append(item.codigo);
                buffer.Append(",");
                buffer.Append(item.nombre);
                buffer.Append(",");
                buffer.Append(item.muestraP);
                buffer.Append(",");
                buffer.Append(item.rechazosP);
                buffer.Append(",");
                buffer.Append(item.aqlP);
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
    }
}

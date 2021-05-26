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
using Microsoft.Win32;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasProduccion
{
    public partial class resultadosBuenasPracticas : UserControl
    {
        #region stringsGenerales
        SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        int[] opciones2_ = new int[2];
        int[] opciones3_ = new int[3];
        int[] opciones4_ = new int[4];
        int[] arterias_ = new int[3];
        List<string> modulos_ = new List<string>();
        #endregion
        #region datosInciales
        public resultadosBuenasPracticas()
        {
            InitializeComponent();
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = DateTime.Now.Year;
            opciones2_[0] = 25;
            opciones2_[1] = 100;
            opciones3_[0] = 25;
            opciones3_[1] = 50;
            opciones3_[2] = 100;
            opciones4_[0] = 25;
            opciones4_[1] = 50;
            opciones4_[2] = 75;
            opciones4_[3] = 100;
            arterias_[0] = 1;
            arterias_[1] = 2;
            arterias_[2] = 3;
            string sql = "select num_auditoria, fecha, hora, modulo, arteria, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, observaciones, resultado from buenaspracticas where semana_bpi ='" + semana + "' and ano_bpi ='" + anio + "'";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                listViewResultadosModulo.Items.Add(new bp {fecha=Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"), modulo=dr["modulo"].ToString(), arteria=Convert.ToInt32(dr["arteria"]), arterias = arterias_, modulos = modulos_.ToArray(), opciones2 = opciones2_, opciones3 = opciones3_, opciones4 = opciones4_, p1 =Convert.ToInt32(dr["p1"]), p2 = Convert.ToInt32(dr["p2"]), p3= Convert.ToInt32(dr["p3"]), p4= Convert.ToInt32(dr["p4"]), p5= Convert.ToInt32(dr["p5"]), p6= Convert.ToInt32(dr["p6"]), p7= Convert.ToInt32(dr["p7"]), p8= Convert.ToInt32(dr["p8"]), p9= Convert.ToInt32(dr["p9"]), p10= Convert.ToInt32(dr["p10"]), p11= Convert.ToInt32(dr["p11"]), p12= Convert.ToInt32(dr["p12"]), p13= Convert.ToInt32(dr["p13"]), p14= Convert.ToInt32(dr["p14"]), p15= Convert.ToInt32(dr["p15"]), p16= Convert.ToInt32(dr["p16"]), p17= Convert.ToInt32(dr["p17"]), p18= Convert.ToInt32(dr["p18"]), p19= Convert.ToInt32(dr["p19"]), p20= Convert.ToInt32(dr["p20"]), comentario=dr["observaciones"].ToString(), resultado= Convert.ToDouble(dr["resultado"]), num_auditoria= Convert.ToInt32(dr["num_auditoria"]), });
            };
            //se termina la conexion a la base
            dr.Close();

            sql = "select modulo from modulosProduccion where coordinadorNombre<>'-'";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                modulos_.Add(dr["modulo"].ToString());
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
        }
        #endregion
        #region calculos_generals
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
        private void ButtonRegresar_Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasIniciales.produccion());
        }
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }
        #endregion
        #region controlModificaciones
        private void calendarFecha_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime fecha = Convert.ToDateTime(calendarFecha.SelectedDate);
            int semana = System.Globalization.CultureInfo.CurrentUICulture.Calendar.GetWeekOfYear(fecha, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
            int anio = fecha.Year;
            listViewResultadosModulo.Items.Clear();
            string sql = "select num_auditoria, fecha, hora, modulo, arteria, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19, p20, p21, p22, p23, observaciones, resultado from buenaspracticas where semana_bpi ='" + semana + "' and ano_bpi ='" + anio + "'";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                listViewResultadosModulo.Items.Add(new bp { fecha = Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"), modulo = dr["modulo"].ToString(), arteria = Convert.ToInt32(dr["arteria"]), arterias=arterias_, modulos=modulos_.ToArray(), opciones2=opciones2_, opciones3=opciones3_, opciones4=opciones4_, p1 = Convert.ToInt32(dr["p1"]), p2 = Convert.ToInt32(dr["p2"]), p3 = Convert.ToInt32(dr["p3"]), p4 = Convert.ToInt32(dr["p4"]), p5 = Convert.ToInt32(dr["p5"]), p6 = Convert.ToInt32(dr["p6"]), p7 = Convert.ToInt32(dr["p7"]), p8 = Convert.ToInt32(dr["p8"]), p9 = Convert.ToInt32(dr["p9"]), p10 = Convert.ToInt32(dr["p10"]), p11 = Convert.ToInt32(dr["p11"]), p12 = Convert.ToInt32(dr["p12"]), p13 = Convert.ToInt32(dr["p13"]), p14 = Convert.ToInt32(dr["p14"]), p15 = Convert.ToInt32(dr["p15"]), p16 = Convert.ToInt32(dr["p16"]), p17 = Convert.ToInt32(dr["p17"]), p18 = Convert.ToInt32(dr["p18"]), p19 = Convert.ToInt32(dr["p19"]), p20 = Convert.ToInt32(dr["p20"]), comentario = dr["observaciones"].ToString(), resultado = Convert.ToDouble(dr["resultado"]), num_auditoria = Convert.ToInt32(dr["num_auditoria"]), });
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();

        }
        private void buttonDescargar_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("FECHA");
            buffer.Append(",");
            buffer.Append("MODULO");
            buffer.Append(",");
            buffer.Append("ARTERIA");
            buffer.Append(",");
            buffer.Append("P1");
            buffer.Append(",");
            buffer.Append("P2");
            buffer.Append(",");
            buffer.Append("P3");
            buffer.Append(",");
            buffer.Append("P4");
            buffer.Append(",");
            buffer.Append("P5");
            buffer.Append(",");
            buffer.Append("P6");
            buffer.Append(",");
            buffer.Append("P7");
            buffer.Append(",");
            buffer.Append("P8");
            buffer.Append(",");
            buffer.Append("P9");
            buffer.Append(",");
            buffer.Append("P10");
            buffer.Append(",");
            buffer.Append("P11");
            buffer.Append(",");
            buffer.Append("P12");
            buffer.Append(",");
            buffer.Append("P13");
            buffer.Append(",");
            buffer.Append("P14");
            buffer.Append(",");
            buffer.Append("P15");
            buffer.Append(",");
            buffer.Append("P16");
            buffer.Append(",");
            buffer.Append("P17");
            buffer.Append(",");
            buffer.Append("P18");
            buffer.Append(",");
            buffer.Append("P19");
            buffer.Append(",");
            buffer.Append("P20");
            buffer.Append(",");
            buffer.Append("RESULTADO");
            buffer.Append(",");
            buffer.Append("COMENTARIO");
            buffer.Append("\n");
            #endregion
            foreach (bp item in listViewResultadosModulo.Items)
            {
                buffer.Append(item.fecha);
                buffer.Append(",");
                buffer.Append(item.modulo);
                buffer.Append(",");
                buffer.Append(item.arteria);
                buffer.Append(",");
                buffer.Append(item.p1);
                buffer.Append(",");
                buffer.Append(item.p2);
                buffer.Append(",");
                buffer.Append(item.p3);
                buffer.Append(",");
                buffer.Append(item.p4);
                buffer.Append(",");
                buffer.Append(item.p5);
                buffer.Append(",");
                buffer.Append(item.p6);
                buffer.Append(",");
                buffer.Append(item.p7);
                buffer.Append(",");
                buffer.Append(item.p8);
                buffer.Append(",");
                buffer.Append(item.p9);
                buffer.Append(",");
                buffer.Append(item.p10);
                buffer.Append(",");
                buffer.Append(item.p11);
                buffer.Append(",");
                buffer.Append(item.p12);
                buffer.Append(",");
                buffer.Append(item.p13);
                buffer.Append(",");
                buffer.Append(item.p14);
                buffer.Append(",");
                buffer.Append(item.p15);
                buffer.Append(",");
                buffer.Append(item.p16);
                buffer.Append(",");
                buffer.Append(item.p17);
                buffer.Append(",");
                buffer.Append(item.p18);
                buffer.Append(",");
                buffer.Append(item.p19);
                buffer.Append(",");
                buffer.Append(item.p20);
                buffer.Append(",");
                buffer.Append(item.resultado);
                buffer.Append(",");
                buffer.Append(item.comentario.Replace(",",""));
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
            catch (Exception ex)
            { }
        }
        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            popUpValidarUsuario.IsOpen = true;
            buttonModificar.IsEnabled = false;
            passWordBoxValidarUsuario.Password = "";
        }
        private void buttonDetallesPreguntas_Click(object sender, RoutedEventArgs e)
        {
            string stringDetalles = "1- Los depositos de basura de cada maquina estan libres de hilos, billeteras, botellas de agua u otro elemento que no corresponda en el basurero" + "\n" + "\n";
            stringDetalles = stringDetalles + "2- En los puestos de trabajo existen elementos u objetos que no agregan valor a la operacion" + "\n" + "\n";
            stringDetalles = stringDetalles + "3- Existe exceso de materiales en el modulo" + "\n" + "\n";
            stringDetalles = stringDetalles + "4- Existen piezas, partes, accesorios, hilos o trims de estilos ya finalizados o antiguos en el modulo." + "\n" + "\n";
            stringDetalles = stringDetalles + "5- En el area de Bandeo esta solamente lo necesario para la operacion y correctamente ordenado, jabas con su respectiva identificacion del material" + "\n" + "\n";
            stringDetalles = stringDetalles + "6- Los puestos de trabajo de inspeccion, empaque y transfer se encuentran limpios, ordenados y depurados de documentos obsoletos" + "\n" + "\n";
            stringDetalles = stringDetalles + "7- Pasillos claros y definidos, sin ninguna obstruccion de maquina, caja o mesa de trabajo" + "\n" + "\n";
            stringDetalles = stringDetalles + "8- Los cables de los equipos electricos y de aire comprimido no representan una fuente de peligro" + "\n" + "\n";
            stringDetalles = stringDetalles + "9- Las areas de trabajo se encuentran identificadas, señalizaciones (Banderines) en buenas condiciones y visibles, ademas las areas de peligro señalizadas con su respectiva cinta reflectiva" + "\n" + "\n";
            stringDetalles = stringDetalles + "10- Existe una clasificacion de material para reciclaje en el area de bandeo y en area de preparacion" + "\n" + "\n" + "\n";
            stringDetalles = stringDetalles + "11- Piso del modulo se encuentran limpio, libre de desperdicios de cadenillas, recortes y cajas solas en el suelo" + "\n" + "\n";
            stringDetalles = stringDetalles + "12- Los puestos de trabajo que se estan utilizando se encuentran limpios incluyendo los que no " + "\n" + "\n";
            stringDetalles = stringDetalles + "13- Se respetan las areas definidas para el material en el modulo" + "\n" + "\n";
            stringDetalles = stringDetalles + "14- Se cumple el procedimiento de devolucion de sobrantes de estilo anterior y se utiliza la luz de color rojo del centro del pasillo" + "\n" + "\n";
            stringDetalles = stringDetalles + "15- Se cumple el procedimiento de traslado de PT a Bod. de exportacion, utilizando la luz de color verde y no acumulando mas del 60% en el area" + "\n" + "\n";
            stringDetalles = stringDetalles + "16- Se mantienen actualizadas las caras de la torre del modulo y en su respectivo porta documento" + "\n" + "\n";
            stringDetalles = stringDetalles + "17- Los materiales del modulo estan ordenados y en su respectiva ubicacion" + "\n" + "\n";
            stringDetalles = stringDetalles + "18- Mantienen unicamente los codigos de agujas correspondientes al estilo que se esta costurando" + "\n" + "\n";
            stringDetalles = stringDetalles + "19- Existe un plan de accion para mejorar los puntos de las auditorias anteriores y los resultados son visibles en la torre del modulo y canalizados a todo el equipo de la unidad" + "\n" + "\n";
            stringDetalles = stringDetalles + "20- Los hallazgos de la evaluacion no son recurrentes con respecto a las auditorias anteriores" + "\n" + "\n";

            textBlockDetallesPreguntas.Text = stringDetalles;

            popUpDetallesDePreguntas.IsOpen = true;
        }
        private void ButtonCerrarPopup_Click(object sender, RoutedEventArgs e)
        {
            popUpDetallesDePreguntas.IsOpen = false;
        }
        private void ButtonCerrarPopup2_Click(object sender, RoutedEventArgs e)
        {
            popUpValidarUsuario.IsOpen = false;
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            #region variablesDeConexionn
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion
            #region consultarUsuario
            //consultar
            sql = "select codigo from usuarios where contrasena='" + passWordBoxValidarUsuario.Password + "' and nivel='1' and cargo='LEAN'";
            cnIngenieria.Open();
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            if (dr.Read())
            {
                labelNombreAutoriza.Content = dr["codigo"].ToString();
                buttonModificar.IsEnabled = true;
            }
            else
            {
                labelNombreAutoriza.Content = "----";
                buttonModificar.IsEnabled = false;
            }
            dr.Close();
            cnIngenieria.Close();
            #endregion

        }
        private void buttonModificar_Click(object sender, RoutedEventArgs e)
        {
            cnProduccion.Open();
            foreach(bp item in listViewResultadosModulo.Items)
            {
                string comentario = item.comentario.Replace("'", "");
                string sql = "update buenaspracticas set fecha='" + item.fecha + "', modulo='" + item.modulo + "', arteria='" + item.arteria + "', p1=" + item.p1 + ", p2=" + item.p2 + ", p3=" + item.p3 + ", p4=" + item.p4 + ", p5=" + item.p5 + ", p6=" + item.p6 + ", p7=" + item.p7 + ", p8=" + item.p8 + ", p9=" + item.p9 + ", p10=" + item.p10 + ", p11=" + item.p11 + ", p12=" + item.p12 + ", p13=" + item.p13 + ", p14=" + item.p14 + ", p15=" + item.p15 + ", p16=" + item.p16 + ", p17=" + item.p17 +", p18="+item.p18+", p19="+item.p19+", p20="+item.p20+", observaciones='"+comentario+"' where num_auditoria="+item.num_auditoria ;
                SqlCommand cm = new SqlCommand(sql, cnProduccion);
                cm.ExecuteNonQuery();
            }
            cnProduccion.Close();
            MessageBox.Show("Acción Terminada");
        }
        #endregion

        private void listViewResultadosModulo_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.Configuration;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace Production_control_1._0
{
    public partial class reporteMaquinaMala : UserControl
    {
        #region clases_especiales
        public class elementos
        {
            public string maquina { get; set; }
            public string problema_reportado { get; set; }
            public DateTime hora_reportada { get; set; }
        }

        public class elemento_grafica
        {
            public DateTime fecha { get; set; }
            public double conteo { get; set; }
        }
        #endregion
        #region clases_especiales_para_la_grafica
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        #endregion
        #region datos_iniciales
        public reporteMaquinaMala()
        {
            InitializeComponent();
            comboBoxArteria.Items.Add(1);
            comboBoxArteria.Items.Add(2);
            comboBoxArteria.Items.Add(3);
            //se cargan los datos de las listas de modulos y de problemas
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select modulo from orden_modulos group by modulo";
            string sql2 = "select codigo from inventario_maquinas";
            string sql3 = "select falla from defectos_linea";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                modulo_reporte.Items.Add(dr["modulo"].ToString());
            };
            dr.Close();
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
               maquina_reporte.Items.Add(dr2["codigo"].ToString());
            };
            dr2.Close();
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                problema_reporte.Items.Add(dr3["falla"].ToString());
            };
            dr3.Close();
            cn.Close();

            //habilitar o inhabilitar boton de envio

            habilitar_boton();

            // se cargan los datos iniciales para la grafica
            SeriesCollection = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Reportes",
                    Values = new ChartValues<double> {0},
                    Fill = System.Windows.Media.Brushes.DarkOrange,
                }
            };
            Formatter = value => value.ToString("N");
            DataContext = this;
        }
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasIniciales.mantenimiento());
        }
        #endregion
        #region tamanos_de_letra_/_tipo_de_texto

        private void letra_tamano__total(object sender, SizeChangedEventArgs e)
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
                tmp.FontSize = e.NewSize.Height * 0.15 / tmp.FontFamily.LineSpacing;
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

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
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
        #endregion
        #region controles_formulario_de_enviar
        private void modulo_reporte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //por defecto los reportes se le asignan a mantenimiento
            corresponde_reporte.Content = "MANTENIMIENTO";
            //se consulta si ha avido un cambio y se consultan los ultimos problemas reportados
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql2 = "select top 25 maquina, problema_reportado, hora_reportada from solicitudes where modulo='" + modulo_reporte.SelectedItem.ToString() + "' order by hora_reportada desc";
            String sql3 = "select id from orden_modulos where modulo='"+ modulo_reporte.SelectedItem.ToString() +"'";
            cn.Open();
            if (comboBoxArteria.SelectedIndex > -1 && modulo_reporte.SelectedIndex > -1)
            {
                string sql = "select TOP 1 hora_cierre from solicitudes where problema_reportado='CAMBIO' AND modulo='" + modulo_reporte.SelectedItem.ToString() + "'  AND arteria='" + comboBoxArteria.SelectedItem.ToString() + "' ORDER BY hora_cierre DESC";
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                //si hay cambios cerrados hace menos de 2 horas se ke asigna el reporte a SMED

                if (dr.Read())
                {
                    DateTime horaCierre = Convert.ToDateTime(dr["hora_cierre"]);
                    DateTime ahora = DateTime.Now;
                    double diferenciaHoras= (ahora - horaCierre).TotalDays * 24;

                    //validar si el dia de cierre del ultimo cambio es igual al dia del reporte
                    if (ahora.Date == horaCierre.Date)
                    {
                        if (diferenciaHoras <= 2)
                        {
                            corresponde_reporte.Content = "SMED";
                        }
                        else
                        {
                            corresponde_reporte.Content = "MANTENIMIENTO";
                        }
                    }
                    //si los dias son diferentes validar si el cambio fue viernes
                    else
                    {
                        //si el cambio fue viernes quitar el tiempo de fin de semana
                        if (horaCierre.DayOfWeek == DayOfWeek.Friday)
                        {
                            if (diferenciaHoras - 62.7 <= 2)
                            {
                                corresponde_reporte.Content = "SMED";
                            }
                            else
                            {
                                corresponde_reporte.Content = "MANTENIMIENTO";
                            }
                        }
                        else
                        {
                            if (diferenciaHoras - 14.7 <= 2)
                            {
                                corresponde_reporte.Content = "SMED";
                            }
                            else
                            {
                                corresponde_reporte.Content = "MANTENIMIENTO";
                            }
                        }
                    }

                }
                else
                {
                    corresponde_reporte.Content = "MANTENIMIENTO";
                };
                dr.Close();
            }

            //se agregan los ultimos problemas del modulo
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            SqlDataReader dr2 = cm2.ExecuteReader();
            List<elementos> lista_reportes = new List<elementos>();
            while (dr2.Read())
            {
                lista_reportes.Add(new elementos { problema_reportado=dr2["problema_reportado"].ToString(), hora_reportada=Convert.ToDateTime(dr2["hora_reportada"]), maquina=dr2["maquina"].ToString() });
            };
            dr2.Close();

            //agregar ubicacion
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            SqlDataReader dr3 = cm3.ExecuteReader();
            dr3.Read();
            labelUbicacion.Content = dr3["id"].ToString();
            dr3.Close();
            cn.Close();
            problemas_modulo.ItemsSource = lista_reportes;
            //habilitar o inhabilitar boton de envio
            habilitar_boton();
        }
        private void comboBoxArteria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            if (comboBoxArteria.SelectedIndex > -1 && modulo_reporte.SelectedIndex > -1)
            {
                cn.Open();
                string sql = "select TOP 1 hora_cierre from solicitudes where problema_reportado='CAMBIO' AND modulo='" + modulo_reporte.SelectedItem.ToString() + "'  AND arteria='" + comboBoxArteria.SelectedItem.ToString() + "' ORDER BY hora_cierre DESC";
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                //si hay cambios cerrados hace menos de 2 horas se ke asigna el reporte a SMED

                if (dr.Read())
                {
                    DateTime horaCierre = Convert.ToDateTime(dr["hora_cierre"]);
                    DateTime ahora = DateTime.Now;
                    double diferenciaHoras = (ahora - horaCierre).TotalDays * 24;
                    //validar si el dia de cierre del ultimo cambio es igual al dia del reporte
                    if (ahora.Date == horaCierre.Date)
                    {
                        if (diferenciaHoras<=2)
                        {
                            corresponde_reporte.Content = "SMED";
                        }
                        else
                        {
                            corresponde_reporte.Content = "MANTENIMIENTO";
                        }
                    }
                    //si los dias son diferentes validar si el cambio fue viernes
                    else
                    {
                        //si el cambio fue viernes quitar el tiempo de fin de semana
                        if (horaCierre.DayOfWeek==DayOfWeek.Friday)
                        {
                            if (diferenciaHoras - 62.7 <= 2)
                            {
                                corresponde_reporte.Content = "SMED";
                            }
                            else
                            {
                                corresponde_reporte.Content = "MANTENIMIENTO";
                            }
                        }
                        else
                        {
                            if (diferenciaHoras - 14.7 <= 2)
                            {
                                corresponde_reporte.Content = "SMED";
                            }
                            else
                            {
                                corresponde_reporte.Content = "MANTENIMIENTO";
                            }
                        }
                    }

                }
                else
                {
                    corresponde_reporte.Content = "MANTENIMIENTO";
                };
                dr.Close();
                cn.Close();
                habilitar_boton();
            }
        }
        private void buscar_maquina_Reporte_TextChanged(object sender, TextChangedEventArgs e)
        {
            //se limpian los items cargados en la lista de maquinas
            maquina_reporte.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select codigo from inventario_maquinas where codigo like '%" + buscar_maquina_Reporte.Text.ToString() + "%'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                maquina_reporte.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();
            cn.Close();

            //habilitar o inhabilitar boton de envio
            habilitar_boton();

            //limpiar grafica
            grafico.AxisX.Clear();
            SeriesCollection[0].Values.Clear();
        }
        private void buscar_problema_reporte_TextChanged(object sender, TextChangedEventArgs e)
        {
            //se limpian los items cargados en la lista de maquinas
            problema_reporte.Items.Clear();
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select falla from defectos_linea where falla like '%" + buscar_problema_reporte.Text.ToString() +"%'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                problema_reporte.Items.Add(dr["falla"].ToString());
            };
            dr.Close();
            cn.Close();
            //habilitar o inhabilitar boton de envio
            habilitar_boton();
        }
        private void maquina_reporte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tipo_maquina.Content = "----";
            categoria.Content = "----";
            marca.Content = "----";
            if (maquina_reporte.SelectedIndex> -1)
            {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select * from inventario_maquinas where codigo= '" + maquina_reporte.SelectedItem.ToString() +"'";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tipo_maquina.Content = dr["tipo"].ToString();
                categoria.Content = dr["clase"].ToString();
                marca.Content = dr["marca"].ToString();
            };
            dr.Close();
            cn.Close();
            }

            //habilitar o inhabilitar boton de envio
            habilitar_boton();


            actualizar_grafica();
        }
        private void codigo_reporte_TextChanged(object sender, TextChangedEventArgs e)
        {
            operario.Content = "----";
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select top 1 nombre_at from asistencia where codigo_at= '" + codigo_reporte.Text.ToString() + "'";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    operario.Content = dr["nombre_at"].ToString();
                };
                dr.Close();
                cn.Close();

            //habilitar o inhabilitar boton de envio
            habilitar_boton();
        }
        private void problema_reporte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //habilitar o inhabilitar boton de envio
            habilitar_boton();
        }
        private void enviar_reporte_Click(object sender, RoutedEventArgs e)
        {
            string prioridad_ = "balance";
            if (checkBoxPrioridad.IsChecked == true)
            {
                prioridad_ = "principal";
            }
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "insert into solicitudes (modulo, arteria, ubicacion, maquina, operario, problema_reportado, hora_reportada, corresponde, prioridad)  values('" + modulo_reporte.SelectedItem.ToString() + "', '" + comboBoxArteria.SelectedItem.ToString()+"', '" + labelUbicacion.Content.ToString() +"', '"+ maquina_reporte.SelectedItem.ToString() + "', '" + codigo_reporte.Text.ToString() + "', '" + problema_reporte.SelectedItem.ToString() + "', '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "', '" + corresponde_reporte.Content.ToString() + "', '" + prioridad_ + "')";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            cn.Close();
            Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            GridPrincipal.Content = new estadoPlantaProduccion();
        }
        #endregion
        #region calculos_generales
        private void habilitar_boton()
        {
            Uri habilitado = new Uri("/imagenes/flecha.png", UriKind.RelativeOrAbsolute);
            Uri inhabilitado = new Uri("/imagenes/flecha_in.png", UriKind.RelativeOrAbsolute);

            if (modulo_reporte.SelectedIndex>-1 & comboBoxArteria.SelectedIndex>-1 & maquina_reporte.SelectedIndex>-1 & problema_reporte.SelectedIndex>-1 & codigo_reporte.Text.ToString() != "" )
            {
                enviar_reporte.IsEnabled = true;
                img_enviar.Source = new BitmapImage(habilitado);
            }
            else
            {
                enviar_reporte.IsEnabled = false;
                img_enviar.Source = new BitmapImage(inhabilitado);
            }
        }
        private void actualizar_grafica()
        {
            if (maquina_reporte.SelectedIndex > -1)
            {
                List<string> lista_fechas = new List<string>();
                List<elemento_grafica> problemas = new List<elemento_grafica>();
                lista_fechas.Clear();
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select CONVERT(date, hora_reportada) as fecha, COUNT(id_solicitud) as conteo from solicitudes where maquina = '" + maquina_reporte.SelectedItem.ToString() + "'  and CONVERT(date, hora_reportada)> getdate()-100 group by CONVERT(date, hora_reportada) order by fecha desc";
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lista_fechas.Add(Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"));
                    problemas.Add(new elemento_grafica() { conteo = Convert.ToDouble(dr["conteo"]) });
                };
                dr.Close();
                cn.Close();

                grafico.AxisX.Clear();
                SeriesCollection[0].Values.Clear();
                grafico.AxisX.Add(new Axis() { Labels = lista_fechas.ToArray(), LabelsRotation = 45, ShowLabels = true, Separator = { Step = 1 }, });

                foreach (elemento_grafica item in problemas)
                {

                    SeriesCollection[0].Values.Add(item.conteo);
                }
            }
        }
        #endregion
    }
}

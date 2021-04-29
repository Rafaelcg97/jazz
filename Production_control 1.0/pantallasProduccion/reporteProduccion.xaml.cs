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
        public string[] _motivos = new string[8];

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
            sql = "select modulo from modulosProduccion where coordinador<>'-'";
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
            buttonGuardar.IsEnabled = false;
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
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key==Key.Tab)
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
            labelTurno.Content = "Extra";

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
                labelTurno.Content = "Nocturno";
            }
            else
            {
                imageTurno.Source = new BitmapImage(sol);
                labelTurno.Content = "Diurno";
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
            #region agregarMotivos
            _motivos[0] = "-";
            _motivos[1] = "Apagón";
            _motivos[2] = "Falta de Accesorios";
            _motivos[3] = "Capacitación";
            _motivos[4] = "Sublimado";
            _motivos[5] = "Máquina Mala";
            _motivos[6] = "Reunión";
            _motivos[7] = "Falta de Tela";
            #endregion
            //agregar registro uno de lote
            if (listBoxLote.SelectedIndex > -1)
            {
                listViewLotes.Items.Add(new horaProduccion { lote = listBoxLote.SelectedItem.ToString(), piezas = piezasLote, terminadas = piezasReportadas, motivoParo="-", motivos=_motivos });
                calculosLotes();
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

        #region calculosGenerales
        private void calculosLotes()
        {
            List<horaProduccion> listaCalculada = new List<horaProduccion>();
            double minutosLote = 0;
            int conteoLote = 0;
            double minutosHora = 0;
            double minutosHoraEfectivos = 0;
            string _color = "Transparent";
            int piezasLote = 0;
            int totalPiezasTerminadas = 0;
            double minutosParoHora = 0;
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
                else
                {
                    _sam = 0;
                }
                dr.Close();
                cnProduccion.Close();
                #endregion
                conteoLote = conteoLote + 1;
                piezasLote= item.xxs + item.xs + item.s + item.m + item.l + item.xl + item.xxl + item.xxxl;
                totalPiezasTerminadas = item.terminadas + piezasLote;
                minutosLote = _sam * piezasLote;
                minutosHora = minutosHora + minutosLote;
                minutosParoHora = minutosParoHora + item.tiempoParo;
                listaCalculada.Add(new horaProduccion {lote=item.lote, codigo=item.codigo, piezas=item.piezas, terminadas=totalPiezasTerminadas, minutosEfectivos=minutosLote, motivos=item.motivos, tiempoParo=item.tiempoParo, motivoParo=item.motivoParo, sam=_sam, xxs=item.xxs, xs=item.xs, s=item.s, m=item.m, l=item.l, xl=item.xl, xxl=item.xxl, xxxl=item.xxxl});
            }
            listViewPiezas.Items.Clear();
            foreach(horaProduccion item2 in listaCalculada)
            {
                if (minutosHora > 0) { minutosLote = (60-minutosParoHora) * (item2.minutosEfectivos / minutosHora); } else {  minutosLote= ((60 - minutosParoHora) / conteoLote); };
                if (item2.terminadas > item2.piezas) { _color = "Red"; } else { _color = "Transparent"; }
                minutosHoraEfectivos = minutosHoraEfectivos + minutosLote;
                listViewPiezas.Items.Add(new horaProduccion {lote = item2.lote, colorLote=_color, codigo=item2.codigo, tiempoParo=item2.tiempoParo, motivoParo=item2.motivoParo, motivos=item2.motivos, piezas = item2.piezas, terminadas = item2.terminadas, minutosEfectivos = minutosLote, sam=item2.sam, xxs=item2.xxs, xs=item2.xs, s=item2.s, m=item2.m, l=item2.l, xl=item2.xl, xxl=item2.xxl, xxxl=item2.xxxl });
            }
            labelMinutos.Content =Math.Round(minutosHoraEfectivos,2);
        }
        private void verificarImagen(int codigo)
        {
            #region consultaSAM
            string sql;
            string temporada = "";
            SqlCommand cm;
            SqlDataReader dr;
            //consultar
            sql = "select temporada, estilo, empaque, descripcion from sam ";
            sql = sql + "left join ingenieria.dbo.empaques on sam.CLIENTE=empaques.cliente and sam.EMPAQUE=empaques.tipo_empaque and sam.TIPO=empaques.tipo ";
            sql = sql + "where CODIGO=" + codigo;
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            if (dr.Read())
            {
                labelEstilo.Content = dr["estilo"].ToString();
                temporada = dr["temporada"].ToString();
                labelEmpaque.Content = dr["empaque"].ToString();
                textBlockElementos.Text = dr["descripcion"].ToString();
            }
            else
            {
                labelEstilo.Content ="No Existe";
                temporada = "----";
                labelEmpaque.Content = "----";
                textBlockElementos.Text = "----";
            }
            dr.Close();
            cnProduccion.Close();
            #endregion
            try
            {
                Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + temporada + "/" + labelEstilo.Content.ToString() + ".jpg");
                imageEsti.Source = new BitmapImage(fileUri);
            }

            // si no encuentra la imagen del estilo carga la imagen inicial
            catch
            {
                Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                imageEsti.Source = new BitmapImage(fileUri);
            }

        }
        private void LostFocusElemento(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            calculosLotes();
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Clear();
                textBox.Text = "0";
            }
            calculosLotes();
        }
        private void LostFocusElementoCompleto(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Clear();
                textBox.Text = "0";
            }
            else
            {
                verificarImagen(Convert.ToInt32(textBox.Text));
            }
            calculosLotes();
        }
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "0")
            {
                textBox.Clear();
            }
            else
            {

            }

        }
        private void listViewLotes_KeyDown(object sender, KeyEventArgs e)
        {
            // eliminar el repuesto solicitado al presionar la tecla d
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                int elementoSeleccionado = listViewLotes.SelectedIndex + 1;
                List<horaProduccion> items = new List<horaProduccion>();
                int conteo = 0;
                foreach (horaProduccion item in listViewLotes.Items)
                {
                    conteo = conteo + 1;
                    if (conteo == elementoSeleccionado)
                    {
                    }
                    else
                    {
                        items.Add(new horaProduccion { lote = item.lote, codigo = item.codigo, piezas=item.piezas, terminadas=item.terminadas, tiempoParo = item.tiempoParo, motivoParo = item.motivoParo, motivos=item.motivos, xxs = item.xxs, xs = item.xs, s = item.s, m = item.m, l = item.l, xl = item.xl, xxl = item.xxl, xxxl = item.xxxl });
                    }
                }

                listViewLotes.Items.Clear();
                foreach (horaProduccion item2 in items)
                {
                    listViewLotes.Items.Add(new horaProduccion { lote = item2.lote, codigo = item2.codigo, piezas=item2.piezas, terminadas=item2.terminadas, tiempoParo = item2.tiempoParo, motivoParo = item2.motivoParo, motivos=item2.motivos, xxs = item2.xxs, xs = item2.xs, s = item2.s, m = item2.m, l = item2.l, xl = item2.xl, xxl = item2.xxl, xxxl = item2.xxxl });
                }

                calculosLotes();
            }
        }
        #endregion

        #region ingresoDatosHora
        private void passwordBoxIngreso_PasswordChanged(object sender, RoutedEventArgs e)
        {
            #region consultaIngenier
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            //consultar
            sql = "select codigo from usuarios where produccion=1 and contrasena='"+ passwordBoxIngreso.Password +"'";
            cnIngenieria.Open();
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            if (dr.Read())
            {
                labelIngen.Content = dr["codigo"].ToString();
                buttonGuardar.IsEnabled = true;
            }
            else
            {
                labelIngen.Content = "----";
                buttonGuardar.IsEnabled = false;
            }
            dr.Close();
            cnIngenieria.Close();
            #endregion

        }
        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            int operariosMaquina = 0;
            int operariosManuqles = 0;
            if (string.IsNullOrEmpty(TextBoxCostura.Text)) { operariosMaquina = 0; } else { operariosMaquina = Convert.ToInt32(TextBoxCostura.Text); }
            if (string.IsNullOrEmpty(TextBoxManuales.Text)) { operariosManuqles = 0; } else { operariosManuqles = Convert.ToInt32(TextBoxManuales.Text); }
            int totalOperarios = operariosMaquina + operariosMaquina;
            if(comboBoxModulo.SelectedIndex<0 || comboBoxArteria.SelectedIndex<0 || comboBoxHora.SelectedIndex<0 || totalOperarios <= 0)
            {
                MessageBox.Show("!Ups Parece que no haz ingresado algunos datos Importantes");
            }
            else
            {
                string sql;
                SqlCommand cm;
                string fecha = labelFecha.Content.ToString();
                string turno = labelTurno.Content.ToString();
                int hora = Convert.ToInt32(comboBoxHora.SelectedItem);
                string modulo = comboBoxModulo.SelectedItem.ToString();
                int arteria = Convert.ToInt32(comboBoxArteria.SelectedItem);
                string coordinador = labelCoordinador.Content.ToString();
                int incapacitados = Convert.ToInt32(string.IsNullOrEmpty(TextBoxIncapacitado.Text) ? "0" : TextBoxIncapacitado.Text);
                int permisos = Convert.ToInt32(string.IsNullOrEmpty(TextBoxPermisos.Text) ? "0" : TextBoxPermisos.Text);
                int cita = Convert.ToInt32(string.IsNullOrEmpty(TextBoxCita.Text) ? "0" : TextBoxCita.Text);
                int inasistencia = Convert.ToInt32(string.IsNullOrEmpty(TextBoxInasistencia.Text) ? "0" : TextBoxInasistencia.Text);
                double costura = Convert.ToDouble(string.IsNullOrEmpty(TextBoxCostura.Text) ? "0" : TextBoxCostura.Text);
                double manuales = Convert.ToDouble(string.IsNullOrEmpty(TextBoxManuales.Text) ? "0" : TextBoxManuales.Text);

                string custom_ = "No";
                string cambioEstilo = "No";
                if (checkBoxCustom.IsChecked == true) { custom_ = "Si"; }
                if(checkBoxCambio.IsChecked == true) { cambioEstilo = "Si"; }
                //consultar
                cnProduccion.Open();
                foreach (horaProduccion item in listViewPiezas.Items)
                {
                    if(item.colorLote != "Red")
                    {
                        sql = "insert into horahora(Fecha, Turno, Hora, Modulo, Arterias, Coordinador, [Cod Info Estilo], SAM, Incapacitados, Permisos, [Cita ISSS], Inasistencia, [Ope Costura], [Ope Manuales], Lote, [2XS], XS,S, M, L, XL, [2XL], [3XL], [Tiempo de Paro], [Motivo de Paro], [Custom], [Minutos efectivos], [Cambio de Estilo], ingresadoPor) ";
                        sql = sql + "values('" + fecha + "', '" + turno + "', " + hora + ", '" + modulo + "', " + arteria + ", '" + coordinador + "', " + item.codigo + ", ";
                        sql = sql + item.sam + ", " + incapacitados + ", " + permisos + ", " + cita + ", " + inasistencia + ", " + costura + ", " + manuales + ", '" + item.lote + "', '" + item.xxs + "', '" + item.xs + "', '" + item.s + "', '" + item.m + "', '" + item.l + "', '" + item.xl + "', '" + item.xxl + "', '" + item.xxxl + "', "+ item.tiempoParo +", '";
                        sql = sql + item.motivoParo + "', '" + custom_ + "', " + item.minutosEfectivos + ", '" + cambioEstilo + "', '"+ labelIngen.Content.ToString() + "')";
                        cm = new SqlCommand(sql, cnProduccion);
                        cm.ExecuteNonQuery();
                    }
                }
                cnProduccion.Close();

                PagePrincipal pagePrincipal = new PagePrincipal();
                NavigationService.Navigate(pagePrincipal);
            }
        }
        #endregion
    }

}




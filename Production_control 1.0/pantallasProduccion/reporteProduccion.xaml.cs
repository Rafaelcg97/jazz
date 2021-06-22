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
        List<lote> listaDeLotes = new List<lote>();
        public string[] _motivos = new string[9];
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
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
            this.NavigationService.GoBack();
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
            if (comboBoxModulo.SelectedIndex > -1)
            {
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                string arteria = "1";
                if (comboBoxArteria.SelectedIndex > -1)
                {
                    arteria = comboBoxArteria.SelectedItem.ToString();
                }
                else
                {
                    comboBoxArteria.SelectedIndex = 0;
                    arteria = "1";
                }

                //colocar coordinador
                cnProduccion.Open();
                sql = "select coordinadorNombre from modulosProduccion where modulo='" + comboBoxModulo.SelectedItem.ToString() + "'";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                dr.Read();
                labelCoordinador.Content = dr["coordinadorNombre"].ToString();
                dr.Close();
                cnProduccion.Close();

                //colocar datosHoraAnterior
                cnProduccion.Open();
                sql = "select top 1 Turno, Fecha, Hora, Incapacitados, Permisos, [Cita ISSS] as cita, Inasistencia, [Ope Costura] as costura, [Ope Manuales] as manuales from horahora where Modulo='"+comboBoxModulo.SelectedItem.ToString() +"' and arterias='"+arteria+"' order by Fecha desc, hora desc";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    if (Convert.ToDateTime(dr["Fecha"]).ToString("yyyy-MM-dd") == DateTime.Now.ToString("yyyy-MM-dd") && Convert.ToInt32(dr["Hora"] is DBNull? 0: dr["Hora"]) < 12)
                    {
                        comboBoxHora.SelectedIndex = Convert.ToInt32(dr["Hora"] is DBNull ? 0 : dr["Hora"]);
                    }
                    else
                    {
                        comboBoxHora.SelectedItem = "1";
                    }
                    TextBoxCostura.Text = dr["costura"].ToString();
                    TextBoxManuales.Text = dr["manuales"].ToString();
                    TextBoxIncapacitado.Text = dr["Incapacitados"].ToString();
                    TextBoxPermisos.Text = dr["Permisos"].ToString();
                    TextBoxCita.Text = dr["cita"].ToString();
                    TextBoxInasistencia.Text = dr["Inasistencia"].ToString();
                    dr.Close();
                }
                else
                {
                    comboBoxHora.SelectedItem = "1";
                    TextBoxCostura.Text="";
                    TextBoxManuales.Text = "";
                    TextBoxIncapacitado.Text = "";
                    TextBoxPermisos.Text = "";
                    TextBoxCita.Text = "";
                    TextBoxInasistencia.Text = "";
                    dr.Close();
                }
                cnProduccion.Close();
            }
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
        private void listViewLotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewLotes.SelectedIndex > -1)
            {
                horaProduccion item = (horaProduccion)listViewLotes.SelectedItem;
                labelEstilo.Content = item.estilo;
                labelEmpaque.Content = item.empaque;
                textBlockElementos.Text = item.descripcion;
                try
                {
                    Uri fileUri = new Uri(ConfigurationManager.AppSettings["imagenes"] + item.temporada + "/" + item.estilo + ".jpg");
                    imageEsti.Source = new BitmapImage(fileUri);
                }

                // si no encuentra la imagen del estilo carga la imagen inicial
                catch
                {
                    Uri fileUri = new Uri("/imagenes/ini.jpg", UriKind.RelativeOrAbsolute);
                    imageEsti.Source = new BitmapImage(fileUri);
                }
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
            //listBoxLote.Items.Clear();
            listaDeLotes.Clear();
            textBoxLote.Clear();
            listBoxLote.Items.Clear();
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            //llenar lista de lotes
            cnIngenieria.Open();
            sql = "select cliente, lote, estilo, tipo, piezas, temporada, sam from lotesconSam where sam is not null";
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listaDeLotes.Add( new lote { numeroLote = dr["lote"].ToString(), estilo = dr["estilo"].ToString(), temporada = dr["temporada"].ToString(), sam = Convert.ToDouble(dr["sam"] is DBNull ? 0: dr["sam"] ), piezas=Convert.ToInt32(dr["piezas"]), cliente=dr["cliente"].ToString(), tipo=dr["tipo"].ToString()});
            };
            dr.Close();
            cnIngenieria.Close();
            foreach (lote lote in listaDeLotes)
            {
                listBoxLote.Items.Add(lote);
            }
            popUpLote.IsOpen = true;
        }
        private void textBoxLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxLote.Text.Trim()) == false)
            {
                listBoxLote.Items.Clear();
                foreach (lote lote in listaDeLotes)
                {
                    if (lote.numeroLote.StartsWith(textBoxLote.Text.Trim()))

                    {
                        listBoxLote.Items.Add(lote);
                    }
                }
            }

            else if (textBoxLote.Text.Trim() == "")
            {
                listBoxLote.Items.Clear();

                foreach (lote lote in listaDeLotes)
                {
                    listBoxLote.Items.Add(lote);
                }
            }
            listBoxEmpaqueLo.Items.Clear();
            popUpLabelEstilo.Content = "----";
            popUpLabelSamE.Content = "0";
            popUpLabelSamO.Content="0";
            popUpLabelSamT.Content = "0";
            popUpTextBlockEmpaque.Text = "----";
            popUpLabelTemporada.Content = "----";
        }
        private void ButtonAgregarLoteSeleccionado_Click(object sender, RoutedEventArgs e)
        {
            #region agregarMotivos
            _motivos[0] = "-";
            _motivos[1] = "Apagón";
            _motivos[2] = "Falta de Accesorios";
            _motivos[3] = "Capacitación";
            _motivos[4] = "Sublimado";
            _motivos[5] = "Máquina Mala";
            _motivos[6] = "Reunión";
            _motivos[7] = "Reproceso";
            _motivos[8] = "Falta de Tela";
            #endregion
            //agregar registro uno de lote
            if (listBoxLote.SelectedIndex > -1 && listBoxEmpaqueLo.SelectedIndex>-1)
            {
                lote item = (lote)listBoxLote.SelectedItem;
                lote item2 = (lote)listBoxEmpaqueLo.SelectedItem;
                listViewLotes.Items.Add(new horaProduccion { lote = item.numeroLote, estilo=item.estilo, temporada=item.temporada, piezas = item.piezas, terminadas = piezasReportadas, motivoParo = "-", motivos = _motivos, sam=Convert.ToDouble(popUpLabelSamT.Content), empaque=item2.empaque, descripcion=item2.descripcion });
                calculosLotes();
            }
            else
            {
                MessageBox.Show("No seleccionaste ningun lote o su empaque");
            }
        }
        private void listBoxLote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                lote item = (lote)listBoxLote.SelectedItem;
                popUpLabelEstilo.Content = item.estilo;
                popUpLabelTemporada.Content = item.temporada;
                popUpLabelSamO.Content = Math.Round(item.sam,4);
                popUpLabelSamE.Content = "0";
                popUpTextBlockEmpaque.Text = "----";
                popUpLabelSamT.Content = Math.Round(item.sam+ Convert.ToDouble(popUpLabelSamE.Content), 4);
                string sql;
                SqlCommand cm;
                SqlDataReader dr;
                //llenar lista de lotes
                cnProduccion.Open();
                sql = "select sum(totalDePiezas) as totalDePiezas from horahora where lote='" + item.numeroLote + "'";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                dr.Read();
                piezasReportadas = Convert.ToInt32(dr["totalDePiezas"] is DBNull ? 0 : dr["totalDePiezas"]);
                dr.Close();
                cnProduccion.Close();

                //llenar empaques
                listBoxEmpaqueLo.Items.Clear();
                cnIngenieria.Open();
                sql = "select tipo_empaque, descripcion, sam from empaques where cliente='" + item.cliente+"' and temporada='"+item.temporada+"' and tipo='"+item.tipo+"'";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listBoxEmpaqueLo.Items.Add(new lote { empaque = dr["tipo_empaque"].ToString(), samEmpaque = Convert.ToDouble(dr["sam"]), descripcion=dr["descripcion"].ToString() });
                }
                dr.Close();
                cnIngenieria.Close();
            }
        }
        private void listBoxEmpaqueLo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxEmpaqueLo.SelectedIndex > -1)
            {
                lote item = (lote)listBoxEmpaqueLo.SelectedItem;
                popUpLabelSamE.Content = Math.Round(item.samEmpaque, 4);
                popUpLabelSamT.Content = Math.Round(item.samEmpaque + Convert.ToDouble(popUpLabelSamO.Content), 4);
                popUpTextBlockEmpaque.Text = item.descripcion;
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
                 _sam = item.sam;
                conteoLote = conteoLote + 1;
                piezasLote= item.xxs + item.xs + item.s + item.m + item.l + item.xl + item.xxl + item.xxxl;
                totalPiezasTerminadas = item.terminadas + piezasLote;
                minutosLote = _sam * piezasLote;
                minutosHora = minutosHora + minutosLote;
                minutosParoHora = minutosParoHora + item.tiempoParo;
                listaCalculada.Add(new horaProduccion {lote=item.lote, estilo=item.estilo, temporada=item.temporada, empaque=item.empaque, descripcion=item.descripcion, piezas=item.piezas, terminadas=totalPiezasTerminadas, minutosEfectivos=minutosLote, motivos=item.motivos, tiempoParo=item.tiempoParo, motivoParo=item.motivoParo, sam=_sam, xxs=item.xxs, xs=item.xs, s=item.s, m=item.m, l=item.l, xl=item.xl, xxl=item.xxl, xxxl=item.xxxl});
            }
            listViewPiezas.Items.Clear();
            foreach(horaProduccion item2 in listaCalculada)
            {
                if (minutosHora > 0) { minutosLote = (60-minutosParoHora) * (item2.minutosEfectivos / minutosHora); } else {  minutosLote= ((60 - minutosParoHora) / conteoLote); };
                if (item2.terminadas > item2.piezas) { _color = "Red"; } else { _color = "Transparent"; }
                minutosHoraEfectivos = minutosHoraEfectivos + minutosLote;
                listViewPiezas.Items.Add(new horaProduccion {lote = item2.lote, colorLote=_color, estilo=item2.estilo, empaque=item2.empaque, descripcion=item2.descripcion, temporada=item2.temporada, tiempoParo=item2.tiempoParo, motivoParo=item2.motivoParo, motivos=item2.motivos, piezas = item2.piezas, terminadas = item2.terminadas, minutosEfectivos = minutosLote, sam=item2.sam, xxs=item2.xxs, xs=item2.xs, s=item2.s, m=item2.m, l=item2.l, xl=item2.xl, xxl=item2.xxl, xxxl=item2.xxxl });
            }
            labelMinutos.Content =Math.Round(minutosHoraEfectivos,2);
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
            // eliminar el lote con ctrl y d
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
                        items.Add(item);
                    }
                }

                listViewLotes.Items.Clear();
                foreach (horaProduccion item2 in items)
                {
                    listViewLotes.Items.Add(item2);
                }

                calculosLotes();
            }
        }
        #endregion
        #region ingresoDatosHora
        private void passwordBoxIngreso_PasswordChanged(object sender, RoutedEventArgs e)
        {
            buttonGuardar.IsEnabled = false;
            comboBoxModulo.Items.Clear();
            #region variablesDeConexionn
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion
            #region consultarUsuario
            //consultar
            sql = "select modulosProduccion.modulo as modulo, usuarios.codigo as codigo from modulosProduccion left join ingenieria.dbo.usuarios on ";
            sql = sql + "modulosProduccion.ingenieroProcesosCodigo= usuarios.codigo or modulosProduccion.coordinadorCodigo= usuarios.codigo ";
            sql = sql + "where produccion=1 and contrasena='" + passwordBoxIngreso.Password + "'";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxModulo.Items.Add(dr["modulo"].ToString());
                labelIngen.Content = dr["codigo"].ToString();
                buttonGuardar.IsEnabled = true;
            }
            dr.Close();
            cnProduccion.Close();
            #endregion
        }
        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            double operariosMaquina = 0;
            double operariosManuqles = 0;
            if (string.IsNullOrEmpty(TextBoxCostura.Text)) { operariosMaquina = 0; } else { operariosMaquina = Convert.ToDouble(TextBoxCostura.Text); }
            if (string.IsNullOrEmpty(TextBoxManuales.Text)) { operariosManuqles = 0; } else { operariosManuqles = Convert.ToDouble(TextBoxManuales.Text); }
            double totalOperarios = operariosMaquina + operariosMaquina;
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
                //consultar si todos los lotes son validos
                int conteoLotesInvalidos = 0;
                foreach (horaProduccion item in listViewPiezas.Items)
                {
                    if (item.colorLote == "Red")
                    {
                        conteoLotesInvalidos = conteoLotesInvalidos + 1;
                    }
                }
                if (conteoLotesInvalidos > 0)
                {
                    buttonIngresarLotesRojos.IsEnabled = false;
                    passWordBoxValidarUsuario.Password = "";
                    labelNombreAutoriza.Content = "----";
                    popUpValidarUsuario.IsOpen = true;
                }
                else
                {
                    cnProduccion.Open();
                    foreach (horaProduccion item in listViewPiezas.Items)
                    {
                        if (item.colorLote != "Red")
                        {
                            sql = "insert into horahora(Fecha, Turno, Hora, Modulo, Arterias, Coordinador, estilo, temporada, empaque, SAM, Incapacitados, Permisos, [Cita ISSS], Inasistencia, [Ope Costura], [Ope Manuales], Lote, [2XS], XS,S, M, L, XL, [2XL], [3XL], [Tiempo de Paro], [Motivo de Paro], [Custom], [Minutos efectivos], [Cambio de Estilo], ingresadoPor) ";
                            sql = sql + "values('" + fecha + "', '" + turno + "', " + hora + ", '" + modulo + "', " + arteria + ", '" + coordinador + "', '" + item.estilo + "', '" + item.temporada + "', '" + item.empaque + "', ";
                            sql = sql + item.sam + ", " + incapacitados + ", " + permisos + ", " + cita + ", " + inasistencia + ", " + costura + ", " + manuales + ", '" + item.lote + "', '" + item.xxs + "', '" + item.xs + "', '" + item.s + "', '" + item.m + "', '" + item.l + "', '" + item.xl + "', '" + item.xxl + "', '" + item.xxxl + "', " + item.tiempoParo + ", '";
                            sql = sql + item.motivoParo + "', '" + custom_ + "', " + item.minutosEfectivos + ", '" + cambioEstilo + "', '" + labelIngen.Content.ToString() + "')";
                            cm = new SqlCommand(sql, cnProduccion);
                            cm.ExecuteNonQuery();
                        }
                    }
                    cnProduccion.Close();
                    #region navegarInicioProduccion
                    PagePrincipal pagePrincipal = new PagePrincipal();
                    Grid gridInicio = (Grid)pagePrincipal.Content;
                    foreach(object objeto in gridInicio.Children)
                    {
                        if (objeto.GetType() == typeof(Grid))
                        {
                            Grid grid = (Grid)objeto;
                            if (grid.Name == "gridListaAreas")
                            {
                                foreach(object objeto2 in grid.Children)
                                {
                                    if(objeto2.GetType()== typeof(ListView))
                                    {
                                        ListView listviewMenu = (ListView)objeto2;
                                        listviewMenu.SelectedIndex = 1;
                                    }
                                }
                            }
                        }
                    }
                    NavigationService.Navigate(pagePrincipal);
                    #endregion
                }
            }
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            buttonIngresarLotesRojos.IsEnabled = false;
            #region variablesDeConexionn
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion
            #region consultarUsuario
            //consultar
            sql = "select codigo from usuarios where contrasena='" + passWordBoxValidarUsuario.Password + "' and nivel='1' and produccion='1'";
            cnIngenieria.Open();
            cm = new SqlCommand(sql, cnIngenieria);
            dr = cm.ExecuteReader();
            if (dr.Read())
            {
                labelNombreAutoriza.Content = dr["codigo"].ToString();
                buttonIngresarLotesRojos.IsEnabled = true;
            }
            dr.Close();
            cnIngenieria.Close();
            #endregion
        }
        private void buttonIngresarLotesRojos_Click(object sender, RoutedEventArgs e)
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
            if (checkBoxCambio.IsChecked == true) { cambioEstilo = "Si"; }

            cnProduccion.Open();
            foreach (horaProduccion item in listViewPiezas.Items)
            {
                if (item.colorLote != "Red")
                {
                    sql = "insert into horahora(Fecha, Turno, Hora, Modulo, Arterias, Coordinador, estilo, temporada, empaque, SAM, Incapacitados, Permisos, [Cita ISSS], Inasistencia, [Ope Costura], [Ope Manuales], Lote, [2XS], XS,S, M, L, XL, [2XL], [3XL], [Tiempo de Paro], [Motivo de Paro], [Custom], [Minutos efectivos], [Cambio de Estilo], ingresadoPor) ";
                    sql = sql + "values('" + fecha + "', '" + turno + "', " + hora + ", '" + modulo + "', " + arteria + ", '" + coordinador + "', '" + item.estilo + "', '" + item.temporada + "', '" + item.empaque + "', ";
                    sql = sql + item.sam + ", " + incapacitados + ", " + permisos + ", " + cita + ", " + inasistencia + ", " + costura + ", " + manuales + ", '" + item.lote + "', '" + item.xxs + "', '" + item.xs + "', '" + item.s + "', '" + item.m + "', '" + item.l + "', '" + item.xl + "', '" + item.xxl + "', '" + item.xxxl + "', " + item.tiempoParo + ", '";
                    sql = sql + item.motivoParo + "', '" + custom_ + "', " + item.minutosEfectivos + ", '" + cambioEstilo + "', '" + labelIngen.Content.ToString() + "')";
                    cm = new SqlCommand(sql, cnProduccion);
                    cm.ExecuteNonQuery();
                }
                else
                {
                    sql = "insert into horahora(Fecha, Turno, Hora, Modulo, Arterias, Coordinador, estilo, temporada, empaque, SAM, Incapacitados, Permisos, [Cita ISSS], Inasistencia, [Ope Costura], [Ope Manuales], Lote, [2XS], XS,S, M, L, XL, [2XL], [3XL], [Tiempo de Paro], [Motivo de Paro], [Custom], [Minutos efectivos], [Cambio de Estilo], ingresadoPor, autorizoSobreProduccion) ";
                    sql = sql + "values('" + fecha + "', '" + turno + "', " + hora + ", '" + modulo + "', " + arteria + ", '" + coordinador + "', '" + item.estilo + "', '" + item.temporada + "', '" + item.empaque + "', ";
                    sql = sql + item.sam + ", " + incapacitados + ", " + permisos + ", " + cita + ", " + inasistencia + ", " + costura + ", " + manuales + ", '" + item.lote + "', '" + item.xxs + "', '" + item.xs + "', '" + item.s + "', '" + item.m + "', '" + item.l + "', '" + item.xl + "', '" + item.xxl + "', '" + item.xxxl + "', " + item.tiempoParo + ", '";
                    sql = sql + item.motivoParo + "', '" + custom_ + "', " + item.minutosEfectivos + ", '" + cambioEstilo + "', '" + labelIngen.Content.ToString() + "', '" + labelNombreAutoriza.Content.ToString() + "')";
                    cm = new SqlCommand(sql, cnProduccion);
                    cm.ExecuteNonQuery();
                }
            }
            cnProduccion.Close();
            #region navegarInicioProduccion
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
                                listviewMenu.SelectedIndex = 1;
                            }
                        }
                    }
                }
            }
            NavigationService.Navigate(pagePrincipal);
            #endregion
        }
        private void ButtonCerrarPopup2_Click_1(object sender, RoutedEventArgs e)
        {
            popUpValidarUsuario.IsOpen = false;
        }
        private void ButtonCerrarPopup2_Click(object sender, RoutedEventArgs e)
        {
            popUpValidarUsuario.IsOpen = false;
        }
        #endregion
    }
}




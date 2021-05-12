﻿using System;
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
    public partial class editarRegistrosProduccion : Page
    {
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public int piezasLote = 0;
        public int piezasReportadas = 0;
        #endregion
        #region listasGenerales
        public string[] _turnos = new string[3];
        public List<string> _modulos = new List<string>();
        public int[] _arterias = new int[3];
        public int[] _horas = new int[12];
        public string[] _eleccion = new string[2];
        public string[] _motivos = new string[8];
        #endregion
        #region datosIniciales
        public editarRegistrosProduccion(int codigo)
        {
            InitializeComponent();
            labelUsuario.Content = codigo;
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #region datosDeCombBox
            //llenar lista de modulos
            //consultar
            sql = "select modulosProduccion.modulo as modulo from modulosProduccion left join ingenieria.dbo.usuarios on ";
            sql = sql + "modulosProduccion.ingenieroProcesosCodigo= usuarios.codigo or modulosProduccion.coordinadorCodigo= usuarios.codigo ";
            sql = sql + "where produccion=1 and usuarios.codigo='" + codigo + "'";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                comboBoxModulo.Items.Add(dr["modulo"].ToString());
                _modulos.Add(dr["modulo"].ToString());
            }
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
        #region consultarRegistros
        private void consultarRegistros()
        {
            List<horaProduccion> lista = new List<horaProduccion>();
            //limpiar consulta anterior
            listViewRegistros.Items.Clear();
            //variables de conexion
            #region variablesConexion
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion

            //variables para consulta
            string fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
            #region validacionFecha
            if (fecha == "0001-01-01" || string.IsNullOrEmpty(fecha))
            {
                fecha = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                fecha = Convert.ToDateTime(calendarFecha.SelectedDate).ToString("yyyy-MM-dd");
            }
            #endregion
            int arteria = 1;
            #region validacionArteria
            if (comboBoxArteria.SelectedIndex == -1)
            {
                arteria = 1;
            }
            else
            {
                arteria= Convert.ToInt32(comboBoxArteria.SelectedItem);
            }
            #endregion
            string modulo = "";
            #region validarModulo
            if (comboBoxModulo.SelectedIndex == -1)
            {
                modulo = "";
            }
            else
            {
                modulo = comboBoxModulo.SelectedItem.ToString();
            }
            #endregion

            //agregar datos a listas
            #region agregarTurnos
            _turnos[0] = "Diurno";
            _turnos[1] = "Nocturno";
            _turnos[2] = "Extra";
            #endregion
            #region agregarArterias
            _arterias[0] = 1;
            _arterias[1] = 2;
            _arterias[2] = 3;
            #endregion
            #region agregareleccion
            _eleccion[0] = "Si";
            _eleccion[1] = "No";
            #endregion
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
            #region agregarHora
            cnProduccion.Open();
            sql = "select hora from horas";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            int conteoHora = 0;
            while (dr.Read())
            {
                _horas[conteoHora] = Convert.ToInt32(dr["hora"]);
                conteoHora = conteoHora + 1;
            };
            dr.Close();
            cnProduccion.Close();
            #endregion

            cnProduccion.Open();
            sql = "select num_hh, Fecha, Turno, Hora, Modulo, arterias, estilo, temporada, empaque, sam, Incapacitados, Permisos, [Cita ISSS] as cita, Inasistencia, [Ope Costura] as costura, [Ope Manuales] as manuales, ";
            sql = sql + "Lote, [2XS] as xxs, XS, S, M, L, XL, [2XL] as xxl, [3XL] as xxxl, totalDePiezas, [Tiempo de Paro] as tiempoParo, [Motivo de Paro] as motivoParo, [Custom], [Minutos efectivos] as minutosEfectivos, [Cambio de Estilo] as cambioEstilo, ingresadoPor from horahora where fecha='" + fecha + "' and modulo='" + modulo + "' and arterias='" + arteria + "'" ;
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new horaProduccion { num_hh = Convert.ToInt32(dr["num_hh"]), fecha = Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"), turno = dr["Turno"].ToString(), turnos = _turnos, hora = Convert.ToInt32(dr["hora"]), horas = _horas, modulo = dr["Modulo"].ToString(), modulos = _modulos.ToArray(), arteria = Convert.ToInt32(dr["arterias"]), arterias = _arterias, estilo=dr["estilo"].ToString(), temporada=dr["temporada"].ToString(), empaque=dr["empaque"].ToString(), sam = Convert.ToDouble(dr["sam"]), incapacitados=Convert.ToInt32(dr["incapacitados"]), permisos=Convert.ToInt32(dr["Permisos"]), cita=Convert.ToInt32(dr["cita"]), inasistencia=Convert.ToInt32(dr["Inasistencia"]), opeCostura= Convert.ToInt32(dr["costura"]), opeManuales=Convert.ToInt32(dr["manuales"]),lote=dr["lote"].ToString(), xxs=Convert.ToInt32(dr["xxs"]), xs=Convert.ToInt32(dr["xs"]), s=Convert.ToInt32(dr["s"]), m=Convert.ToInt32(dr["m"]), l=Convert.ToInt32(dr["l"]), xl=Convert.ToInt32(dr["xl"]), xxl=Convert.ToInt32(dr["xxl"]), xxxl=Convert.ToInt32(dr["xxxl"]), totalDePiezas=Convert.ToInt32(dr["totalDePiezas"]), tiempoParo=Convert.ToDouble(dr["tiempoParo"] is DBNull ? 0: dr["tiempoParo"]), motivoParo=dr["motivoParo"].ToString() , motivos=_motivos, custom=dr["custom"].ToString(), eleccion=_eleccion, cambioEstilo=dr["cambioEstilo"].ToString(), minutosEfectivos=Convert.ToDouble(dr["minutosEfectivos"]), ingresadoPor=dr["ingresadoPor"].ToString()  });
            };
            dr.Close();
            cnProduccion.Close();

            cnIngenieria.Open();
            foreach (horaProduccion item in lista)
            {
                List<string> empaques = new List<string>();
                sql = "select tipo_empaque from lotesconsamempaque where lote='" + item.lote + "'";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    empaques.Add(dr["tipo_empaque"].ToString());
                };
                dr.Close();
                listViewRegistros.Items.Add(new horaProduccion {fecha=item.fecha, turno=item.turno, hora=item.hora, modulo=item.modulo, arteria=item.arteria, lote=item.lote, empaque=item.empaque, empaques=empaques, estilo=item.estilo, temporada=item.temporada, sam=item.sam, incapacitados=item.incapacitados, permisos=item.permisos, cita=item.cita, inasistencia=item.inasistencia, opeCostura=item.opeCostura, opeManuales=item.opeManuales, xxs=item.xxs, xs=item.xs, s=item.s, m=item.m, l=item.l, xl=item.xl, xxl=item.xxl, xxxl=item.xxxl, totalDePiezas=item.totalDePiezas, motivoParo=item.motivoParo, tiempoParo=item.tiempoParo, custom=item.custom, minutosEfectivos=item.minutosEfectivos, cambioEstilo=item.cambioEstilo, ingresadoPor=item.ingresadoPor, num_hh=item.num_hh, arterias=item.arterias, turnos=item.turnos, modulos=item.modulos, horas=item.horas, eleccion=item.eleccion, motivos=item.motivos});
            }
            cnIngenieria.Close();
        }
        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            consultarRegistros();
        }
        private void calendarFecha_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            consultarRegistros();
        }
        private void comboBoxArteria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            consultarRegistros();
        }
        #endregion
        #region guardarRegistros
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region variablesConexion
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion

            cnProduccion.Open();
            foreach (horaProduccion item in listViewRegistros.Items)
            {
                int diferenciaPiezas = (item.xxs + item.xs + item.s + item.m + item.l + item.xl + item.xxl + item.xxxl)-item.totalDePiezas;
                sql = "select valido, diferencia from validacionLotes  where lote='"+item.lote+"'";
                cm = new SqlCommand(sql, cnProduccion);
                dr=cm.ExecuteReader();
                dr.Read();
                int valido = Convert.ToInt32(dr["valido"]);
                int diferenciaPiezasIngresadas = Convert.ToInt32(dr["diferencia"]);
                dr.Close();
                //validar si el lote existe
                if (valido == 0)
                {
                    MessageBox.Show("Lote No valido");
                }
                else
                {
                    //validar el numero de piezas a ingresar
                    int resultadoValidacion = diferenciaPiezasIngresadas + diferenciaPiezas;
                    if (resultadoValidacion >= 0)
                    {
                        if (item.sam > 0)
                        {
                            sql = "update horahora set turno='" + item.turno + "', Fecha='" + item.fecha + "', Hora= '" + item.hora + "', Modulo='" + item.modulo + "', arterias='" + item.arteria + "', estilo='" + item.estilo + "', temporada='" + item.temporada + "', sam='" + item.sam + "', empaque='" + item.empaque + "', incapacitados='" + item.incapacitados + "', Permisos='" + item.permisos + "', [Cita ISSS]= '" + item.cita + "', Inasistencia= '" + item.inasistencia + "', [Ope Costura]= '" + item.opeCostura + "', [Ope Manuales]='" + item.opeCostura + "', Lote='" + item.lote + "', [2XS]='" + item.xxs + "', XS='" + item.xs + "', S='" + item.s + "', M='" + item.m + "', L='" + item.l + "', XL='" + item.xl + "', [2XL]='" + item.xxl + "', [3XL]='" + item.xxxl + "', [Tiempo de Paro]='" + item.tiempoParo + "', [Motivo de Paro]='" + item.motivoParo + "', [custom]='" + item.custom + "', [Minutos Efectivos]='" + item.minutosEfectivos + "', [Cambio de Estilo]='" + item.cambioEstilo + "', ingresadoPor='" + labelUsuario.Content.ToString() + "'  where num_hh='" + item.num_hh + "'";
                            cm = new SqlCommand(sql, cnProduccion);
                            cm.ExecuteNonQuery();
                        }
                        else
                        {
                            MessageBox.Show("El registro numero " + item.num_hh + " tiene SAM de 0 por lo que no fue actualizado");
                        }
                    }
                    else
                    {
                        MessageBox.Show("El registro numero " + item.num_hh + " no fue actualizado porque se superara el numero de piezas del lote");
                    }
                }
            }
            cnProduccion.Close();
            MessageBox.Show("Accion Terminada");
        }
        private void buttonActualizarEmpaques_Click(object sender, RoutedEventArgs e)
        {
            #region variablesConexion
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion
            List<horaProduccion> listaAuxiliar = new List<horaProduccion>();
            cnIngenieria.Open();
            foreach (horaProduccion item in listViewRegistros.Items)
            {
                List<string> _empaques = new List<string>();
                string _estilo = "";
                string _temporada = "";
                sql = "select estilo, temporada, tipo_empaque from lotesconsamempaque where lote='" + item.lote + "'";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    _empaques.Add(dr["tipo_empaque"].ToString());
                    _estilo = dr["estilo"].ToString();
                    _temporada = dr["temporada"].ToString();
                }
                dr.Close();
                listaAuxiliar.Add(new horaProduccion { num_hh = item.num_hh, fecha = item.fecha, turno = item.turno, turnos = item.turnos, hora = item.hora, horas = item.horas, modulo = item.modulo, modulos = item.modulos, arteria = item.arteria, arterias = item.arterias, estilo = _estilo, empaque = item.empaque, empaques = _empaques, temporada = _temporada, sam = 0, incapacitados = item.incapacitados, permisos = item.permisos, cita = item.cita, inasistencia = item.inasistencia, opeCostura = item.opeCostura, opeManuales = item.opeManuales, lote = item.lote, xxs = item.xxs, xs = item.xs, s = item.s, m = item.m, l = item.l, xl = item.xl, xxl = item.xxl, xxxl = item.xxxl, totalDePiezas=item.totalDePiezas, tiempoParo = item.tiempoParo, motivoParo = item.motivoParo, motivos = item.motivos, custom = item.custom, eleccion = item.eleccion, cambioEstilo = item.cambioEstilo, minutosEfectivos = item.minutosEfectivos, ingresadoPor = item.ingresadoPor });
            }
            cnIngenieria.Close();
            //limpiar listVien y agregar nuevos
            listViewRegistros.Items.Clear();
            foreach (horaProduccion item2 in listaAuxiliar)
            {
                listViewRegistros.Items.Add(item2);
            }
            MessageBox.Show("Empaques Actualizados");
        }
        private void buttonActualizarSam_Click(object sender, RoutedEventArgs e)
        {
            #region variablesConexion
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            double _sam = 0;
            #endregion
            List<horaProduccion> listaAuxiliar = new  List<horaProduccion>();
            cnIngenieria.Open();
            foreach (horaProduccion item in listViewRegistros.Items)
            {
                sql = "select sam from lotesconsamempaque where lote='" + item.lote+"' and tipo_empaque='"+item.empaque+"'";
                cm = new SqlCommand(sql, cnIngenieria);
                dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    _sam = Convert.ToDouble(dr["sam"]);
                }
                dr.Close();
                listaAuxiliar.Add(new horaProduccion { num_hh = item.num_hh, fecha = item.fecha, turno = item.turno, turnos = item.turnos, hora = item.hora, horas = item.horas, modulo = item.modulo, modulos = item.modulos, arteria = item.arteria, arterias = item.arterias, estilo=item.estilo, empaque=item.empaque, empaques=item.empaques, temporada=item.temporada, sam = _sam, incapacitados = item.incapacitados, permisos = item.permisos, cita = item.cita, inasistencia = item.inasistencia, opeCostura = item.opeCostura, opeManuales = item.opeManuales, lote = item.lote, xxs = item.xxs, xs = item.xs, s = item.s, m = item.m, l = item.l, xl = item.xl, xxl = item.xxl, xxxl = item.xxxl, totalDePiezas=item.totalDePiezas, tiempoParo = item.tiempoParo, motivoParo = item.motivoParo, motivos = item.motivos, custom = item.custom, eleccion = item.eleccion, cambioEstilo = item.cambioEstilo, minutosEfectivos = item.minutosEfectivos, ingresadoPor = item.ingresadoPor });
            }
            cnIngenieria.Close();
            //limpiar listVien y agregar nuevos
            listViewRegistros.Items.Clear();
            foreach(horaProduccion item2 in listaAuxiliar)
            {
                listViewRegistros.Items.Add(item2);
            }
            MessageBox.Show("SAM Actualizado");
        }
        #endregion
        #region eliminarRegistrosSeleccionados
        private void listViewRegistros_KeyDown(object sender, KeyEventArgs e)
        {
            #region variablesConexion
            string sql;
            SqlCommand cm;
            #endregion
            // eliminar el registro seleccionado con ctrl y d
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                MessageBoxResult result = MessageBox.Show("¿Desea Eliminar los Registros Seleccionados?", "Jazz-CCO", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        cnProduccion.Open();
                        foreach (horaProduccion item in listViewRegistros.SelectedItems)
                        {
                            sql = "delete from horahora where num_hh='" + item.num_hh + "'";
                            cm = new SqlCommand(sql, cnProduccion);
                            cm.ExecuteNonQuery();
                        }
                        cnProduccion.Close();
                        consultarRegistros();

                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
        #endregion
    }
}

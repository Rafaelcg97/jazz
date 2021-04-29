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
        public string[] _modulos = new string[100];
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
        #region consultarRegistros

        private void consultarRegistros()
        {
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
            #region agregarModulos
            cnProduccion.Open();
            sql = "select modulo from modulosProduccion where coordinador<>'-'";
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            int conteoModulo = 0;
            while (dr.Read())
            {
                _modulos[conteoModulo] = dr["modulo"].ToString();
                conteoModulo = conteoModulo + 1;
            };
            dr.Close();
            cnProduccion.Close();
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

            //llenar lista de modulos
            cnProduccion.Open();
            sql = "select num_hh, Fecha, Turno, Hora, Modulo, arterias, [Cod Info Estilo] as codigo, sam, Incapacitados, Permisos, [Cita ISSS] as cita, Inasistencia, [Ope Costura] as costura, [Ope Manuales] as manuales, ";
            sql = sql + "Lote, [2XS] as xxs, XS, S, M, L, XL, [2XL] as xxl, [3XL] as xxxl, [Tiempo de Paro] as tiempoParo, [Motivo de Paro] as motivoParo, [Custom], [Minutos efectivos] as minutosEfectivos, [Cambio de Estilo] as cambioEstilo, ingresadoPor from horahora where fecha='" + fecha + "' and modulo='" + modulo + "' and arterias='" + arteria + "'" ;
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
            listViewRegistros.Items.Add(new horaProduccion { num_hh = Convert.ToInt32(dr["num_hh"]), fecha = Convert.ToDateTime(dr["fecha"]).ToString("yyyy-MM-dd"), turno = dr["Turno"].ToString(), turnos = _turnos, hora = Convert.ToInt32(dr["hora"]), horas = _horas, modulo = dr["Modulo"].ToString(), modulos = _modulos, arteria = Convert.ToInt32(dr["arterias"]), arterias = _arterias, codigo = Convert.ToInt32(dr["codigo"]), sam = Convert.ToDouble(dr["sam"]), incapacitados=Convert.ToInt32(dr["incapacitados"]), permisos=Convert.ToInt32(dr["Permisos"]), cita=Convert.ToInt32(dr["cita"]), inasistencia=Convert.ToInt32(dr["Inasistencia"]), opeCostura= Convert.ToInt32(dr["costura"]), opeManuales=Convert.ToInt32(dr["manuales"]),lote=dr["lote"].ToString(), xxs=Convert.ToInt32(dr["xxs"]), xs=Convert.ToInt32(dr["xs"]), s=Convert.ToInt32(dr["s"]), m=Convert.ToInt32(dr["m"]), l=Convert.ToInt32(dr["l"]), xl=Convert.ToInt32(dr["xl"]), xxl=Convert.ToInt32(dr["xxl"]), xxxl=Convert.ToInt32(dr["xxxl"]), tiempoParo=Convert.ToDouble(dr["tiempoParo"]), motivoParo=dr["motivoParo"].ToString() , motivos=_motivos, custom=dr["custom"].ToString(), eleccion=_eleccion, cambioEstilo=dr["cambioEstilo"].ToString(), minutosEfectivos=Convert.ToDouble(dr["minutosEfectivos"]), ingresadoPor=dr["ingresadoPor"].ToString()  });
            };
            dr.Close();
            cnProduccion.Close();
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
            #endregion

            cnProduccion.Open();
            foreach (horaProduccion item in listViewRegistros.Items)
            {
                sql = "update horahora set turno='" + item.turno + "', Fecha='"+ item.fecha +"', Hora= '"+ item.hora +"', Modulo='"+ item.modulo + "', arterias='"+ item.arteria +"', [Cod Info Estilo]='"+ item.codigo + "', incapacitados='"+ item.incapacitados +"', Permisos='"+item.permisos+"', [Cita ISSS]= '"+ item.cita + "', Inasistencia= '"+item.inasistencia+"', [Ope Costura]= '"+item.opeCostura+"', [Ope Manuales]='"+item.opeCostura +"', Lote='"+item.lote+"', [2XS]='"+item.xxs+"', XS='"+item.xs +"', S='"+item.s+"', M='"+item.m +"', L='"+item.l+"', XL='"+item.xl+"', [2XL]='"+item.xxl+"', [3XL]='"+item.xxxl+"', [Tiempo de Paro]='"+item.tiempoParo+"', [Motivo de Paro]='"+item.motivoParo+"', [custom]='"+item.custom+"', [Minutos Efectivos]='"+item.minutosEfectivos+"', [Cambio de Estilo]='"+item.cambioEstilo+"', ingresadoPor='"+labelUsuario.Content.ToString()+"'  where num_hh='"+item.num_hh+"'";
                cm = new SqlCommand(sql, cnProduccion);
                cm.ExecuteNonQuery();
            }
            cnProduccion.Close();
            MessageBox.Show("Datos Actualizados");
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
            cnProduccion.Open();
            foreach (horaProduccion item in listViewRegistros.Items)
            {
                sql = "select SAM from sam where CODIGO='"+item.codigo+"'";
                cm = new SqlCommand(sql, cnProduccion);
                dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    _sam = Convert.ToDouble(dr["SAM"]);
                }
                dr.Close();
                listaAuxiliar.Add(new horaProduccion { num_hh = item.num_hh, fecha = item.fecha, turno = item.turno, turnos = item.turnos, hora = item.hora, horas = item.horas, modulo = item.modulo, modulos = item.modulos, arteria = item.arteria, arterias = item.arterias, codigo = item.codigo, sam = _sam, incapacitados = item.incapacitados, permisos = item.permisos, cita = item.cita, inasistencia = item.inasistencia, opeCostura = item.opeCostura, opeManuales = item.opeManuales, lote = item.lote, xxs = item.xxs, xs = item.xs, s = item.s, m = item.m, l = item.l, xl = item.xl, xxl = item.xxl, xxxl = item.xxxl, tiempoParo = item.tiempoParo, motivoParo = item.motivoParo, motivos = item.motivos, custom = item.custom, eleccion = item.eleccion, cambioEstilo = item.cambioEstilo, minutosEfectivos = item.minutosEfectivos, ingresadoPor = item.ingresadoPor });
            }
            cnProduccion.Close();
            //limpiar listVien y agregar nuevos
            listViewRegistros.Items.Clear();
            foreach(horaProduccion item2 in listaAuxiliar)
            {
                listViewRegistros.Items.Add(new horaProduccion { num_hh = item2.num_hh, fecha = item2.fecha, turno = item2.turno, turnos = item2.turnos, hora = item2.hora, horas = item2.horas, modulo = item2.modulo, modulos = item2.modulos, arteria = item2.arteria, arterias = item2.arterias, codigo = item2.codigo, sam = item2.sam, incapacitados = item2.incapacitados, permisos = item2.permisos, cita = item2.cita, inasistencia = item2.inasistencia, opeCostura = item2.opeCostura, opeManuales = item2.opeManuales, lote = item2.lote, xxs = item2.xxs, xs = item2.xs, s = item2.s, m = item2.m, l = item2.l, xl = item2.xl, xxl = item2.xxl, xxxl = item2.xxxl, tiempoParo = item2.tiempoParo, motivoParo = item2.motivoParo, motivos = item2.motivos, custom = item2.custom, eleccion = item2.eleccion, cambioEstilo = item2.cambioEstilo, minutosEfectivos = item2.minutosEfectivos, ingresadoPor = item2.ingresadoPor });
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
                cnProduccion.Open();
                foreach(horaProduccion item in listViewRegistros.SelectedItems)
                {
                    sql = "delete from horahora where num_hh='"+item.num_hh+"'";
                    cm = new SqlCommand(sql, cnProduccion);
                    cm.ExecuteNonQuery();
                }
                cnProduccion.Close();
                consultarRegistros();
            }
        }
        #endregion
    }
}

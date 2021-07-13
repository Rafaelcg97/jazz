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
using Production_control_1._0.pantallasIniciales;

namespace Production_control_1._0.kanban
{
    public partial class solicitudMateriales : UserControl
    {
        #region varibalesConexion
        public SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        public SqlConnection cnManto = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion
        List<solicitudKanban> listaCompletaLotes = new List<solicitudKanban>();
        List<materialesEstilos> listaDeMaterialesCompleta = new List<materialesEstilos>();
        public solicitudMateriales()
        {
            InitializeComponent();
        }
        #region tamanos_de_letra_/_tipo_de_texto
        private void letraAjustable1(object sender, SizeChangedEventArgs e)
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
        private void letraAjustable2(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.1 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }
        private void letraAjustable4(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.4 / tmp.FontFamily.LineSpacing;
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
        #region calculosGenerales
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
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            Grid GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Grid)) as Grid;
            GridPrincipal.Children.Clear();
            GridPrincipal.Children.Add(new pantallasIniciales.kanban());
        }

        private void enviar_reporte_Click(object sender, RoutedEventArgs e)
        {
            string tipo_ = "solicitud";
            //if (radioButtonDevolucion.IsChecked == true)
            //{
            //    tipo_ = "devolucion";
            //}
            //else
            //{
            //    tipo_ = "solicitud";
            //}

            //SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            //string sql = "insert into solicitudesKanban(tipo, modulo, ubicacion, lote, material, fechaSolicitud) values('"+tipo_+"', '"+ listBoxModulo.SelectedItem.ToString() + "', '" + labelUbicacion.Content + "', '" + listBoxLote.SelectedItem.ToString() + "', '"+ listBoxMaterial.SelectedItem.ToString()+"', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "')";
            //cn.Open();
            //SqlCommand cm = new SqlCommand(sql, cn);
            //cm.ExecuteNonQuery();
            //cn.Close();

            //estadoPlanta estadoPlanta = new estadoPlanta();
            //Frame GridPrincipal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
            //GridPrincipal.Content = estadoPlanta;
        }

        private void listBoxLote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                //mostrar datos general de el lote
                solicitudKanban loteSeleccionado = (solicitudKanban)listBoxLote.SelectedItem;
                labelEstilo.Content = loteSeleccionado.estilo;
                labelTemporada.Content = loteSeleccionado.temporada;
                labelPiezas.Content = loteSeleccionado.cantidad;
                string lote = listBoxLote.SelectedItem.ToString();

                //conteo de solicitudes de lote seleccionado
                string sql = "select count(solicitudKanbanId) as numero from detalleSolicitudeKanban where lote='" + lote + "'";
                cnKanban.Open();
                SqlCommand cm = new SqlCommand(sql, cnKanban);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    labelNumeroDeOrden.Content = (Convert.ToInt32(dr["numero"])+1).ToString();
                }
                else
                {
                    labelNumeroDeOrden.Content = "1";
                };
                dr.Close();

                //cargar en lista todos los materiales que tiene lote seleccionado
                listaDeMaterialesCompleta.Clear();
                sql = "select StyleId, StyleNumber, SeasonCode, CategoryName, SubCategoryName, PartNumber, Quantity from componentePorEstilo where StyleId='" + loteSeleccionado.styleId+"'";

                sql = "select " +
                    "StyleNumber, " +
                    "SeasonCode, " +
                    "CategoryName, " +
                    "SubCategoryName, " +
                    "PartNumber, " +
                    "Quantity " +
                    "from componentePorEstilo as a " +
                    "where a.StyleId='97210' and " +
                    "varyByColor=0 and varyBySize=0 " +
                    "union " +
                    "select " +
                    "styleNumber, " +
                    "seasonCode, " +
                    "categoryName, " +
                    "subcategoryName, " +
                    "partNumber, " +
                    "quantity " +
                    "from  variacionesPorEstilo " +
                    "where StyleId = '97210'";

                cm = new SqlCommand(sql, cnKanban);
                dr = cm.ExecuteReader();
                while(dr.Read())
                {
                    listaDeMaterialesCompleta.Add(new materialesEstilos {styleId=Convert.ToInt32(dr["StyleId"]), styleNumber=dr["styleNumber"].ToString(), seasonCode=dr["SeasonCode"].ToString(), categoryName=dr["categoryName"].ToString(), subCategoryName=dr["SubCategoryName"].ToString(), partNumber=dr["partNumber"].ToString(), quantity=Convert.ToDouble(dr["quantity"] is DBNull? 0 : dr["quantity"]) });
                }
                dr.Close();
                cnKanban.Close();

                //cargar lista de accesorios del lote
            }
        }

        private void textBoxBuscarLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            //limpiar lista de lotes
            listBoxLote.Items.Clear();

            //revisar lotes que coinciden con la busqueda
            foreach (solicitudKanban lote in listaCompletaLotes)
            {
                if (!string.IsNullOrEmpty(textBoxBuscarLote.Text))
                {
                    if (lote.lote.StartsWith(textBoxBuscarLote.Text))
                    {
                        listBoxLote.Items.Add(lote);
                    }
                }
                else
                {
                    listBoxLote.Items.Add(lote);
                }

            }

            //limpiar las etiquetas de solicitud, piezas, tempordas
            labelNumeroDeOrden.Content = "0";
            labelEstilo.Content = "----";
            labelTemporada.Content = "----";
            labelPiezas.Content = "----";
            labelEstadoAccesorios.Content = "----";

        }

        private void buttonAgregarMaterial_Click(object sender, RoutedEventArgs e)
        {
            bool datoExistente = false;
            foreach(solicitudKanban item in listViewListaMateriales.Items)
            {
                if(item.lote == ((solicitudKanban)(listBoxLote.SelectedItem)).lote && item.material == "Accesorios")
                {
                    datoExistente = true;
                }
            }
            if (datoExistente==false)
            {
                listViewListaMateriales.Items.Add(new solicitudKanban
                {
                    lote = ((solicitudKanban)(listBoxLote.SelectedItem)).lote,
                    modulo = listBoxModulo.SelectedItem.ToString(),
                    material = "Accesorios",
                    cantidad = 1,
                    talla = "Unica"
                });
            }
            else
            {
                MessageBox.Show("Ya lo agregaste a la lista de elementos solictados");
            }

        }

        private void buttonEnviarSolicitud_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt32(labelNumeroDeOrden.Content) > 3)
            {
                MessageBox.Show("Se excedio el numero de solicitudes permitidas");
            }
            else
            {
                solicitudKanban loteSeleccionado = (solicitudKanban)listBoxLote.SelectedItem;
                string lote = listBoxLote.SelectedItem.ToString();
                SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "insert into solicitudesKanban(tipo, modulo, ubicacion, fechaSolicitud) values('solicitud', '" + listBoxModulo.SelectedItem.ToString() + "', '"+labelUbicacion.Content + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "') SELECT SCOPE_IDENTITY()";
                cnKanban.Open();
                SqlCommand cm = new SqlCommand(sql, cnKanban);
                SqlDataReader dr = cm.ExecuteReader();
                dr.Read();
                int idIngreso = Convert.ToInt32(dr[0]);
                dr.Close();

                //ingresar detalles de la orden
                foreach (solicitudKanban item in listViewListaMateriales.Items)
                {
                    sql = "insert into detalleSolicitudeKanban(solicitudKanbanId, lote, material, talla, cantidad) values('" + idIngreso + "', '" + item.lote + "', '" + item.material + "', '" + item.talla + "', '" + item.cantidad+ "')";
                    cm = new SqlCommand(sql, cnKanban);
                    cm.ExecuteNonQuery();
                }
                cnKanban.Close();
            }
        }

        private void listBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxModulo.SelectedIndex > -1)
            {
                //carga ubicacion de 
                cnManto.Open();
                string sql = "select id from orden_modulos where modulo = '" + listBoxModulo.SelectedItem.ToString() + "'";
                SqlCommand cm = new SqlCommand(sql, cnManto);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    labelUbicacion.Content = dr["id"].ToString();
                }
                dr.Close();
                cnManto.Close();


                cnKanban.Open();
                sql = "select lote, make, estilo, estiloId, temporada from lotes where modulo='" + listBoxModulo.SelectedItem.ToString() + "'";
                listBoxLote.Items.Clear();
                listaCompletaLotes.Clear();
                cm = new SqlCommand(sql, cnKanban);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listBoxLote.Items.Add(
                        new solicitudKanban
                        {
                            lote = dr["lote"].ToString(),
                            cantidad = Convert.ToInt32(dr["make"]),
                            estilo = dr["estilo"].ToString(),
                            styleId = Convert.ToInt32(dr["estiloId"]),
                            temporada = dr["temporada"].ToString()
                        });
                    listaCompletaLotes.Add(new solicitudKanban
                    {
                        lote = dr["lote"].ToString(),
                        cantidad = Convert.ToInt32(dr["make"]),
                        estilo = dr["estilo"].ToString(),
                        styleId = Convert.ToInt32(dr["estiloId"]),
                        temporada = dr["temporada"].ToString()
                    });
                };
                dr.Close();
                cnKanban.Close();
            }
        }

        private void passwordboxUsuario_PasswordChanged(object sender, RoutedEventArgs e)
        {
            labelUsuario.Content = "*";
            buttonEnviarSolicitud.IsEnabled = false;
            listBoxModulo.Items.Clear();
            listBoxLote.Items.Clear();
            #region variablesDeConexionn
            string sql;
            SqlCommand cm;
            SqlDataReader dr;
            #endregion
            #region consultarUsuario
            //consultar
            sql = "select modulosProduccion.modulo as modulo, usuarios.codigo as codigo from modulosProduccion left join ingenieria.dbo.usuarios on ";
            sql = sql + "modulosProduccion.ingenieroProcesosCodigo= usuarios.codigo or modulosProduccion.coordinadorCodigo= usuarios.codigo ";
            sql = sql + "where produccion=1 and contrasena='" + passwordboxUsuario.Password + "'";
            cnProduccion.Open();
            cm = new SqlCommand(sql, cnProduccion);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxModulo.Items.Add(dr["modulo"].ToString());
                labelUsuario.Content = dr["codigo"].ToString();
                buttonEnviarSolicitud.IsEnabled = true;
            }
            dr.Close();
            cnProduccion.Close();
            #endregion
        }
    }
}

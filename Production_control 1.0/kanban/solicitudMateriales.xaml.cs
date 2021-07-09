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
        List<solicitudKanban> listaCompletaLotes = new List<solicitudKanban>();
        public solicitudMateriales()
        {
            InitializeComponent();
            //agregar modulos
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select modulo from orden_modulos group by modulo";
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listBoxModulo.Items.Add(dr["modulo"].ToString());
            };
            dr.Close();
            cn.Close();

            cnKanban.Open();
            sql = "select lote, make, estilo, estiloId, temporada from lotes";
            cn.Open();
            cm = new SqlCommand(sql, cnKanban);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                listaCompletaLotes.Add(
                    new solicitudKanban 
                    { 
                        lote = dr["lote"].ToString(), 
                        cantidad=Convert.ToInt32(dr["make"]), 
                        estilo=dr["estilo"].ToString(), 
                        styleId = Convert.ToInt32(dr["estiloId"]),
                        temporada=dr["temporada"].ToString()
                     });
            };
            dr.Close();
            cnKanban.Close();

            listBoxLote.ItemsSource = listaCompletaLotes;

            //agregar materiales


            //habilitar o deshabilitar boton
            habilitarButton();
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

        private void comboBoxModulo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select id from orden_modulos where modulo = '"+ listBoxModulo.SelectedItem.ToString() +"'";
            cn.Open();
            SqlCommand cm3 = new SqlCommand(sql, cn);
            SqlDataReader dr3 = cm3.ExecuteReader();
            dr3.Read();
            //labelUbicacion.Content = dr3["id"].ToString();
            dr3.Close();
            cn.Close();
            habilitarButton();
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

        private void habilitarButton()
        {
            //if(comboBoxModulo.SelectedIndex>-1 && listBoxMaterial.SelectedIndex>-1 && listBoxLote.SelectedIndex>-1 && (radioButtonDevolucion.IsChecked==true || radioButtonSolicitud.IsChecked == true))
            //{
            //    enviar_reporte.IsEnabled = true;
            //}
            //else
            //{
            //    enviar_reporte.IsEnabled = false;
            //}
        }

        private void listBoxLote_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxLote.SelectedIndex > -1)
            {
                solicitudKanban loteSeleccionado = (solicitudKanban)listBoxLote.SelectedItem;
                string lote = listBoxLote.SelectedItem.ToString();
                SqlConnection cnKanban = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select count(lote) as lote from solicitudesKanban where lote='" + lote + "'";
                cnKanban.Open();
                SqlCommand cm = new SqlCommand(sql, cnKanban);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    labelNumeroDeOrden.Content = dr["lote"].ToString();
                }
                else
                {
                    labelNumeroDeOrden.Content = "0";
                };
                dr.Close();
                cnKanban.Close();
                labelEstilo.Content = loteSeleccionado.estilo;
                labelTemporada.Content = loteSeleccionado.temporada;
                labelPiezas.Content = loteSeleccionado.cantidad;

                //llamar lista de partNumbers que componen el estilo

            }
            habilitarButton();
        }

        private void listBoxMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            habilitarButton();
        }

        private void textBoxBuscarLote_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<solicitudKanban> listaBusquedaLotes = new List<solicitudKanban>();
            //revisar lotes que coinciden con la busqueda
            foreach (solicitudKanban lote in listaCompletaLotes)
            {
                if (!string.IsNullOrEmpty(textBoxBuscarLote.Text))
                {
                    if (lote.lote.StartsWith(textBoxBuscarLote.Text))
                    {
                        listaBusquedaLotes.Add(lote);
                    }
                }
                else
                {
                    listaBusquedaLotes.Add(lote);
                }

            }

            listBoxLote.ItemsSource = listaBusquedaLotes;

            //limpiar las etiquetas de solicitud, piezas, tempordas
            labelNumeroDeOrden.Content = "0";
            labelEstilo.Content = "----";
            labelTemporada.Content = "----";
            labelPiezas.Content = "----";

        }
    }
}

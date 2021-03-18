using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Configuration;

namespace Production_control_1._0
{

    public partial class solicitud_repuestos : Page
    {
        #region variables_generales
        public SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion

        #region datos_iniciales
        public solicitud_repuestos()
        {
            InitializeComponent();
            usuario_.Content = clases_globales.usuario_respuesto.usuario.ToString();
            codigo_2.Content = clases_globales.usuario_respuesto.usuario.ToString();

            //se realiza la consulta en la base de cual es la contrasena del usuario

            cn.Open();
            //cargar los datos del mecanico
            string sql = "select nombre from mecanicos where codigo='" + clases_globales.usuario_respuesto.usuario.ToString() + "'";
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            dr.Read();
            nombre_.Content = dr[0].ToString();
            nombre_2.Content = dr[0].ToString();
            dr.Close();

            //cargar la lista de maquinas
            string sql2 = "select codigo from inventario_maquinas";
            SqlCommand cm2 = new SqlCommand(sql2, cn);
            SqlDataReader dr2 = cm2.ExecuteReader();
            while (dr2.Read())
            {
               maquina.Items.Add(dr2["codigo"].ToString());
            };
            dr2.Close();

            //cargar la lista de repuestos
            List <clases_globales.repuesto> lista_repuestos = new List<clases_globales.repuesto>();
            string sql3 = "select PartNumber, Description, OnHand, Cost from spare_onhand";
            SqlCommand cm3 = new SqlCommand(sql3, cn);
            SqlDataReader dr3 = cm3.ExecuteReader();
            while (dr3.Read())
            {
                lista_repuestos.Add(new clases_globales.repuesto { partnumber = dr3["PartNumber"].ToString(), description = dr3["Description"].ToString(), onhand = Convert.ToInt32(dr3["OnHand"]), cost = Convert.ToDouble(dr3["cost"]) });
            };
            dr3.Close();
            cn.Close();
            repuesto.ItemsSource = lista_repuestos;

        }
        #endregion

        #region tamanos_de_letra_/_tipo_de_texto

        private void letra_8(object sender, SizeChangedEventArgs e)
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

        private void letra_7(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.7 / tmp.FontFamily.LineSpacing;
            }
            catch
            {
                Control tmp = sender as Control;
                tmp.FontSize = 2;
            }
        }

        private void letra_09(object sender, SizeChangedEventArgs e)
        {
            try
            {
                Control tmp = sender as Control;
                tmp.FontSize = e.NewSize.Height * 0.09 / tmp.FontFamily.LineSpacing;
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

        #region control_general_del_programa
        private void salir__Click(object sender, RoutedEventArgs e)
        {
            inicio inicio = new inicio();
            this.NavigationService.Navigate(inicio);
        }
        #endregion

        private void buscar_maquina_TextChanged(object sender, TextChangedEventArgs e)
        {
            maquina.Items.Clear();
            cn.Open();
            //cargar la lista de maquinas
            string sql = "select codigo from inventario_maquinas where codigo like '%" + buscar_maquina.Text + "%'";
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                maquina.Items.Add(dr["codigo"].ToString());
            };
            dr.Close();
            cn.Close();
        }

        private void buscar_repuesto_TextChanged(object sender, TextChangedEventArgs e)
        {
            cn.Open();
            //cargar la lista de repuestos
            List<clases_globales.repuesto> lista_repuestos = new List<clases_globales.repuesto>();
            string sql = "select PartNumber, Description, OnHand, Cost from spare_onhand where Description like '%" + buscar_repuesto.Text + "%'";
            SqlCommand cm = new SqlCommand(sql, cn);
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                lista_repuestos.Add(new clases_globales.repuesto { partnumber = dr["PartNumber"].ToString(), description = dr["Description"].ToString(), onhand = Convert.ToInt32(dr["OnHand"]), cost = Convert.ToDouble(dr["cost"])});
            };
            dr.Close();
            cn.Close();
            repuesto.ItemsSource = lista_repuestos;
        }
    }
}

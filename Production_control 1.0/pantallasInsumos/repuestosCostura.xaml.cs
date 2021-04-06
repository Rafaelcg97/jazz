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

namespace Production_control_1._0.pantallasInsumos
{
    public partial class repuestosCostura : UserControl
    {
        public repuestosCostura()
        {
            InitializeComponent();
            #region conexionesConBasesSQL
            SqlConnection cnMantenimiento = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql; //Consulta que se hace en sql
            SqlCommand cm; //comando sql (base en la que se ejecutara la consulta sql)
            SqlDataReader dr; //leer los resultados del comando sql
            #endregion


            cnMantenimiento.Open();
            sql = "select top 100 PartNumber, Description, OnHand, Cost from spare_onhand where final_category='Repuestos de Costura'";
            cm = new SqlCommand(sql, cnMantenimiento);
            dr = cm.ExecuteReader();
            List<solicitudInsumo> listaDeInsumos = new List<solicitudInsumo>();
            //agregar operaciones de consulta
            while (dr.Read())
            {
                string colorDisponible = "DarkGreen";
                if (Convert.ToInt32(dr["OnHand"]) >0)
                {
                    colorDisponible = "DarkGreen";
                }
                else
                {
                    colorDisponible = "Red";
                }
               listaDeInsumos.Add(new solicitudInsumo {partNumber=dr["partNumber"].ToString(), description=dr["Description"].ToString(), onHand=Convert.ToInt32(dr["OnHand"]), cost=Convert.ToDouble(dr["Cost"]), color=colorDisponible });
            };
            dr.Close();
            cnMantenimiento.Close();
            //agregar lista de respuestos a listBoxRepuesto
            listBoxRepuesto.ItemsSource = listaDeInsumos;

        }

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
            GridPrincipal.Children.Add(new pantallasIniciales.insumos());
        }
        #endregion
    }
}

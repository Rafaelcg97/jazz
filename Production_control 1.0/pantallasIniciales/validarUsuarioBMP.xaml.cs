using Production_control_1._0.pantallasBMP;
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

namespace Production_control_1._0.pantallasIniciales
{
    public partial class validarUsuarioBMP : UserControl
    {
        public validarUsuarioBMP()
        {
            InitializeComponent();
        }
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

        private void ButtonIngresarContrasena_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
            string sql = "select codigo from usuarios where contrasena='" + passwordBoxContrasenaAdministrador.Password + "' and bmp=1";
            try
            {
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                if (dr.Read())
                {
                    Frame principal = GetDependencyObjectFromVisualTree(this, typeof(Frame)) as Frame;
                    principal.Content=new menuBMP();
                }
                else
                {
                    MessageBox.Show("El usuario ingresado no existe o no tiene autorización");
                };
                dr.Close();
                cn.Close();
            }
            catch(Exception excp)
            {
                MessageBox.Show(excp.Message);
            }

        }
    }
}

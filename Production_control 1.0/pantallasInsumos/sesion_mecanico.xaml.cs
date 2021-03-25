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
using System.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Production_control_1._0
{
    public partial class sesion_mecanico : Page
    {
        #region variables_generales
        public SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        #endregion

        #region datos_iniciales
        public sesion_mecanico()
        {
            InitializeComponent();
        }
        #endregion

        #region control_general_del_programa

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        #endregion

        #region tamanos_de_letra_/_tipo_de_texto

        private void letra_grande(object sender, SizeChangedEventArgs e)
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

        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                e.Handled = false;
            else
                e.Handled = true;
        }

        #endregion

        #region formulario_ingreso

        private void Ingresar_Click(object sender, RoutedEventArgs e)
        {
            //obtener la contrasena ingresada
            try
            {
                int contrasena_introducida = Convert.ToInt32(password.Password);
                int usuario_introducido = Convert.ToInt32(Usuario.Text);
                  
            //se realiza la consulta en la base de cual es la contrasena del usuario
            string sql = "select contrasena from mecanicos where codigo=" + usuario_introducido;
            cn.Open();
            SqlCommand cm = new SqlCommand(sql, cn);
            cm.ExecuteNonQuery();
            SqlDataReader dr = cm.ExecuteReader();
            dr.Read();
            try
            {
                int contrasena_correcta = Convert.ToInt32(dr["contrasena"]);
                dr.Close();
                cn.Close();
                //se valida si la contrasena del usuario es la misma que la que se introduji
                if (contrasena_introducida == contrasena_correcta)
                {
                    clases_globales.usuario_respuesto.usuario = usuario_introducido;
                    solicitud_repuestos solicitud_repuestos = new solicitud_repuestos();
                    this.NavigationService.Navigate(solicitud_repuestos);
                }
                else
                {
                    MessageBox.Show("Contraseña Incorrecta");
                }
            }
            catch
            {
                MessageBox.Show("Usuario Incorrecto");
                cn.Close();
            }
            }
            catch
            {
                MessageBox.Show("Hay un campo que no se ha llenado");
            }

        }

        #endregion
    }
}

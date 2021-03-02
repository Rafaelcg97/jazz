using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Xml;

namespace Production_control_1._0
{
    /// <summary>
    /// Interaction logic for configuraciones.xaml
    /// </summary>
    public partial class configuraciones : Page
    {
        #region datos_iniciales
        public configuraciones()
        {
            InitializeComponent();
            server.Text = ConfigurationManager.AppSettings["servidor_ing"];
            user.Text = ConfigurationManager.AppSettings["usuario_ing"];
            pass.Text = ConfigurationManager.AppSettings["pass_ing"];
            b_ingenieria.Text = ConfigurationManager.AppSettings["base_ing"];
            b_smed.Text = ConfigurationManager.AppSettings["base_smed"];
            b_prod.Text = ConfigurationManager.AppSettings["base_produccion"];
            b_manto.Text = ConfigurationManager.AppSettings["base_manto"];
            imag.Text = ConfigurationManager.AppSettings["imagenes"];
        }
        #endregion

        #region control_general_del_programa

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            inicio inicio = new inicio();
            this.NavigationService.Navigate(inicio);
        }

        #endregion

        #region modificar_servidor
        private void mod_server_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = server.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "servidor_ing")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Servidor actualizado");
        }

        #endregion

        #region modificar_usuario
        private void mod_user_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = user.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "usuario_ing")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Usuario actualizado");
        }

        #endregion

        #region modificar_contrasena
        private void mod_pass_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = pass.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "pass_ing")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Contraseña actualizada");
        }
        #endregion

        #region base_de_ingenieria
        private void mod_bing_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = b_ingenieria.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "base_ing")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Base de ingenieria actualizada");
        }
        #endregion

        #region base_smed

        private void mod_bsmed_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = b_smed.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "base_smed")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Base de smed actualizada");
        }

        #endregion

        #region base_de_produccion
        private void mod_bprod_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = b_prod.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "base_produccion")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
        }

        #endregion

        #region base_de_mantenimiento

        private void mod_bmanto_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = b_manto.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "base_manto")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
            MessageBox.Show("Base de mantenimiento actualizada");
        }

        #endregion

        #region modificar_origen_de_imagenes

        private void mod_imag_Click(object sender, RoutedEventArgs e)
        {
            string nuevo = imag.Text.Trim();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "imagenes")
                            node.Attributes[1].Value = nuevo;
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");

            MessageBox.Show("Origen de imagenes actualizado");
        }

        #endregion


    }
}

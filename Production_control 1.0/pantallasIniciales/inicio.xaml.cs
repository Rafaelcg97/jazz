using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using LiveCharts;
using LiveCharts.Wpf;

namespace Production_control_1._0.pantallasIniciales
{
    public partial class inicio : UserControl
    {
        #region datosIniciales
        public inicio()
        {
            InitializeComponent();
            labelHora.Content = DateTime.Now.ToLongDateString();
            string nombre= System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            nombre = nombre.Substring(7,nombre.Length-7);
            labelUsuario.Content = nombre;
            areaDeTrabajo.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);
        }
        #endregion

        private void color1_Checked(object sender, RoutedEventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "colorInicial") { node.Attributes[1].Value = "#FF47506C";}
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
            areaDeTrabajo.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);
        }
        private void color2_Checked(object sender, RoutedEventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "colorInicial") { node.Attributes[1].Value = "#FF084D21"; }
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
            areaDeTrabajo.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);
        }
        private void color3_Checked(object sender, RoutedEventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "colorInicial") { node.Attributes[1].Value = "#FF575656"; }
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
            areaDeTrabajo.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);

        }
        private void color4_Checked(object sender, RoutedEventArgs e)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "colorInicial") { node.Attributes[1].Value = "#FF6C0F0F"; }
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");
            areaDeTrabajo.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(ConfigurationManager.AppSettings["colorInicial"]);

        }
    }
}

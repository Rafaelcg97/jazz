using System;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Xml;

namespace Production_control_1._0.pantallasIniciales
{
    public partial class configuracion : UserControl
    {
        #region datos_iniciales
        public configuracion()
        {
            InitializeComponent();

            //rellenar los textbox con los datos de conexion configurados (actual estado)
            TextBoxServidor.Text = ConfigurationManager.AppSettings["servidor_ing"];
            TextBoxUsuario.Text = ConfigurationManager.AppSettings["usuario_ing"];
            PasswordBoxContrasena.Password = ConfigurationManager.AppSettings["pass_ing"];
            textBoxIngenieria.Text = ConfigurationManager.AppSettings["base_ing"];
            textBoxSmed.Text = ConfigurationManager.AppSettings["base_smed"];
            textBoxProduccion.Text = ConfigurationManager.AppSettings["base_produccion"];
            textBoxMantenimiento.Text = ConfigurationManager.AppSettings["base_manto"];
            textBoxBalance.Text = ConfigurationManager.AppSettings["base_balances"];
            textBoxGaleria.Text = ConfigurationManager.AppSettings["imagenes"];
            passwordBoxAdministrador.Password = ConfigurationManager.AppSettings["administrador"];

            //marcar los checkbox con los datos de permisos configurados (actual estado)
            if (ConfigurationManager.AppSettings["areaProduccion"] == "1") { CheckBoxProduccion.IsChecked = true; } else { CheckBoxProduccion.IsChecked = false; }
            if (ConfigurationManager.AppSettings["areaMantenimiento"] == "1") { CheckBoxMantenimiento.IsChecked = true; } else { CheckBoxMantenimiento.IsChecked = false; }
            if (ConfigurationManager.AppSettings["areaIngenieria"] == "1") { CheckBoxIngenieria.IsChecked = true; } else { CheckBoxIngenieria.IsChecked = false; }
            if (ConfigurationManager.AppSettings["areaMateriales"] == "1") { CheckBoxMateriales.IsChecked = true; } else { CheckBoxMateriales.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria1"] == "1") { CheckBoxSubCategoria1.IsChecked = true; } else { CheckBoxSubCategoria1.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria2"] == "1") { CheckBoxSubCategoria2.IsChecked = true; } else { CheckBoxSubCategoria2.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria3"] == "1") { CheckBoxSubCategoria3.IsChecked = true; } else { CheckBoxSubCategoria3.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria4"] == "1") { CheckBoxSubCategoria4.IsChecked = true; } else { CheckBoxSubCategoria4.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria5"] == "1") { CheckBoxSubCategoria5.IsChecked = true; } else { CheckBoxSubCategoria5.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria6"] == "1") { CheckBoxSubCategoria6.IsChecked = true; } else { CheckBoxSubCategoria6.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria7"] == "1") { CheckBoxSubCategoria7.IsChecked = true; } else { CheckBoxSubCategoria7.IsChecked = false; }
            if (ConfigurationManager.AppSettings["subCategoria8"] == "1") { CheckBoxSubCategoria8.IsChecked = true; } else { CheckBoxSubCategoria8.IsChecked = false; }
        }
        #endregion

        #region guardar
        private void ButtonGuardar_Click(object sender, RoutedEventArgs e)
        {
            //definir valores a gardar por los checks
            string habilitarProduccion;
            string habilitarMantenimiento;
            string habilitarMateriales;
            string habilitarSubCategoria1;
            string habilitarSubCategoria2;
            string habilitarSubCategoria3;
            string habilitarSubCategoria4;
            string habilitarSubCategoria5;
            string habilitarSubCategoria6;
            string habilitarSubCategoria7;
            string habilitarSubCategoria8;
            string habilitarIngenieria;


            if (CheckBoxProduccion.IsChecked == true) { habilitarProduccion = "1"; } else { habilitarProduccion = "0"; }
            if (CheckBoxMantenimiento.IsChecked == true) { habilitarMantenimiento = "1"; } else { habilitarMantenimiento = "0"; }
            if (CheckBoxMateriales.IsChecked == true) { habilitarMateriales = "1"; } else { habilitarMateriales = "0"; }
            if (CheckBoxIngenieria.IsChecked == true) { habilitarIngenieria = "1"; } else { habilitarIngenieria = "0"; }
            if (CheckBoxSubCategoria1.IsChecked == true) { habilitarSubCategoria1 = "1"; } else { habilitarSubCategoria1 = "0"; }
            if (CheckBoxSubCategoria2.IsChecked == true) { habilitarSubCategoria2 = "1"; } else { habilitarSubCategoria2 = "0"; }
            if (CheckBoxSubCategoria3.IsChecked == true) { habilitarSubCategoria3 = "1"; } else { habilitarSubCategoria3 = "0"; }
            if (CheckBoxSubCategoria4.IsChecked == true) { habilitarSubCategoria4 = "1"; } else { habilitarSubCategoria4 = "0"; }
            if (CheckBoxSubCategoria5.IsChecked == true) { habilitarSubCategoria5 = "1"; } else { habilitarSubCategoria5 = "0"; }
            if (CheckBoxSubCategoria6.IsChecked == true) { habilitarSubCategoria6 = "1"; } else { habilitarSubCategoria6 = "0"; }
            if (CheckBoxSubCategoria7.IsChecked == true) { habilitarSubCategoria7 = "1"; } else { habilitarSubCategoria7 = "0"; }
            if (CheckBoxSubCategoria8.IsChecked == true) { habilitarSubCategoria8 = "1"; } else { habilitarSubCategoria8 = "0"; }


            //guardar datos en el archivo de configuraciones del programa



            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            foreach (XmlElement element in xmldoc.DocumentElement)
            {
                if (element.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in element.ChildNodes)
                    {

                        if (node.Attributes[0].Value == "servidor_ing") { node.Attributes[1].Value = TextBoxServidor.Text; }
                        if (node.Attributes[0].Value == "usuario_ing") { node.Attributes[1].Value = TextBoxUsuario.Text; }
                        if (node.Attributes[0].Value == "pass_ing") { node.Attributes[1].Value = PasswordBoxContrasena.Password; }
                        if (node.Attributes[0].Value == "base_ing") { node.Attributes[1].Value = textBoxIngenieria.Text; }
                        if (node.Attributes[0].Value == "base_manto") { node.Attributes[1].Value = textBoxMantenimiento.Text; }
                        if (node.Attributes[0].Value == "base_smed") { node.Attributes[1].Value = textBoxSmed.Text; }
                        if (node.Attributes[0].Value == "base_balances") { node.Attributes[1].Value = textBoxBalance.Text; }
                        if (node.Attributes[0].Value == "base_produccion") { node.Attributes[1].Value = textBoxProduccion.Text; }
                        if (node.Attributes[0].Value == "imagenes") { node.Attributes[1].Value = textBoxGaleria.Text; }
                        if (node.Attributes[0].Value == "areaProduccion") { node.Attributes[1].Value = habilitarProduccion; }
                        if (node.Attributes[0].Value == "areaMantenimiento") { node.Attributes[1].Value = habilitarMantenimiento; }
                        if (node.Attributes[0].Value == "areaMateriales") { node.Attributes[1].Value = habilitarMateriales; }
                        if (node.Attributes[0].Value == "areaIngenieria") { node.Attributes[1].Value = habilitarIngenieria; }
                        if (node.Attributes[0].Value == "subCategoria1") { node.Attributes[1].Value = habilitarSubCategoria1; }
                        if (node.Attributes[0].Value == "subCategoria2") { node.Attributes[1].Value = habilitarSubCategoria2; }
                        if (node.Attributes[0].Value == "subCategoria3") { node.Attributes[1].Value = habilitarSubCategoria3; }
                        if (node.Attributes[0].Value == "subCategoria4") { node.Attributes[1].Value = habilitarSubCategoria4; }
                        if (node.Attributes[0].Value == "subCategoria5") { node.Attributes[1].Value = habilitarSubCategoria5; }
                        if (node.Attributes[0].Value == "subCategoria6") { node.Attributes[1].Value = habilitarSubCategoria6; }
                        if (node.Attributes[0].Value == "subCategoria7") { node.Attributes[1].Value = habilitarSubCategoria7; }
                        if (node.Attributes[0].Value == "subCategoria8") { node.Attributes[1].Value = habilitarSubCategoria8; }
                        if (node.Attributes[0].Value == "administrador") { node.Attributes[1].Value = passwordBoxAdministrador.Password; }
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");

            //notificar que se han actualizado los datos

            MessageBox.Show("Datos Actualizados");

        }
        #endregion
    }
}

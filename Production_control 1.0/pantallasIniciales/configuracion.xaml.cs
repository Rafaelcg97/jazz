using System;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.Xml;
using System.Data.SqlClient;
using Production_control_1._0.clases;
using System.Collections.Generic;
using System.Windows.Input;

namespace Production_control_1._0.pantallasIniciales
{
    public partial class configuracion : UserControl
    {
        #region variablesListas
        SqlConnection cnIngenieria = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_ing"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        SqlConnection cnManto = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        SqlConnection cnProduccion = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_produccion"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
        List<usuario> usuariosTotales = new List<usuario>();
        List<string> cargos_ = new List<string>();
        List<string> modulos_ = new List<string>();
        int[] niveles_ = new int[2];
        string cargo_ = "";
        #endregion
        #region datos_iniciales
        public configuracion(string cargo)
        {
            InitializeComponent();
            cargo_ = cargo;
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
            niveles_[0] = 0;
            niveles_[1] = 1;
            cargos_.Add("SOPORTE");
            cargos_.Add("MECANICO");
            cargos_.Add("INGENIERO");
            cargos_.Add("COORDINADOR");
            cargos_.Add("LEAN");
            cargos_.Add("ADMINISTRADOR1");
            cargos_.Add("ADMINISTRADOR2");
            cargos_.Add("ADMINISTRADORGENERAL");
            consultarIngenieros();
            consultarModulos();
            consultar(cargo);
            consultarSolicitudes();
        }
        #endregion
        #region guardar
        private void ButtonGuardar_Click(object sender, RoutedEventArgs e)
        {
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
                    }
                }
            }
            xmldoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            ConfigurationManager.RefreshSection("appSettings");

            //notificar que se han actualizado los datos

            MessageBox.Show("Datos Actualizados");

        }
        #endregion
        #region asignarPermisos
        private void buttonAgregar_Click(object sender, RoutedEventArgs e)
        {
            usuariosTotales.Add(new usuario {});
            listViewAsignarUsuarios.Items.Add(new usuario {cargos=cargos_.ToArray(), niveles=niveles_ });
        }
        private void textBoxBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            listViewAsignarUsuarios.Items.Clear();
            if (String.IsNullOrEmpty(textBoxBuscar.Text.Trim()) == false)
            {
                foreach (usuario item in usuariosTotales)
                {
                    if (item.nombre.StartsWith(textBoxBuscar.Text.Trim()))
                    {
                        listViewAsignarUsuarios.Items.Add(item);
                    }
                }
            }
            else if (textBoxBuscar.Text.Trim() == "")
            {
                foreach (usuario item in usuariosTotales)
                {
                   listViewAsignarUsuarios.Items.Add(item);
                }
            }

        }
        private void listViewAsignarUsuarios_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // eliminar el operario con ctrl y d
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                MessageBoxResult result = MessageBox.Show("¿Desea Eliminar los Usuarios Seleccionados?", "Jazz-CCO", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        cnIngenieria.Open();
                        foreach (usuario item in listViewAsignarUsuarios.SelectedItems)
                        {
                            string sql = "delete from usuarios where id='" + item.id + "'";
                            SqlCommand cm = new SqlCommand(sql, cnIngenieria);
                            cm.ExecuteNonQuery();
                        }
                        cnIngenieria.Close();
                        consultar(cargo_);
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
        private void consultar(string cargo)
        {
            //dependiendo de quien sea el que entre se cargan las listas
            string sql = "select id, codigo, nombre, nivel, cargo, contrasena, produccion, mantenimiento, bodega, [ingenieria/SMED] as ingenieria from usuarios";
            if (cargo == "ADMINISTRADOR1")
            {
                sql = "select id, codigo, nombre, nivel, cargo, contrasena, produccion, mantenimiento, bodega, [ingenieria/SMED] as ingenieria from usuarios where cargo='INGENIERO' or cargo='SOPORTE' or cargo='COORDINADOR'";
                cargos_.Clear();
                cargos_.Add("SOPORTE");
                cargos_.Add("INGENIERO");
                cargos_.Add("COORDINADOR");
                ordenMoulos.IsEnabled = false;
            }
            else if (cargo == "ADMINISTRADOR2")
            {
                sql = "select id, codigo, nombre, nivel, cargo, contrasena, produccion, mantenimiento, bodega, [ingenieria/SMED] as ingenieria from usuarios where cargo='MECANICO'";
                cargos_.Clear();
                cargos_.Add("MECANICO");
                ordenIngenieros.IsEnabled = false;
            }
            bool ingenieria_ = false;
            bool produccion_ = false;
            bool bodega_ = false;
            bool mantenimiento_ = false;
            usuariosTotales.Clear();
            cnIngenieria.Open();
            SqlCommand cm = new SqlCommand(sql, cnIngenieria);
            SqlDataReader dr = cm.ExecuteReader();

            // se llenan la lista de modulos con los datos de la consulta
            while (dr.Read())
            {
                if (Convert.ToInt32(dr["ingenieria"]) == 1) { ingenieria_ = true; } else { ingenieria_ = false; }
                if (Convert.ToInt32(dr["produccion"]) == 1) { produccion_ = true; } else { produccion_ = false; }
                if (Convert.ToInt32(dr["bodega"]) == 1) { bodega_ = true; } else { bodega_ = false; }
                if (Convert.ToInt32(dr["mantenimiento"]) == 1) { mantenimiento_ = true; } else { mantenimiento_ = false; }
                usuariosTotales.Add(new usuario { id = Convert.ToInt32(dr["id"]), codigo = Convert.ToInt32(dr["codigo"]), nombre = dr["nombre"].ToString(), nivel = Convert.ToInt32(dr["nivel"]), cargo = dr["cargo"].ToString(), contrasenia = dr["contrasena"].ToString(), produccion = produccion_, mantenimiento = mantenimiento_, bodega = bodega_, ingenieria = ingenieria_, niveles = niveles_, cargos = cargos_.ToArray() });
            };
            //se termina la conexion a la base
            dr.Close();
            cnIngenieria.Close();
            listViewAsignarUsuarios.Items.Clear();
            foreach (usuario item in usuariosTotales)
            {
                listViewAsignarUsuarios.Items.Add(item);
            }
        }
        private void consultarIngenieros()
        {
            string sql = "select modulo, coordinadorNombre, coordinadorCodigo, ingenieroProcesosNombre, ingenieroProcesosCodigo from modulosProduccion";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            listViewOrdenarIngenieros.Items.Clear();
            modulos_.Clear();
            while (dr.Read())
            {
                listViewOrdenarIngenieros.Items.Add(new moduloAdministrado { modulo = dr["modulo"].ToString(), coordinadorNombre = dr["coordinadorNombre"].ToString(), coordinadorCodigo = Convert.ToInt32(dr["coordinadorCodigo"] is DBNull ? 0 : dr["coordinadorCodigo"]), ingenieroNombre = dr["ingenieroProcesosNombre"].ToString(), ingenieroCodigo = Convert.ToInt32(dr["ingenieroProcesosCodigo"] is DBNull ? 0 : dr["ingenieroProcesosCodigo"]) });
                modulos_.Add(dr["modulo"].ToString());
            };
            //se termina la conexion a la base
            dr.Close();
            cnProduccion.Close();
        }
        private void consultarModulos()
        {
            string sql = "select id, modulo from orden_modulos";
            cnManto.Open();
            SqlCommand cm = new SqlCommand(sql, cnManto);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            listViewOrdenarModulos.Items.Clear();
            while (dr.Read())
            {
                listViewOrdenarModulos.Items.Add(new moduloAdministrado { id = Convert.ToInt32(dr["id"]), modulo = dr["modulo"].ToString(), modulos = modulos_ });
            };
            //se termina la conexion a la base
            dr.Close();
            cnManto.Close();

        }
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControlRegistrosPermisos.SelectedIndex == 1 && cargo_=="ADMINISTRADOR1")
            {
                MessageBox.Show("Usted No tiene Permiso Para realiza Cambios en esta sección");
                tabControlRegistrosPermisos.SelectedIndex = 0;
            }
            else if (tabControlRegistrosPermisos.SelectedIndex == 2 && cargo_ == "ADMINISTRADOR2")
            {
                MessageBox.Show("Usted No tiene Permiso Para realiza Cambios en esta sección");
                tabControlRegistrosPermisos.SelectedIndex = 0;
            }
        }
        private void consultarSolicitudes()
        {
            string sql = "select numero_solicitud, modulo, apertura, cierre, minutos_pausa from resumen_tiempos_por_solicitud where minutos_pausa<0";
            cnManto.Open();
            SqlCommand cm = new SqlCommand(sql, cnManto);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            listViewSolicitudes.Items.Clear();
            while (dr.Read())
            {
                listViewSolicitudes.Items.Add(new solicitudMaquina { id_solicitud = Convert.ToInt32(dr["numero_solicitud"]), modulo = dr["modulo"].ToString(), hora_apertura=dr["apertura"].ToString(), hora_cierre=dr["cierre"].ToString(), tiempoPausa=Convert.ToDouble(dr["minutos_pausa"]) });
            };
            //se termina la conexion a la base
            dr.Close();
            cnManto.Close();

        }

        #endregion
    }
}

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
        int numSolicitud_ = 0;
        int numSolicitud_2 = 0;
        List<usuario> usuariosTotales = new List<usuario>();
        List<string> cargos_ = new List<string>();
        List<string> modulos_ = new List<string>();
        List<string> coordinadores_ = new List<string>();
        List<string> ingenieros_ = new List<string>();
        List<string> soportes_ = new List<string>();
        int[] acciones_ = new int[2];
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
            acciones_[0] = 1;
            acciones_[1] = -1;
            cargos_.Add("SOPORTE");
            cargos_.Add("MECANICO");
            cargos_.Add("ELECTRICISTA");
            cargos_.Add("INGENIERO");
            cargos_.Add("COORDINADOR");
            cargos_.Add("LEAN");
            cargos_.Add("ADMINISTRADOR1");
            cargos_.Add("ADMINISTRADOR2");
            cargos_.Add("ADMINISTRADORGENERAL");
            consultar(cargo);
            actualizarListas();
            consultarModulos();
            consultarIngenieros();
            consultarSolicitudes();
            consultarSolicitudesMecanicos();
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

            guardarUusario();
            guardarOrdenModulo();
            guardarIngenieros();
            guardarPausas();
            guardarAccionMeca();
            //notificar que se han actualizado los datos
            MessageBox.Show("Datos Actualizados");

        }
        private void guardarUusario()
        {
            cnIngenieria.Open();
            foreach (usuario item in listViewAsignarUsuarios.Items)
            {
                int produccion_ = 0;
                int mantenimiento_ = 0;
                int bodega_ = 0;
                int ingenieria_ = 0;
                if (item.produccion == true) { produccion_ = 1; } else { produccion_ = 0; }
                if (item.bodega == true) { bodega_ = 1; } else { bodega_ = 0; }
                if (item.mantenimiento == true) { mantenimiento_ = 1; } else { mantenimiento_ = 0; }
                if (item.ingenieria == true) { ingenieria_ = 1; } else { ingenieria_ = 0; }

                string sql = "update usuarios set codigo='"+item.codigo+"', nombre='"+item.nombre+"', nivel='"+item.nivel+"', cargo='" + item.cargo+"', contrasena='"+item.contrasenia+"', produccion='"+produccion_+"', mantenimiento='"+mantenimiento_+"', bodega='" + bodega_+"', [ingenieria/SMED]='"+ingenieria_+"' where id='" + item.id +"'";
                if (item.id == 0)
                {
                    sql = "insert into usuarios (codigo, nombre, nivel, cargo, contrasena, produccion, mantenimiento, bodega, [ingenieria/SMED]) values('"+item.codigo+"', '"+item.nombre+"', '" + item.nivel+"', '"+item.cargo+"', '"+item.contrasenia+"', '"+produccion_+"', '"+mantenimiento_+"', '"+bodega_+"', '"+ingenieria_+"')";
                }
                SqlCommand cm = new SqlCommand(sql, cnIngenieria);
                cm.ExecuteNonQuery();
            }
            cnIngenieria.Close();
        }
        private void guardarOrdenModulo()
        {
            cnManto.Open();
            foreach (moduloAdministrado item in listViewOrdenarModulos.Items)
            {
                string sql = "update orden_modulos set modulo='" + item.modulo + "' where id='" + item.id + "'";
                SqlCommand cm = new SqlCommand(sql, cnManto);
                cm.ExecuteNonQuery();
            }
            cnManto.Close();
        }
        private void guardarIngenieros()
        {
            cnProduccion.Open();
            foreach (moduloAdministrado item in listViewOrdenarIngenieros.Items)
            {
                string sql = "update modulosProduccion set coordinadorNombre='" + item.coordinadorNombre + "', coordinadorCodigo='" + item.coordinadorCodigo + "', ingenieroProcesosNombre='"+item.ingenieroNombre+"', ingenieroProcesosCodigo='"+item.ingenieroCodigo+"', soporteNombre='"+item.soporteNombre+"', soporteCodigo='"+item.soporteCodigo+"'  where id='" + item.id + "'";
                SqlCommand cm = new SqlCommand(sql, cnProduccion);
                cm.ExecuteNonQuery();
            }
            cnProduccion.Close();
        }
        private void guardarPausas()
        {
            cnManto.Open();
            foreach (accionSolicitud item in listViewAccionesSolicitudes.Items)
            {
                string sql = "update pausas set num_solicitud='"+item.num_solicitud+"', hora='"+item.hora+"', tipo='"+item.tipo+"' where id='"+item.id+"'";
                if (item.id == 0)
                {
                    sql = "insert into pausas(num_solicitud, hora, tipo) values('" + item.num_solicitud +"', '" + item.hora + "', '" + item.tipo + "')";
                }
                SqlCommand cm = new SqlCommand(sql, cnManto);
                cm.ExecuteNonQuery();
            }
            cnManto.Close();
        }
        private void guardarAccionMeca()
        {
            cnManto.Open();
            foreach (accionSolicitud item in listViewAccionesMecanico.Items)
            {
                string sql = "update tiempos_por_mecanico set num_solicitud='" + item.num_solicitud + "', hora='" + item.hora + "', tipo='" + item.tipo + "',  mecanico='" + item.codigo +"' where id='" + item.id + "'";
                if (item.id == 0)
                {
                    sql = "insert into tiempos_por_mecanico(num_solicitud, mecanico, hora, tipo) values('" + item.num_solicitud + "', '"+ item.codigo +"', '" + item.hora + "', '" + item.tipo + "')";
                }
                SqlCommand cm = new SqlCommand(sql, cnManto);
                cm.ExecuteNonQuery();
            }
            cnManto.Close();
            consultar(cargo_);

        }
        #endregion
        #region consultarDatosTodos
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
                    if (string.IsNullOrEmpty(item.nombre) == false)
                    {
                        if (item.nombre.StartsWith(textBoxBuscar.Text.Trim()))
                        {
                            listViewAsignarUsuarios.Items.Add(item);
                        }

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
                arreglarSolicitudes.IsEnabled = false;
                arreglarSolicitudesMecanicos.IsEnabled = false;
                
            }
            else if (cargo == "ADMINISTRADOR2")
            {
                sql = "select id, codigo, nombre, nivel, cargo, contrasena, produccion, mantenimiento, bodega, [ingenieria/SMED] as ingenieria from usuarios where cargo='MECANICO'";
                cargos_.Clear();
                cargos_.Add("MECANICO");
                cargos_.Add("ELECTRICISTA");
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
                if (Convert.ToInt32(dr["ingenieria"] is DBNull ? 0 : dr["ingenieria"]) == 1) { ingenieria_ = true; } else { ingenieria_ = false; }
                if (Convert.ToInt32(dr["produccion"] is DBNull ? 0 : dr["produccion"]) == 1) { produccion_ = true; } else { produccion_ = false; }
                if (Convert.ToInt32(dr["bodega"] is DBNull ? 0 : dr["bodega"]) == 1) { bodega_ = true; } else { bodega_ = false; }
                if (Convert.ToInt32(dr["mantenimiento"] is DBNull ? 0 : dr["mantenimiento"]) == 1) { mantenimiento_ = true; } else { mantenimiento_ = false; }
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
            string sql = "select id, modulo, coordinadorNombre, coordinadorCodigo, ingenieroProcesosNombre, ingenieroProcesosCodigo, soporteCodigo, soporteNombre from modulosProduccion";
            cnProduccion.Open();
            SqlCommand cm = new SqlCommand(sql, cnProduccion);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            listViewOrdenarIngenieros.Items.Clear();
            modulos_.Clear();
            while (dr.Read())
            {
                listViewOrdenarIngenieros.Items.Add(new moduloAdministrado {id=Convert.ToInt32(dr["id"]), modulo = dr["modulo"].ToString(), soportes=soportes_, ingenieros=ingenieros_, coordinadores=coordinadores_, coordinadorNombre = dr["coordinadorNombre"].ToString(), coordinadorCodigo = Convert.ToInt32(dr["coordinadorCodigo"] is DBNull ? 0 : dr["coordinadorCodigo"]), ingenieroNombre = dr["ingenieroProcesosNombre"].ToString(), ingenieroCodigo = Convert.ToInt32(dr["ingenieroProcesosCodigo"] is DBNull ? 0 : dr["ingenieroProcesosCodigo"]), soporteNombre=dr["soporteNombre"].ToString(), soporteCodigo=Convert.ToInt32(dr["soporteCodigo"] is DBNull? 0 : dr["soporteCodigo"] ) });
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
            string sql = "select numero_solicitud, modulo, problema_reportado, apertura, cierre, minutos_pausa from resumen_tiempos_por_solicitud where minutos_pausa<0";
            cnManto.Open();
            SqlCommand cm = new SqlCommand(sql, cnManto);
            SqlDataReader dr = cm.ExecuteReader();
            // se llenan la lista de modulos con los datos de la consulta
            listViewSolicitudes.Items.Clear();
            listViewAccionesSolicitudes.Items.Clear();
            while (dr.Read())
            {
                listViewSolicitudes.Items.Add(new solicitudMaquina { id_solicitud = Convert.ToInt32(dr["numero_solicitud"]), modulo = dr["modulo"].ToString(), problema_reportado=dr["problema_reportado"].ToString(), hora_apertura=dr["apertura"].ToString(), hora_cierre=dr["cierre"].ToString(), tiempoPausa=Convert.ToDouble(dr["minutos_pausa"]) });
            };
            //se termina la conexion a la base
            dr.Close();
            cnManto.Close();
        }
        private void consultarSolicitudesMecanicos()
        {
            string sql = "select num_solicitud, codigo, nombre, tiempo_por_solicitud from tiempo_consolidado_por_mecanico where tiempo_por_solicitud>10000";
            cnManto.Open();
            SqlCommand cm = new SqlCommand(sql, cnManto);
            SqlDataReader dr = cm.ExecuteReader();
            listViewSolicitudesMecanicos.Items.Clear();
            listViewAccionesMecanico.Items.Clear();
            while (dr.Read())
            {
                listViewSolicitudesMecanicos.Items.Add(new accionSolicitud {num_solicitud = Convert.ToInt32(dr["num_solicitud"]), codigo=Convert.ToInt32(dr["codigo"]), nombre=dr["nombre"].ToString(), tiempor_por_solicitud=Convert.ToDouble(dr["tiempo_por_solicitud"]) });
            };
            //se termina la conexion a la base
            dr.Close();
            cnManto.Close();
        }
        #endregion
        #region actualizarListas
        private void actualizarListas()
        {
            coordinadores_.Clear();
            ingenieros_.Clear();
            foreach (usuario item in listViewAsignarUsuarios.Items)
            {
                if (item.cargo == "COORDINADOR")
                {
                    coordinadores_.Add(item.nombre);
                }
                else if (item.cargo == "INGENIERO")
                {
                    ingenieros_.Add(item.nombre);
                }
                else if (item.cargo == "SOPORTE")
                {
                    soportes_.Add(item.nombre);
                }
            }
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                List<moduloAdministrado> listaAuxiliar = new List<moduloAdministrado>();
                //validar codigo de coordinadores
                foreach (moduloAdministrado item in listViewOrdenarIngenieros.Items)
                {
                    bool agregado = false;
                    foreach (usuario subitem in listViewAsignarUsuarios.Items)
                    {
                        if (item.coordinadorNombre == subitem.nombre)
                        {
                            listaAuxiliar.Add(new moduloAdministrado { id = item.id, coordinadorNombre = item.coordinadorNombre, coordinadorCodigo = subitem.codigo, ingenieroNombre = item.ingenieroNombre, ingenieroCodigo = item.ingenieroCodigo, ingenieros = item.ingenieros, coordinadores = item.coordinadores, modulo = item.modulo, soporteCodigo=item.soporteCodigo, soporteNombre=item.soporteNombre, soportes=item.soportes });
                            agregado = true;
                        }
                    }

                    if (agregado == false)
                    {
                        listaAuxiliar.Add(item);
                    }
                }

                listViewOrdenarIngenieros.Items.Clear();
                foreach (moduloAdministrado item in listaAuxiliar)
                {
                    listViewOrdenarIngenieros.Items.Add(item);
                }

            }
        }
        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                List<moduloAdministrado> listaAuxiliar = new List<moduloAdministrado>();
                //validar codigo de coordinadores
                foreach (moduloAdministrado item in listViewOrdenarIngenieros.Items)
                {
                    bool agregado = false;
                    foreach (usuario subitem in listViewAsignarUsuarios.Items)
                    {
                        if (item.ingenieroNombre == subitem.nombre)
                        {
                            listaAuxiliar.Add(new moduloAdministrado { id = item.id, coordinadorNombre = item.coordinadorNombre, coordinadorCodigo = item.coordinadorCodigo, ingenieroNombre = item.ingenieroNombre, ingenieroCodigo = subitem.codigo, ingenieros = item.ingenieros, coordinadores = item.coordinadores, modulo = item.modulo, soporteCodigo = item.soporteCodigo, soporteNombre = item.soporteNombre, soportes = item.soportes });
                            agregado = true;
                        }
                    }
                    if (agregado == false)
                    {
                        listaAuxiliar.Add(item);
                    }
                }
                listViewOrdenarIngenieros.Items.Clear();
                foreach (moduloAdministrado item in listaAuxiliar)
                {
                    listViewOrdenarIngenieros.Items.Add(item);
                }
            }
        }
        private void ComboBox_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).SelectedIndex > -1)
            {
                List<moduloAdministrado> listaAuxiliar = new List<moduloAdministrado>();
                //validar codigo de coordinadores
                foreach (moduloAdministrado item in listViewOrdenarIngenieros.Items)
                {
                    bool agregado = false;
                    foreach (usuario subitem in listViewAsignarUsuarios.Items)
                    {
                        if (item.soporteNombre == subitem.nombre)
                        {
                            listaAuxiliar.Add(new moduloAdministrado { id = item.id, coordinadorNombre = item.coordinadorNombre, coordinadorCodigo = item.coordinadorCodigo, ingenieroNombre = item.ingenieroNombre, ingenieroCodigo = item.ingenieroCodigo , ingenieros = item.ingenieros, coordinadores = item.coordinadores, modulo = item.modulo, soporteCodigo = subitem.codigo, soporteNombre = item.soporteNombre, soportes = item.soportes });
                            agregado = true;
                        }
                    }
                    if (agregado == false)
                    {
                        listaAuxiliar.Add(item);
                    }
                }
                listViewOrdenarIngenieros.Items.Clear();
                foreach (moduloAdministrado item in listaAuxiliar)
                {
                    listViewOrdenarIngenieros.Items.Add(item);
                }
            }

        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            actualizarListas();
            consultarIngenieros();
        }
        private void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            actualizarListas();
            consultarIngenieros();
        }
        #endregion
        #region consultaSolicitudes
        private void buttonAgregarAccionSolicitud_Click(object sender, RoutedEventArgs e)
        {
            if (listViewSolicitudes.SelectedIndex > -1)
            {
                listViewAccionesSolicitudes.Items.Add(new accionSolicitud {id=0, num_solicitud=numSolicitud_, acciones=acciones_});
            }
        }
        private void listViewSolicitudes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewSolicitudes.SelectedIndex > -1)
            {
                listViewAccionesSolicitudes.Items.Clear();
                numSolicitud_ = ((solicitudMaquina)listViewSolicitudes.SelectedItem).id_solicitud;
                string sql = "select id, num_solicitud, hora, tipo from pausas where num_solicitud=" + numSolicitud_ +" order by hora";
                cnManto.Open();
                SqlCommand cm = new SqlCommand(sql, cnManto);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewAccionesSolicitudes.Items.Add(new accionSolicitud {id=Convert.ToInt32(dr["id"]), num_solicitud=Convert.ToInt32(dr["num_solicitud"]), hora=Convert.ToDateTime(dr["hora"]).ToString("yyyy-MM-dd hh:mm:ss.000"), tipo=Convert.ToInt32(dr["tipo"]), acciones=acciones_});
                };
                dr.Close();
                cnManto.Close();
            }
        }
        private void solo_numeros(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 || e.Key == Key.Tab)
                e.Handled = false;
            else
                e.Handled = true;
        }
        private void listViewAccionesSolicitudes_KeyDown(object sender, KeyEventArgs e)
        {
            // eliminar pausa con ctrl y d
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                MessageBoxResult result = MessageBox.Show("¿Desea eliminar las pausas seleccionadas?", "Jazz-CCO", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        cnManto.Open();
                        foreach (accionSolicitud item in listViewAccionesSolicitudes.SelectedItems)
                        {
                            string sql = "delete from pausas where id='" + item.id + "'";
                            SqlCommand cm = new SqlCommand(sql, cnManto);
                            cm.ExecuteNonQuery();
                        }
                        cnManto.Close();
                        consultarSolicitudes();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }
        }
        private void listViewSolicitudesMecanicos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewSolicitudesMecanicos.SelectedIndex > -1)
            {
                listViewAccionesMecanico.Items.Clear();
                numSolicitud_2 = ((accionSolicitud)listViewSolicitudesMecanicos.SelectedItem).num_solicitud;
                string sql = "select id, num_solicitud, mecanico, hora, tipo from tiempos_por_mecanico where num_solicitud=" + numSolicitud_2 + " order by hora";
                cnManto.Open();
                SqlCommand cm = new SqlCommand(sql, cnManto);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    listViewAccionesMecanico.Items.Add(new accionSolicitud { id = Convert.ToInt32(dr["id"]), num_solicitud = Convert.ToInt32(dr["num_solicitud"]), hora = Convert.ToDateTime(dr["hora"]).ToString("yyyy-MM-dd hh:mm:ss.000"), codigo=Convert.ToInt32(dr["mecanico"]), tipo = Convert.ToInt32(dr["tipo"]), acciones = acciones_ });
                };
                dr.Close();
                cnManto.Close();
            }
        }
        private void listViewAccionesMecanico_KeyDown(object sender, KeyEventArgs e)
        {
            // eliminar pausa con ctrl y d
            if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.D))
            {
                MessageBoxResult result = MessageBox.Show("¿Desea eliminar las acciones seleccionadas?", "Jazz-CCO", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        cnManto.Open();
                        foreach (accionSolicitud item in listViewAccionesMecanico.SelectedItems)
                        {
                            string sql = "delete from tiempos_por_mecanico where id='" + item.id + "'";
                            SqlCommand cm = new SqlCommand(sql, cnManto);
                            cm.ExecuteNonQuery();
                        }
                        cnManto.Close();
                        consultarSolicitudesMecanicos();
                        break;
                    case MessageBoxResult.No:
                        break;
                }
            }

        }
        private void buttonAgregarAccionSolicitudMeca_Click(object sender, RoutedEventArgs e)
        {
            if (listViewSolicitudesMecanicos.SelectedIndex > -1)
            {
                listViewAccionesMecanico.Items.Add(new accionSolicitud { id = 0, num_solicitud = numSolicitud_2, acciones = acciones_ });
            }

        }
        #endregion
    }
}

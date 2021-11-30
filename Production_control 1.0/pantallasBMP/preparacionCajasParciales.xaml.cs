using JazzCCO._0.clases;
using JazzCCO._0.pantallasKanban.NotificacionesDeTablaSQL;
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

namespace JazzCCO._0.pantallasBMP
{
    public partial class preparacionCajasParciales : Page
    {
        public preparacionCajasParciales()
        {
            InitializeComponent();
            this.CreatePermission();
            MessageModelPlanta model = new MessageModelPlanta(this.Dispatcher);
            this.DataContext = model;
        }

        private void btnAtras_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        public void CreatePermission()
        {
            // Make sure client has permissions 
            try
            {
                SqlClientPermission perm = new SqlClientPermission(System.Security.Permissions.PermissionState.Unrestricted);
                perm.Demand();
            }
            catch
            {
                throw new ApplicationException("No permission");
            }
        }

        private void btnDetalles_Click(object sender, RoutedEventArgs e)
        {
            lblSolicitud.Content = "0000";
            lstbMateriales.Items.Clear();
            if (lstPendientes.SelectedIndex > -1)
            {
                solicitudKanban itemsSeleccionado = (solicitudKanban)lstPendientes.SelectedItem;
                popUpDetalles.IsOpen = true;
                lblSolicitud.Content = itemsSeleccionado.solicitudKanbanId;

                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "select lote, material, cantidad from detalleSolicitudeKanban where solicitudKanbanId=" + itemsSeleccionado.solicitudKanbanId;
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                SqlDataReader dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    lstbMateriales.Items.Add(new solicitudKanban { lote = dr["lote"].ToString(), material = dr["material"].ToString(), cantidad = Convert.ToInt32(dr["cantidad"])});
                }
                dr.Close();
                cn.Close();
            }
        }

        private void btnTerminar_Click(object sender, RoutedEventArgs e)
        {
            if (lstPendientes.SelectedIndex > -1)
            {
                solicitudKanban itemsSeleccionado = (solicitudKanban)lstPendientes.SelectedItem;
                SqlConnection cn = new SqlConnection("Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_kanban"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"]);
                string sql = "UPDATE solicitudesKanban SET horaPreparacionCajaParcial='"+DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")+"'  where solicitudKanbanId=" + itemsSeleccionado.solicitudKanbanId;
                cn.Open();
                SqlCommand cm = new SqlCommand(sql, cn);
                cm.ExecuteNonQuery();
                cn.Close();
            }
        }

        private void btn_GotFocus(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            lstPendientes.SelectedItem = btn.DataContext;
        }

    }
}

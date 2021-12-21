using ClasesTexops;
using Microsoft.Win32;
using SQLConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
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

namespace JazzCCO._0.pantallasKanban
{
    public partial class cuadraturaEntregaMateriales : Page
    {
        public cuadraturaEntregaMateriales()
        {
            InitializeComponent();

            using (SqlConnection cn = ConexionTexopsServer.Kanban())
            {
                try
                {
                    string sql = "SELECT lote, material, inventario FROM lotesConSalidasPendientes ORDER BY lote, material";
                    SqlCommand cm = new SqlCommand(sql, cn);
                    SqlDataReader dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        lstvSalidasPendientes.Items.Add(new SolicitudKanban { NumeroLote= dr["lote"].ToString(), CodigoPartNumber= dr["material"].ToString(),  RequeridoPartnumber= Convert.ToDouble(dr["inventario"]) });
                    }
                    dr.Close();
                    //

                    sql = "SELECT lote, talla, categoria, requerido, solicitado FROM lotesConSolicitudesPendientes ORDER BY lote, categoria, talla";
                    cm = new SqlCommand(sql, cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        lstvSolicitudesPendientes.Items.Add(new SolicitudKanban { NumeroLote = dr["lote"].ToString(), TallaPartnumber=dr["talla"].ToString(), CategoryPartNumber = dr["categoria"].ToString(), RequeridoPartnumber = Convert.ToDouble(dr["requerido"]), SolicitadoPartnumber = Convert.ToDouble(dr["solicitado"]) });
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ha ocurrido un error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void salir__Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnDescargarSalidas_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("LOTES TERMINADOS CON SALIDAS PENDIENTES");
            buffer.Append("\n");
            buffer.Append("LOTE");
            buffer.Append(",");
            buffer.Append("CATEGORIA");
            buffer.Append(",");
            buffer.Append("INVENTARIO");
            buffer.Append("\n");
            #endregion
            foreach (SolicitudKanban item in lstvSalidasPendientes.Items)
            {
                buffer.Append(item.NumeroLote);
                buffer.Append(",");
                buffer.Append(item.CodigoPartNumber);
                buffer.Append(",");
                buffer.Append(item.RequeridoPartnumber);
                buffer.Append("\n");
            }
            String result = buffer.ToString();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV (*.csv)|*.csv";
                string fileName = "";
                if (saveFileDialog.ShowDialog() == true)
                {
                    fileName = saveFileDialog.FileName;
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine(result);
                    sw.Close();

                }

                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDescargarSolicitudes_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder buffer = new StringBuilder();
            #region encabezados
            buffer.Append("LOTES TERMINADOS CON SOLICITUDES PENDIENTES");
            buffer.Append("\n");
            buffer.Append("LOTE");
            buffer.Append(",");
            buffer.Append("CATEGORIA");
            buffer.Append(",");
            buffer.Append("TALLA");
            buffer.Append(",");
            buffer.Append("REQUERIDO");
            buffer.Append(",");
            buffer.Append("SOLICITADO");
            buffer.Append("\n");
            #endregion
            foreach (SolicitudKanban item in lstvSolicitudesPendientes.Items)
            {
                buffer.Append(item.NumeroLote);
                buffer.Append(",");
                buffer.Append(item.CategoryPartNumber);
                buffer.Append(",");
                buffer.Append(item.TallaPartnumber);
                buffer.Append(",");
                buffer.Append(item.RequeridoPartnumber);
                buffer.Append(",");
                buffer.Append(item.SolicitadoPartnumber);
                buffer.Append("\n");
            }
            String result = buffer.ToString();
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "CSV (*.csv)|*.csv";
                string fileName = "";
                if (saveFileDialog.ShowDialog() == true)
                {
                    fileName = saveFileDialog.FileName;
                    StreamWriter sw = new StreamWriter(fileName);
                    sw.WriteLine(result);
                    sw.Close();

                }

                Process.Start(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}

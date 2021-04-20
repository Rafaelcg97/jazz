using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Production_control_1._0.clases;
using Production_control_1._0.pantallasMantenimiento.NotificacionesDeTablaSQL;

namespace Production_control_1._0.pantallasMantenimiento.NotificacionesMantenimientoPreventivo
{
    class mensaje
    {

        private ObservableCollection<solicitudMaquina> preventivo = null;
        public ObservableCollection<solicitudMaquina> PREVENTIVO
        {
            get
            {
                preventivo = preventivo ?? new ObservableCollection<solicitudMaquina>();
                return preventivo;
            }
        }


        public Dispatcher UIDispatcher { get; set; }
        public notificacion Notifier { get; set; }
        public mensaje(Dispatcher uidispatcher)
        {
            this.UIDispatcher = uidispatcher;
            this.Notifier = new notificacion();

            this.Notifier.NewMessage += new EventHandler<SqlNotificationEventArgs>(notifier_NewMessage);
            DataTable dt = this.Notifier.RegisterDependency();
            this.LoadMessage(dt);
        }
        private void LoadMessage(DataTable consultado)
        {
            _ = this.UIDispatcher.BeginInvoke((Action)delegate ()
            {
                if (consultado != null)
                {
                    #region limpiarDatos
                    this.PREVENTIVO.Clear();

                    #endregion
                    #region agregarDatosLista
                    foreach (DataRow dr in consultado.Rows)
                    {
                            solicitudMaquina prev = new solicitudMaquina
                            {
                                id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                modulo = dr["modulo"].ToString(),
                                maquina = dr["maquina"].ToString(),
                                problema_reportado = dr["problema_reportado"].ToString(),
                                hora_reportada = Convert.ToDateTime(dr["hora_reportada"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                hora_apertura = string.IsNullOrEmpty(dr["hora_apertura"].ToString()) ? "0" : Convert.ToDateTime(dr["hora_apertura"]).ToString("yyyy-MM-dd hh:mm:ss"),
                            };
                            this.PREVENTIVO.Add(prev);
                    }
                    #endregion
                }
            });
        }

        void notifier_NewMessage(object sender, SqlNotificationEventArgs e)
        {
            this.LoadMessage(this.Notifier.RegisterDependency());
        }
    }
}

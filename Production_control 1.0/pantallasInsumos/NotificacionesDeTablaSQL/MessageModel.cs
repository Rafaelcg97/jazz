using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Data;
using System.Windows.Input;
using System.Data.SqlClient;
using Production_control_1._0.NotificacionesDeTablaSQL;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasInsumos.NotificacionesDeTablaSQL
{
    class MessageModel : INotifyPropertyChanged
    {
        private ObservableCollection<solicitudInsumo> recibidas = null;
        private ObservableCollection<solicitudInsumo> aprobadas = null;
        private ObservableCollection<solicitudInsumo> entregadas = null;
        public ObservableCollection<solicitudInsumo> Recibidas
        {
            get
            {
                recibidas = recibidas ?? new ObservableCollection<solicitudInsumo>();
                return recibidas;
            }
        }
        public ObservableCollection<solicitudInsumo> Aprobadas
        {
            get
            {
                aprobadas = aprobadas ?? new ObservableCollection<solicitudInsumo>();
                return aprobadas;
            }
        }
        public ObservableCollection<solicitudInsumo> Entregadas
        {
            get
            {
                entregadas = entregadas ?? new ObservableCollection<solicitudInsumo>();
                return entregadas;
            }
        }

        public Dispatcher UIDispatcher { get; set; }
        public SQLNotifier Notifier { get; set; }
        public MessageModel(Dispatcher uidispatcher)
        {
            this.UIDispatcher = uidispatcher;
            this.Notifier = new SQLNotifier();

            this.Notifier.NewMessage += new EventHandler<SqlNotificationEventArgs>(notifier_NewMessage);
            DataTable dt = this.Notifier.RegisterDependency();


            this.LoadMessage(dt);
        }

        private void LoadMessage(DataTable dt)
        {

            _ = this.UIDispatcher.BeginInvoke((Action)delegate ()
              {
                  if (dt != null)
                  {
                      this.Recibidas.Clear();
                      this.Entregadas.Clear();
                      this.Aprobadas.Clear();

                      foreach (DataRow drow in dt.Rows)
                      {
                          switch (drow["ordenStatus"].ToString())
                          {
                              case "Recibida":
                                  solicitudInsumo recibida = new solicitudInsumo
                                  {
                                      ordenIdNum = Convert.ToInt32(drow["orden_id"]),
                                      ordenNombreSolicitante = drow["ordenNombreSolicitante"] as string,
                                      autorizado = drow["ordenStatus"] as string,
                                      costC = Convert.ToDouble(drow["CostoTotal"]).ToString("C")
                                  };
                                  this.Recibidas.Add(recibida);
                                  break;
                              case "Aprobada":
                                  solicitudInsumo aprobada = new solicitudInsumo
                                  {
                                      ordenIdNum = Convert.ToInt32(drow["orden_id"]),
                                      ordenNombreSolicitante = drow["ordenNombreSolicitante"] as string,
                                      autorizado = drow["ordenStatus"] as string,
                                      costC = Convert.ToDouble(drow["CostoTotal"]).ToString("C")
                                  };
                                  this.Aprobadas.Add(aprobada);
                                  break;
                              case "Entregada":
                                  solicitudInsumo entregada = new solicitudInsumo
                                  {
                                      ordenIdNum = Convert.ToInt32(drow["orden_id"]),
                                      ordenNombreSolicitante = drow["ordenNombreSolicitante"] as string,
                                      autorizado = drow["ordenStatus"] as string,
                                      costC = Convert.ToDouble(drow["CostoTotal"]).ToString("C")
                                  };
                                  this.Entregadas.Add(entregada);
                                  break;
                          }
                      }
                  }
              });
        }
        void notifier_NewMessage(object sender, SqlNotificationEventArgs e)
        {
            this.LoadMessage(this.Notifier.RegisterDependency());
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

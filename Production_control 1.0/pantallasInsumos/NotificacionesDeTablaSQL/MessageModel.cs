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
        private ObservableCollection<solicitudInsumo> messages = null;
        public ObservableCollection<solicitudInsumo> Messages
        {
            get
            {
                messages = messages ?? new ObservableCollection<solicitudInsumo>();
                return messages;
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

        private int ordenId;
        private string ordenStatus;

        public int OrdenId
        {
            get
            {
                return this.ordenId;
            }
            set
            {
                this.ordenId = value;
                this.OnPropertyChanged("OrdenId");
            }
        }

        public string OrdenStatus
        {
            get
            {
                return this.ordenStatus;
            }
            set
            {
                this.ordenStatus = value;
                this.OnPropertyChanged("OrdenStatus");
            }
        }


        private void LoadMessage(DataTable dt)
        {

            this.UIDispatcher.BeginInvoke((Action)delegate ()
            {
                if (dt != null)
                {
                    this.Messages.Clear();

                    foreach (DataRow drow in dt.Rows)
                    {
                        solicitudInsumo msg = new solicitudInsumo
                        {
                            ordenIdNum = Convert.ToInt32(drow["orden_id"]),
                            ordenNombreSolicitante = drow["ordenNombreSolicitante"] as string,
                            autorizado = drow["ordenStatus"] as string,
                        };
                        this.Messages.Add(msg);
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

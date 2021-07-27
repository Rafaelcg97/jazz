using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Media;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasMantenimiento.NotificacionesDeTablaSQL
{
    class SQLNotifierPlanta
    {
        public SqlCommand CurrentCommand { get; set; }
        private SqlConnection connection;
        public SqlConnection CurrentConnection
        {
            get
            {
                this.connection = this.connection ?? new SqlConnection(this.ConnectionString);
                return this.connection;
            }
        }
        public string ConnectionString
        {
            get
            {
                return "Data Source=" + ConfigurationManager.AppSettings["servidor_ing"] + ";Initial Catalog=" + ConfigurationManager.AppSettings["base_manto"] + ";Persist Security Info=True;User ID=" + ConfigurationManager.AppSettings["usuario_ing"] + ";Password=" + ConfigurationManager.AppSettings["pass_ing"];
            }
        }
        public SQLNotifierPlanta()
        {
            SqlDependency.Start(this.ConnectionString);
        }
        private event EventHandler<SqlNotificationEventArgs> _newMessage;
        public event EventHandler<SqlNotificationEventArgs> NewMessage
        {
            add
            {
                this._newMessage += value;
            }
            remove
            {
                this._newMessage -= value;
            }
        }
        public virtual void OnNewMessage(SqlNotificationEventArgs notification)
        {
            if (this._newMessage != null)
                this._newMessage(this, notification);
        }
        public DataTable RegisterDependency()
        {

            this.CurrentCommand = new SqlCommand("select [id_solicitud], [ubicacion], [modulo], [maquina], [hora_reportada], [hora_asignacion], [hora_apertura], [hora_cierre], [problema_reportado], [corresponde], [prioridad] from dbo.solicitudes where [hora_apertura] is null or [hora_cierre] is null", this.CurrentConnection);
            this.CurrentCommand.Notification = null;


            SqlDependency dependency = new SqlDependency(this.CurrentCommand);
            dependency.OnChange += this.dependency_OnChange;


            if (this.CurrentConnection.State == ConnectionState.Closed)
                this.CurrentConnection.Open();
            try
            {

                DataTable dt = new DataTable();
                dt.Load(this.CurrentCommand.ExecuteReader(CommandBehavior.CloseConnection));
                return dt;
            }
            catch { return null; }

        }
        void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            SqlDependency dependency = sender as SqlDependency;

            dependency.OnChange -= new OnChangeEventHandler(dependency_OnChange);

            this.OnNewMessage(e);
        }
    }
}

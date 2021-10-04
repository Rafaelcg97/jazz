using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Data;
using System.Data.SqlClient;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasKanban.NotificacionesDeTablaSQL
{
    class MessageModelPlanta
    {
        #region coleccionObservablesPriv
        private ObservableCollection<solicitudKanban> u1 = null;
        private ObservableCollection<solicitudKanban> u2 = null;
        private ObservableCollection<solicitudKanban> u3 = null;
        private ObservableCollection<solicitudKanban> u4 = null;
        private ObservableCollection<solicitudKanban> u5 = null;
        private ObservableCollection<solicitudKanban> u6 = null;
        private ObservableCollection<solicitudKanban> u7 = null;
        private ObservableCollection<solicitudKanban> u8 = null;
        private ObservableCollection<solicitudKanban> u9 = null;
        private ObservableCollection<solicitudKanban> u10 = null;
        private ObservableCollection<solicitudKanban> u11 = null;
        private ObservableCollection<solicitudKanban> u12 = null;
        private ObservableCollection<solicitudKanban> u13 = null;
        private ObservableCollection<solicitudKanban> u14 = null;
        private ObservableCollection<solicitudKanban> u15 = null;
        private ObservableCollection<solicitudKanban> u16 = null;
        private ObservableCollection<solicitudKanban> u17 = null;
        private ObservableCollection<solicitudKanban> u18 = null;
        private ObservableCollection<solicitudKanban> u19 = null;
        private ObservableCollection<solicitudKanban> u20 = null;
        private ObservableCollection<solicitudKanban> u21 = null;
        private ObservableCollection<solicitudKanban> u22 = null;
        private ObservableCollection<solicitudKanban> u23 = null;
        private ObservableCollection<solicitudKanban> u24 = null;
        private ObservableCollection<solicitudKanban> u25 = null;
        private ObservableCollection<solicitudKanban> u26 = null;
        private ObservableCollection<solicitudKanban> u27 = null;
        private ObservableCollection<solicitudKanban> u28 = null;
        private ObservableCollection<solicitudKanban> u29 = null;
        private ObservableCollection<solicitudKanban> u30 = null;
        private ObservableCollection<solicitudKanban> u31 = null;
        private ObservableCollection<solicitudKanban> u32 = null;
        private ObservableCollection<solicitudKanban> p = null;
        #endregion
        #region coleccionesObservablesPub
        public ObservableCollection<solicitudKanban> U1
        {
            get
            {
                u1 = u1 ?? new ObservableCollection<solicitudKanban>();
                return u1;
            }
        }
        public ObservableCollection<solicitudKanban> U2
        {
            get
            {
                u2 = u2 ?? new ObservableCollection<solicitudKanban>();
                return u2;
            }
        }
        public ObservableCollection<solicitudKanban> U3
        {
            get
            {
                u3 = u3 ?? new ObservableCollection<solicitudKanban>();
                return u3;
            }
        }
        public ObservableCollection<solicitudKanban> U4
        {
            get
            {
                u4 = u4 ?? new ObservableCollection<solicitudKanban>();
                return u4;
            }
        }
        public ObservableCollection<solicitudKanban> U5
        {
            get
            {
                u5 = u5 ?? new ObservableCollection<solicitudKanban>();
                return u5;
            }
        }
        public ObservableCollection<solicitudKanban> U6
        {
            get
            {
                u6 = u6 ?? new ObservableCollection<solicitudKanban>();
                return u6;
            }
        }
        public ObservableCollection<solicitudKanban> U7
        {
            get
            {
                u7 = u7 ?? new ObservableCollection<solicitudKanban>();
                return u7;
            }
        }
        public ObservableCollection<solicitudKanban> U8
        {
            get
            {
                u8 = u8 ?? new ObservableCollection<solicitudKanban>();
                return u8;
            }
        }
        public ObservableCollection<solicitudKanban> U9
        {
            get
            {
                u9 = u9 ?? new ObservableCollection<solicitudKanban>();
                return u9;
            }
        }
        public ObservableCollection<solicitudKanban> U10
        {
            get
            {
                u10 = u10 ?? new ObservableCollection<solicitudKanban>();
                return u10;
            }
        }
        public ObservableCollection<solicitudKanban> U11
        {
            get
            {
                u11 = u11 ?? new ObservableCollection<solicitudKanban>();
                return u11;
            }
        }
        public ObservableCollection<solicitudKanban> U12
        {
            get
            {
                u12 = u12 ?? new ObservableCollection<solicitudKanban>();
                return u12;
            }
        }
        public ObservableCollection<solicitudKanban> U13
        {
            get
            {
                u13 = u13 ?? new ObservableCollection<solicitudKanban>();
                return u13;
            }
        }
        public ObservableCollection<solicitudKanban> U14
        {
            get
            {
                u14 = u14 ?? new ObservableCollection<solicitudKanban>();
                return u14;
            }
        }
        public ObservableCollection<solicitudKanban> U15
        {
            get
            {
                u15 = u15 ?? new ObservableCollection<solicitudKanban>();
                return u15;
            }
        }
        public ObservableCollection<solicitudKanban> U16
        {
            get
            {
                u16 = u16 ?? new ObservableCollection<solicitudKanban>();
                return u16;
            }
        }
        public ObservableCollection<solicitudKanban> U17
        {
            get
            {
                u17 = u17 ?? new ObservableCollection<solicitudKanban>();
                return u17;
            }
        }
        public ObservableCollection<solicitudKanban> U18
        {
            get
            {
                u18 = u18 ?? new ObservableCollection<solicitudKanban>();
                return u18;
            }
        }
        public ObservableCollection<solicitudKanban> U19
        {
            get
            {
                u19 = u19 ?? new ObservableCollection<solicitudKanban>();
                return u19;
            }
        }
        public ObservableCollection<solicitudKanban> U20
        {
            get
            {
                u20 = u20 ?? new ObservableCollection<solicitudKanban>();
                return u20;
            }
        }
        public ObservableCollection<solicitudKanban> U21
        {
            get
            {
                u21 = u21 ?? new ObservableCollection<solicitudKanban>();
                return u21;
            }
        }
        public ObservableCollection<solicitudKanban> U22
        {
            get
            {
                u22 = u22 ?? new ObservableCollection<solicitudKanban>();
                return u22;
            }
        }
        public ObservableCollection<solicitudKanban> U23
        {
            get
            {
                u23 = u23 ?? new ObservableCollection<solicitudKanban>();
                return u23;
            }
        }
        public ObservableCollection<solicitudKanban> U24
        {
            get
            {
                u24 = u24 ?? new ObservableCollection<solicitudKanban>();
                return u24;
            }
        }
        public ObservableCollection<solicitudKanban> U25
        {
            get
            {
                u25 = u25 ?? new ObservableCollection<solicitudKanban>();
                return u25;
            }
        }
        public ObservableCollection<solicitudKanban> U26
        {
            get
            {
                u26 = u26 ?? new ObservableCollection<solicitudKanban>();
                return u26;
            }
        }
        public ObservableCollection<solicitudKanban> U27
        {
            get
            {
                u27 = u27 ?? new ObservableCollection<solicitudKanban>();
                return u27;
            }
        }
        public ObservableCollection<solicitudKanban> U28
        {
            get
            {
                u28 = u28 ?? new ObservableCollection<solicitudKanban>();
                return u28;
            }
        }
        public ObservableCollection<solicitudKanban> U29
        {
            get
            {
                u29 = u29 ?? new ObservableCollection<solicitudKanban>();
                return u29;
            }
        }
        public ObservableCollection<solicitudKanban> U30
        {
            get
            {
                u30 = u30 ?? new ObservableCollection<solicitudKanban>();
                return u30;
            }
        }
        public ObservableCollection<solicitudKanban> U31
        {
            get
            {
                u31 = u31 ?? new ObservableCollection<solicitudKanban>();
                return u31;
            }
        }
        public ObservableCollection<solicitudKanban> U32
        {
            get
            {
                u32 = u32 ?? new ObservableCollection<solicitudKanban>();
                return u32;
            }
        }
        public ObservableCollection<solicitudKanban> P
        {
            get
            {
                p = p ?? new ObservableCollection<solicitudKanban>();
                return p;
            }
        }
        #endregion
        public Dispatcher UIDispatcher { get; set; }
        public SQLNotifierPlanta Notifier { get; set; }
        public MessageModelPlanta(Dispatcher uidispatcher)
        {
            this.UIDispatcher = uidispatcher;
            this.Notifier = new SQLNotifierPlanta();

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
                    this.U1.Clear();
                    this.U2.Clear();
                    this.U3.Clear();
                    this.U4.Clear();
                    this.U5.Clear();
                    this.U6.Clear();
                    this.U7.Clear();
                    this.U8.Clear();
                    this.U9.Clear();
                    this.U10.Clear();
                    this.U11.Clear();
                    this.U12.Clear();
                    this.U13.Clear();
                    this.U14.Clear();
                    this.U15.Clear();
                    this.U16.Clear();
                    this.U17.Clear();
                    this.U18.Clear();
                    this.U19.Clear();
                    this.U20.Clear();
                    this.U21.Clear();
                    this.U22.Clear();
                    this.U23.Clear();
                    this.U24.Clear();
                    this.U25.Clear();
                    this.U26.Clear();
                    this.U27.Clear();
                    this.U28.Clear();
                    this.U29.Clear();
                    this.U30.Clear();
                    this.U31.Clear();
                    this.U32.Clear();
                    this.P.Clear();
                    #endregion
                    #region agregarDatosLista
                    foreach (DataRow dr in consultado.Rows)
                    {
                        string color_ = "";
                        if (dr["tipo"].ToString() == "solicitud" && string.IsNullOrEmpty(dr["fechaInicio"].ToString()) && Convert.ToBoolean(dr["validadoSmed"])==true)
                        {
                            color_ = "Red";
                        }
                        else if (dr["tipo"].ToString() == "solicitud" && string.IsNullOrEmpty(dr["fechaInicio"].ToString()) && Convert.ToBoolean(dr["validadoSmed"]) == false)
                        {
                            color_ = "Orange";
                        }
                        else if (dr["tipo"].ToString() == "solicitud" && !string.IsNullOrEmpty(dr["fechaParcial"].ToString()))
                        {
                            color_ = "Yellow";
                        }
                        else if (dr["tipo"].ToString() == "devolucion" && string.IsNullOrEmpty(dr["fechaInicio"].ToString()))
                        {
                            color_ = "Blue";
                        }
                        else
                        {
                            color_ = "LightGreen";
                        }
                        switch (Convert.ToInt32(dr["ubicacion"] is DBNull ? 0 : dr["ubicacion"]))
                        {
                            case 1:
                                solicitudKanban itemU1 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio= Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0: dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U1.Add(itemU1);
                                break;
                            case 2:
                                solicitudKanban itemU2 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U2.Add(itemU2);
                                break;
                            case 3:
                                solicitudKanban itemU3 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U3.Add(itemU3);
                                break;
                            case 4:
                                solicitudKanban itemU4 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U4.Add(itemU4);
                                break;
                            case 5:
                                solicitudKanban itemU5 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U5.Add(itemU5);
                                break;
                            case 6:
                                solicitudKanban itemU6 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U6.Add(itemU6);
                                break;
                            case 7:
                                solicitudKanban itemU7 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U7.Add(itemU7);
                                break;
                            case 8:
                                solicitudKanban itemU8 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U8.Add(itemU8);
                                break;
                            case 9:
                                solicitudKanban itemU9 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U9.Add(itemU9);
                                break;
                            case 10:
                                solicitudKanban itemU10 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U10.Add(itemU10);
                                break;
                            case 11:
                                solicitudKanban itemU11 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U11.Add(itemU11);
                                break;
                            case 12:
                                solicitudKanban itemU12 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U12.Add(itemU12);
                                break;
                            case 13:
                                solicitudKanban itemU13 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U13.Add(itemU13);
                                break;
                            case 14:
                                solicitudKanban itemU14 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U14.Add(itemU14);
                                break;
                            case 15:
                                solicitudKanban itemU15 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U15.Add(itemU15);
                                break;
                            case 16:
                                solicitudKanban itemU16 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U16.Add(itemU16);
                                break;
                            case 17:
                                solicitudKanban itemU17 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U17.Add(itemU17);
                                break;
                            case 18:
                                solicitudKanban itemU18 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U18.Add(itemU18);
                                break;
                            case 19:
                                solicitudKanban itemU19 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U19.Add(itemU19);
                                break;
                            case 20:
                                solicitudKanban itemU20 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U20.Add(itemU20);
                                break;
                            case 21:
                                solicitudKanban itemU21 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U21.Add(itemU21);
                                break;
                            case 22:
                                solicitudKanban itemU22 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U22.Add(itemU22);
                                break;
                            case 23:
                                solicitudKanban itemU23 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U23.Add(itemU23);
                                break;
                            case 24:
                                solicitudKanban itemU24 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U24.Add(itemU24);
                                break;
                            case 25:
                                solicitudKanban itemU25 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U25.Add(itemU25);
                                break;
                            case 26:
                                solicitudKanban itemU26 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U26.Add(itemU26);
                                break;
                            case 27:
                                solicitudKanban itemU27 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U27.Add(itemU27);
                                break;
                            case 28:
                                solicitudKanban itemU28 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U28.Add(itemU28);
                                break;
                            case 29:
                                solicitudKanban itemU29 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U29.Add(itemU29);
                                break;
                            case 30:
                                solicitudKanban itemU30 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U30.Add(itemU30);
                                break;
                            case 31:
                                solicitudKanban itemU31 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U31.Add(itemU31);
                                break;
                            case 32:
                                solicitudKanban itemU32 = new solicitudKanban
                                {
                                    solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                    tipo = dr["tipo"].ToString(),
                                    modulo = dr["modulo"].ToString(),
                                    ubicacion = Convert.ToInt32(dr["ubicacion"]),
                                    fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaInicio = Convert.ToDateTime(dr["fechaInicio"] is DBNull ? "1900-01-01" : dr["fechaInicio"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    fechaEntrega = Convert.ToDateTime(dr["fechaEntrega"] is DBNull ? "1900-01-01" : dr["fechaEntrega"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                    color = color_,
                                    atiendeSolicitud = Convert.ToInt32(dr["atiendeSolicitud"] is DBNull ? 0 : dr["atiendeSolicitud"]),
                                    validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                                };
                                this.U32.Add(itemU32);
                                break;
                        }

                        if(string.IsNullOrEmpty(dr["fechaInicio"].ToString()) && Convert.ToBoolean(dr["validadoSmed"])==true)
                        {
                            TimeSpan diferencia = DateTime.Now - Convert.ToDateTime(dr["fechaSolicitud"]);
                            double diferenciaenminutos = diferencia.TotalMinutes;

                            string color = "Green";
                            if (diferenciaenminutos >= 120)
                            {
                                color = "Red";
                            }
                            else if(diferenciaenminutos>=80)
                            {
                                color = "Yellow";
                            }

                            solicitudKanban itemP = new solicitudKanban
                            {
                                solicitudKanbanId = Convert.ToInt32(dr["solicitudKanbanId"]),
                                tipo = dr["tipo"].ToString(),
                                modulo = dr["modulo"].ToString(),
                                fechaSolicitud = Convert.ToDateTime(dr["fechaSolicitud"]).ToString("yyyy-MM-dd hh:mm:ss"),
                                color = color,
                                validadoSmed = Convert.ToBoolean(dr["validadoSmed"]),
                            };
                            this.P.Add(itemP);
                        }

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


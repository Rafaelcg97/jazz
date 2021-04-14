using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Data;
using System.Data.SqlClient;
using Production_control_1._0.NotificacionesDeTablaSQL;
using Production_control_1._0.clases;

namespace Production_control_1._0.pantallasMantenimiento.NotificacionesDeTablaSQL
{
    class MessageModelPlanta 
    {
        #region coleccionObservablesPriv
        private ObservableCollection<solicitudMaquina> u1 = null;
        private ObservableCollection<solicitudMaquina> u2 = null;
        private ObservableCollection<solicitudMaquina> u3 = null;
        private ObservableCollection<solicitudMaquina> u4 = null;
        private ObservableCollection<solicitudMaquina> u5 = null;
        private ObservableCollection<solicitudMaquina> u6 = null;
        private ObservableCollection<solicitudMaquina> u7 = null;
        private ObservableCollection<solicitudMaquina> u8 = null;
        private ObservableCollection<solicitudMaquina> u9 = null;
        private ObservableCollection<solicitudMaquina> u10 = null;
        private ObservableCollection<solicitudMaquina> u11 = null;
        private ObservableCollection<solicitudMaquina> u12 = null;
        private ObservableCollection<solicitudMaquina> u13 = null;
        private ObservableCollection<solicitudMaquina> u14 = null;
        private ObservableCollection<solicitudMaquina> u15 = null;
        private ObservableCollection<solicitudMaquina> u16 = null;
        private ObservableCollection<solicitudMaquina> u17 = null;
        private ObservableCollection<solicitudMaquina> u18 = null;
        private ObservableCollection<solicitudMaquina> u19 = null;
        private ObservableCollection<solicitudMaquina> u20 = null;
        private ObservableCollection<solicitudMaquina> u21 = null;
        private ObservableCollection<solicitudMaquina> u22 = null;
        private ObservableCollection<solicitudMaquina> u23 = null;
        private ObservableCollection<solicitudMaquina> u24 = null;
        private ObservableCollection<solicitudMaquina> u25 = null;
        private ObservableCollection<solicitudMaquina> u26 = null;
        private ObservableCollection<solicitudMaquina> u27 = null;
        private ObservableCollection<solicitudMaquina> u28 = null;
        private ObservableCollection<solicitudMaquina> u29 = null;
        private ObservableCollection<solicitudMaquina> u30 = null;
        private ObservableCollection<solicitudMaquina> u31 = null;
        private ObservableCollection<solicitudMaquina> u32 = null;
        private ObservableCollection<solicitudMaquina> u33 = null;
        private ObservableCollection<solicitudMaquina> u34 = null;
        private ObservableCollection<solicitudMaquina> p = null;
        #endregion
        #region coleccionesObservablesPub
        public ObservableCollection<solicitudMaquina> U1
        {
            get
            {
                u1 = u1 ?? new ObservableCollection<solicitudMaquina>();
                return u1;
            }
        }
        public ObservableCollection<solicitudMaquina> U2
        {
            get
            {
                u2 = u2 ?? new ObservableCollection<solicitudMaquina>();
                return u2;
            }
        }
        public ObservableCollection<solicitudMaquina> U3
        {
            get
            {
                u3 = u3 ?? new ObservableCollection<solicitudMaquina>();
                return u3;
            }
        }
        public ObservableCollection<solicitudMaquina> U4
        {
            get
            {
                u4 = u4 ?? new ObservableCollection<solicitudMaquina>();
                return u4;
            }
        }
        public ObservableCollection<solicitudMaquina> U5
        {
            get
            {
                u5 = u5 ?? new ObservableCollection<solicitudMaquina>();
                return u5;
            }
        }
        public ObservableCollection<solicitudMaquina> U6
        {
            get
            {
                u6 = u6 ?? new ObservableCollection<solicitudMaquina>();
                return u6;
            }
        }
        public ObservableCollection<solicitudMaquina> U7
        {
            get
            {
                u7 = u7 ?? new ObservableCollection<solicitudMaquina>();
                return u7;
            }
        }
        public ObservableCollection<solicitudMaquina> U8
        {
            get
            {
                u8 = u8 ?? new ObservableCollection<solicitudMaquina>();
                return u8;
            }
        }
        public ObservableCollection<solicitudMaquina> U9
        {
            get
            {
                u9 = u9 ?? new ObservableCollection<solicitudMaquina>();
                return u9;
            }
        }
        public ObservableCollection<solicitudMaquina> U10
        {
            get
            {
                u10 = u10 ?? new ObservableCollection<solicitudMaquina>();
                return u10;
            }
        }
        public ObservableCollection<solicitudMaquina> U11
        {
            get
            {
                u11 = u11 ?? new ObservableCollection<solicitudMaquina>();
                return u11;
            }
        }
        public ObservableCollection<solicitudMaquina> U12
        {
            get
            {
                u12 = u12 ?? new ObservableCollection<solicitudMaquina>();
                return u12;
            }
        }
        public ObservableCollection<solicitudMaquina> U13
        {
            get
            {
                u13 = u13 ?? new ObservableCollection<solicitudMaquina>();
                return u13;
            }
        }
        public ObservableCollection<solicitudMaquina> U14
        {
            get
            {
                u14 = u14 ?? new ObservableCollection<solicitudMaquina>();
                return u14;
            }
        }
        public ObservableCollection<solicitudMaquina> U15
        {
            get
            {
                u15 = u15 ?? new ObservableCollection<solicitudMaquina>();
                return u15;
            }
        }
        public ObservableCollection<solicitudMaquina> U16
        {
            get
            {
                u16 = u16 ?? new ObservableCollection<solicitudMaquina>();
                return u16;
            }
        }
        public ObservableCollection<solicitudMaquina> U17
        {
            get
            {
                u17 = u17 ?? new ObservableCollection<solicitudMaquina>();
                return u17;
            }
        }
        public ObservableCollection<solicitudMaquina> U18
        {
            get
            {
                u18 = u18 ?? new ObservableCollection<solicitudMaquina>();
                return u18;
            }
        }
        public ObservableCollection<solicitudMaquina> U19
        {
            get
            {
                u19 = u19 ?? new ObservableCollection<solicitudMaquina>();
                return u19;
            }
        }
        public ObservableCollection<solicitudMaquina> U20
        {
            get
            {
                u20 = u20 ?? new ObservableCollection<solicitudMaquina>();
                return u20;
            }
        }
        public ObservableCollection<solicitudMaquina> U21
        {
            get
            {
                u21 = u21 ?? new ObservableCollection<solicitudMaquina>();
                return u21;
            }
        }
        public ObservableCollection<solicitudMaquina> U22
        {
            get
            {
                u22 = u22 ?? new ObservableCollection<solicitudMaquina>();
                return u22;
            }
        }
        public ObservableCollection<solicitudMaquina> U23
        {
            get
            {
                u23 = u23 ?? new ObservableCollection<solicitudMaquina>();
                return u23;
            }
        }
        public ObservableCollection<solicitudMaquina> U24
        {
            get
            {
                u24 = u24 ?? new ObservableCollection<solicitudMaquina>();
                return u24;
            }
        }
        public ObservableCollection<solicitudMaquina> U25
        {
            get
            {
                u25 = u25 ?? new ObservableCollection<solicitudMaquina>();
                return u25;
            }
        }
        public ObservableCollection<solicitudMaquina> U26
        {
            get
            {
                u26 = u26 ?? new ObservableCollection<solicitudMaquina>();
                return u26;
            }
        }
        public ObservableCollection<solicitudMaquina> U27
        {
            get
            {
                u27 = u27 ?? new ObservableCollection<solicitudMaquina>();
                return u27;
            }
        }
        public ObservableCollection<solicitudMaquina> U28
        {
            get
            {
                u28 = u28 ?? new ObservableCollection<solicitudMaquina>();
                return u28;
            }
        }
        public ObservableCollection<solicitudMaquina> U29
        {
            get
            {
                u29 = u29 ?? new ObservableCollection<solicitudMaquina>();
                return u29;
            }
        }
        public ObservableCollection<solicitudMaquina> U30
        {
            get
            {
                u30 = u30 ?? new ObservableCollection<solicitudMaquina>();
                return u30;
            }
        }
        public ObservableCollection<solicitudMaquina> U31
        {
            get
            {
                u31 = u31 ?? new ObservableCollection<solicitudMaquina>();
                return u31;
            }
        }
        public ObservableCollection<solicitudMaquina> U32
        {
            get
            {
                u32 = u32 ?? new ObservableCollection<solicitudMaquina>();
                return u32;
            }
        }
        public ObservableCollection<solicitudMaquina> U33
        {
            get
            {
                u33 = u33 ?? new ObservableCollection<solicitudMaquina>();
                return u33;
            }
        }
        public ObservableCollection<solicitudMaquina> U34
        {
            get
            {
                u34 = u34 ?? new ObservableCollection<solicitudMaquina>();
                return u34;
            }
        }
        public ObservableCollection<solicitudMaquina> P
        {
            get
            {
                p = p ?? new ObservableCollection<solicitudMaquina>();
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
                    this.U33.Clear();
                    this.U34.Clear();
                    this.P.Clear();

                    int conteoPrio = 0;
                    foreach (DataRow dr in consultado.Rows)
                    {
                        switch (Convert.ToInt32(dr["ubicacion"]))
                        {
                            case 1:
                                solicitudMaquina itemU1 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U1.Add(itemU1);
                                break;
                            case 2:
                                solicitudMaquina itemU2 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U2.Add(itemU2);
                                break;
                            case 3:
                                solicitudMaquina itemU3 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U3.Add(itemU3);
                                break;
                            case 4:
                                solicitudMaquina itemU4 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U4.Add(itemU4);
                                break;
                            case 5:
                                solicitudMaquina itemU5 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U5.Add(itemU5);
                                break;
                            case 6:
                                solicitudMaquina itemU6 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U6.Add(itemU6);
                                break;
                            case 7:
                                solicitudMaquina itemU7 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U7.Add(itemU7);
                                break;
                            case 8:
                                solicitudMaquina itemU8 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U8.Add(itemU8);
                                break;
                            case 9:
                                solicitudMaquina itemU9 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U9.Add(itemU9);
                                break;
                            case 10:
                                solicitudMaquina itemU10 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U10.Add(itemU10);
                                break;
                            case 11:
                                solicitudMaquina itemU11 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U11.Add(itemU11);
                                break;
                            case 12:
                                solicitudMaquina itemU12 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U12.Add(itemU12);
                                break;
                            case 13:
                                solicitudMaquina itemU13 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U13.Add(itemU13);
                                break;
                            case 14:
                                solicitudMaquina itemU14 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U14.Add(itemU14);
                                break;
                            case 15:
                                solicitudMaquina itemU15 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U15.Add(itemU15);
                                break;
                            case 16:
                                solicitudMaquina itemU16 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U16.Add(itemU16);
                                break;
                            case 17:
                                solicitudMaquina itemU17 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U17.Add(itemU17);
                                break;
                            case 18:
                                solicitudMaquina itemU18 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U18.Add(itemU18);
                                break;
                            case 19:
                                solicitudMaquina itemU19 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U19.Add(itemU19);
                                break;
                            case 20:
                                solicitudMaquina itemU20 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U20.Add(itemU20);
                                break;
                            case 21:
                                solicitudMaquina itemU21 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U21.Add(itemU21);
                                break;
                            case 22:
                                solicitudMaquina itemU22 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U22.Add(itemU22);
                                break;
                            case 23:
                                solicitudMaquina itemU23 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U23.Add(itemU23);
                                break;
                            case 24:
                                solicitudMaquina itemU24 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U24.Add(itemU24);
                                break;
                            case 25:
                                solicitudMaquina itemU25 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U25.Add(itemU25);
                                break;
                            case 26:
                                solicitudMaquina itemU26 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U26.Add(itemU26);
                                break;
                            case 27:
                                solicitudMaquina itemU27 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U27.Add(itemU27);
                                break;
                            case 28:
                                solicitudMaquina itemU28 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U28.Add(itemU28);
                                break;
                            case 29:
                                solicitudMaquina itemU29 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U29.Add(itemU29);
                                break;
                            case 30:
                                solicitudMaquina itemU30 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U30.Add(itemU30);
                                break;
                            case 31:
                                solicitudMaquina itemU31 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U31.Add(itemU31);
                                break;
                            case 32:
                                solicitudMaquina itemU32 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U32.Add(itemU32);
                                break;
                            case 33:
                                solicitudMaquina itemU33 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U33.Add(itemU33);

                                break;
                            case 34:
                                solicitudMaquina itemU34 = new solicitudMaquina
                                {
                                    id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                    problema_reportado = dr["problema_reportado"].ToString()
                                };
                                this.U34.Add(itemU34);
                                break;
                        }
                    if (dr["corresponde"].ToString()=="MANTENIMIENTO" && String.IsNullOrEmpty(dr["hora_cierre"].ToString()))
                        {
                            conteoPrio = conteoPrio + 1;
                            solicitudMaquina prioridad = new solicitudMaquina
                            {
                                id_solicitud = Convert.ToInt32(dr["id_solicitud"]),
                                modulo = dr["modulo"].ToString(),
                                maquina = dr["maquina"].ToString(),
                                problema_reportado = dr["problema_reportado"].ToString(),
                                prioridad = conteoPrio
                            };
                            this.P.Add(prioridad);

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


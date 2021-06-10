using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Production_control_1._0.clases;
using static Production_control_1._0.estadoPlantaProduccion;
using Brush = System.Windows.Media.Brush;
using Production_control_1._0.pantallasMantenimiento.NotificacionesDeTablaSQL;
using System.Collections.ObjectModel;

namespace Production_control_1._0.pantallasMantenimiento.NotificacionesDeTablaSQL
{
    public class coloresModulo
    {
        public static void VerificarColor(ObservableCollection<solicitudMaquina> U1, ObservableCollection<solicitudMaquina> U2, ObservableCollection<solicitudMaquina> U3, ObservableCollection<solicitudMaquina> U4, ObservableCollection<solicitudMaquina> U5, ObservableCollection<solicitudMaquina> U6, ObservableCollection<solicitudMaquina> U7, ObservableCollection<solicitudMaquina> U8, ObservableCollection<solicitudMaquina> U9, ObservableCollection<solicitudMaquina> U10, ObservableCollection<solicitudMaquina> U11, ObservableCollection<solicitudMaquina> U12, ObservableCollection<solicitudMaquina> U13, ObservableCollection<solicitudMaquina> U14, ObservableCollection<solicitudMaquina> U15, ObservableCollection<solicitudMaquina> U16, ObservableCollection<solicitudMaquina> U17, ObservableCollection<solicitudMaquina> U18, ObservableCollection<solicitudMaquina> U19, ObservableCollection<solicitudMaquina> U20, ObservableCollection<solicitudMaquina> U21, ObservableCollection<solicitudMaquina> U22, ObservableCollection<solicitudMaquina> U23, ObservableCollection<solicitudMaquina> U24, ObservableCollection<solicitudMaquina> U25, ObservableCollection<solicitudMaquina> U26, ObservableCollection<solicitudMaquina> U27, ObservableCollection<solicitudMaquina> U28, ObservableCollection<solicitudMaquina> U29, ObservableCollection<solicitudMaquina> U30, ObservableCollection<solicitudMaquina> U31, ObservableCollection<solicitudMaquina> U32, ObservableCollection<solicitudMaquina> U33, ObservableCollection<solicitudMaquina> U34, ObservableCollection<solicitudMaquina> U35)
        {
            #region variablesConteo
            //am (abierto Mantenimiento), as(abierto SMED), pm(pendiente Mantenimiento), ps(pendiente SMED), c(cambio)
            int am1 = 0;
            int am2 = 0;
            int am3 = 0;
            int am4 = 0;
            int am5 = 0;
            int am6 = 0;
            int am7 = 0;
            int am8 = 0;
            int am9 = 0;
            int am10 = 0;
            int am11 = 0;
            int am12 = 0;
            int am13 = 0;
            int am14 = 0;
            int am15 = 0;
            int am16 = 0;
            int am17 = 0;
            int am18 = 0;
            int am19 = 0;
            int am20 = 0;
            int am21 = 0;
            int am22 = 0;
            int am23 = 0;
            int am24 = 0;
            int am25 = 0;
            int am26 = 0;
            int am27 = 0;
            int am28 = 0;
            int am29 = 0;
            int am30 = 0;
            int am31 = 0;
            int am32 = 0;
            int am33 = 0;
            int am34 = 0;
            int am35 = 0;
            int as1 = 0;
            int as2 = 0;
            int as3 = 0;
            int as4 = 0;
            int as5 = 0;
            int as6 = 0;
            int as7 = 0;
            int as8 = 0;
            int as9 = 0;
            int as10 = 0;
            int as11 = 0;
            int as12 = 0;
            int as13 = 0;
            int as14 = 0;
            int as15 = 0;
            int as16 = 0;
            int as17 = 0;
            int as18 = 0;
            int as19 = 0;
            int as20 = 0;
            int as21 = 0;
            int as22 = 0;
            int as23 = 0;
            int as24 = 0;
            int as25 = 0;
            int as26 = 0;
            int as27 = 0;
            int as28 = 0;
            int as29 = 0;
            int as30 = 0;
            int as31 = 0;
            int as32 = 0;
            int as33 = 0;
            int as34 = 0;
            int as35 = 0;
            int pm1 = 0;
            int pm2 = 0;
            int pm3 = 0;
            int pm4 = 0;
            int pm5 = 0;
            int pm6 = 0;
            int pm7 = 0;
            int pm8 = 0;
            int pm9 = 0;
            int pm10 = 0;
            int pm11 = 0;
            int pm12 = 0;
            int pm13 = 0;
            int pm14 = 0;
            int pm15 = 0;
            int pm16 = 0;
            int pm17 = 0;
            int pm18 = 0;
            int pm19 = 0;
            int pm20 = 0;
            int pm21 = 0;
            int pm22 = 0;
            int pm23 = 0;
            int pm24 = 0;
            int pm25 = 0;
            int pm26 = 0;
            int pm27 = 0;
            int pm28 = 0;
            int pm29 = 0;
            int pm30 = 0;
            int pm31 = 0;
            int pm32 = 0;
            int pm33 = 0;
            int pm34 = 0;
            int pm35 = 0;
            int ps1 = 0;
            int ps2 = 0;
            int ps3 = 0;
            int ps4 = 0;
            int ps5 = 0;
            int ps6 = 0;
            int ps7 = 0;
            int ps8 = 0;
            int ps9 = 0;
            int ps10 = 0;
            int ps11 = 0;
            int ps12 = 0;
            int ps13 = 0;
            int ps14 = 0;
            int ps15 = 0;
            int ps16 = 0;
            int ps17 = 0;
            int ps18 = 0;
            int ps19 = 0;
            int ps20 = 0;
            int ps21 = 0;
            int ps22 = 0;
            int ps23 = 0;
            int ps24 = 0;
            int ps25 = 0;
            int ps26 = 0;
            int ps27 = 0;
            int ps28 = 0;
            int ps29 = 0;
            int ps30 = 0;
            int ps31 = 0;
            int ps32 = 0;
            int ps33 = 0;
            int ps34 = 0;
            int ps35 = 0;
            int c1 = 0;
            int c2 = 0;
            int c3 = 0;
            int c4 = 0;
            int c5 = 0;
            int c6 = 0;
            int c7 = 0;
            int c8 = 0;
            int c9 = 0;
            int c10 = 0;
            int c11 = 0;
            int c12 = 0;
            int c13 = 0;
            int c14 = 0;
            int c15 = 0;
            int c16 = 0;
            int c17 = 0;
            int c18 = 0;
            int c19 = 0;
            int c20 = 0;
            int c21 = 0;
            int c22 = 0;
            int c23 = 0;
            int c24 = 0;
            int c25 = 0;
            int c26 = 0;
            int c27 = 0;
            int c28 = 0;
            int c29 = 0;
            int c30 = 0;
            int c31 = 0;
            int c32 = 0;
            int c33 = 0;
            int c34 = 0;
            int c35 = 0;
            #endregion
            #region contarListas
            foreach (solicitudMaquina item in U1)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm1 = pm1 + 1;
                        }
                        else
                        {
                            am1 = am1 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c1 = c1 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps1 = ps1 + 1;
                            }
                            else
                            {
                                as1 = as1 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U2)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm2 = pm2 + 1;
                        }
                        else
                        {
                            am2 = am2 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c2 = c2 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps2 = ps2 + 1;
                            }
                            else
                            {
                                as2 = as2 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U3)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm3 = pm3 + 1;
                        }
                        else
                        {
                            am3 = am3 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c3 = c3 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps3 = ps3 + 1;
                            }
                            else
                            {
                                as3 = as3 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U4)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm4 = pm4 + 1;
                        }
                        else
                        {
                            am4 = am4 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c4 = c4 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps4 = ps4 + 1;
                            }
                            else
                            {
                                as4 = as4 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U5)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm5 = pm5 + 1;
                        }
                        else
                        {
                            am5 = am5 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c5 = c5 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps5 = ps5 + 1;
                            }
                            else
                            {
                                as5 = as5 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U6)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm6 = pm6 + 1;
                        }
                        else
                        {
                            am6 = am6 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c6 = c6 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps6 = ps6 + 1;
                            }
                            else
                            {
                                as6 = as6 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U7)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm7 = pm7 + 1;
                        }
                        else
                        {
                            am7 = am7 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c7 = c7 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps7 = ps7 + 1;
                            }
                            else
                            {
                                as7 = as7 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U8)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm8 = pm8 + 1;
                        }
                        else
                        {
                            am8 = am8 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c8 = c8 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps8 = ps8 + 1;
                            }
                            else
                            {
                                as8 = as8 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U9)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm9 = pm9 + 1;
                        }
                        else
                        {
                            am9 = am9 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c9 = c9 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps9 = ps9 + 1;
                            }
                            else
                            {
                                as9 = as9 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U10)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm10 = pm10 + 1;
                        }
                        else
                        {
                            am10 = am10 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c10 = c10 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps10 = ps10 + 1;
                            }
                            else
                            {
                                as10 = as10 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U11)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm11 = pm11 + 1;
                        }
                        else
                        {
                            am11 = am11 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c11 = c11 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps11 = ps11 + 1;
                            }
                            else
                            {
                                as11 = as11 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U12)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm12 = pm12 + 1;
                        }
                        else
                        {
                            am12 = am12 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c12 = c12 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps12 = ps12 + 1;
                            }
                            else
                            {
                                as12 = as12 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U13)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm13 = pm13 + 1;
                        }
                        else
                        {
                            am13 = am13 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c13 = c13 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps13 = ps13 + 1;
                            }
                            else
                            {
                                as13 = as13 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U14)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm14 = pm14 + 1;
                        }
                        else
                        {
                            am14 = am14 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c14 = c14 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps14 = ps14 + 1;
                            }
                            else
                            {
                                as14 = as14 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U15)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm15 = pm15 + 1;
                        }
                        else
                        {
                            am15 = am15 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c15 = c15 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps15 = ps15 + 1;
                            }
                            else
                            {
                                as15 = as15 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U16)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm16 = pm16 + 1;
                        }
                        else
                        {
                            am16 = am16 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c16 = c16 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps16 = ps16 + 1;
                            }
                            else
                            {
                                as16 = as16 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U17)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm17 = pm17 + 1;
                        }
                        else
                        {
                            am17 = am17 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c17 = c17 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps17 = ps17 + 1;
                            }
                            else
                            {
                                as17 = as17 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U18)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm18 = pm18 + 1;
                        }
                        else
                        {
                            am18 = am18 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c18 = c18 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps18 = ps18 + 1;
                            }
                            else
                            {
                                as18 = as18 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U19)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm19 = pm19 + 1;
                        }
                        else
                        {
                            am19 = am19 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c19 = c19 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps19 = ps19 + 1;
                            }
                            else
                            {
                                as19 = as19 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U20)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm20 = pm20 + 1;
                        }
                        else
                        {
                            am20 = am20 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c20 = c20 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps20 = ps20 + 1;
                            }
                            else
                            {
                                as20 = as20 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U21)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm21 = pm21 + 1;
                        }
                        else
                        {
                            am21 = am21 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c21 = c21 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps21 = ps21 + 1;
                            }
                            else
                            {
                                as21 = as21 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U22)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm22 = pm22 + 1;
                        }
                        else
                        {
                            am22 = am22 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c22 = c22 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps22 = ps22 + 1;
                            }
                            else
                            {
                                as22 = as22 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U23)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm23 = pm23 + 1;
                        }
                        else
                        {
                            am23 = am23 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c23 = c23 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps23 = ps23 + 1;
                            }
                            else
                            {
                                as23 = as23 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U24)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm24 = pm24 + 1;
                        }
                        else
                        {
                            am24 = am24 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c24 = c24 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps24 = ps24 + 1;
                            }
                            else
                            {
                                as24 = as24 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U25)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm25 = pm25 + 1;
                        }
                        else
                        {
                            am25 = am25 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c25 = c25 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps25 = ps25 + 1;
                            }
                            else
                            {
                                as25 = as25 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U26)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm26 = pm26 + 1;
                        }
                        else
                        {
                            am26 = am26 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c26 = c26 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps26 = ps26 + 1;
                            }
                            else
                            {
                                as26 = as26 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U27)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm27 = pm27 + 1;
                        }
                        else
                        {
                            am27 = am27 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c27 = c27 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps27 = ps27 + 1;
                            }
                            else
                            {
                                as27 = as27 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U28)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm28 = pm28 + 1;
                        }
                        else
                        {
                            am28 = am28 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c28 = c28 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps28 = ps28 + 1;
                            }
                            else
                            {
                                as28 = as28 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U29)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm29 = pm29 + 1;
                        }
                        else
                        {
                            am29 = am29 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c29 = c29 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps29 = ps29 + 1;
                            }
                            else
                            {
                                as29 = as29 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U30)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm30 = pm30 + 1;
                        }
                        else
                        {
                            am30 = am30 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c30 = c30 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps30 = ps30 + 1;
                            }
                            else
                            {
                                as30 = as30 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U31)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm31 = pm31 + 1;
                        }
                        else
                        {
                            am31 = am31 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c31 = c31 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps31 = ps31 + 1;
                            }
                            else
                            {
                                as31 = as31 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U32)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm32 = pm32 + 1;
                        }
                        else
                        {
                            am32 = am32 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c32 = c32 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps32 = ps32 + 1;
                            }
                            else
                            {
                                as32 = as32 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U33)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm33 = pm33 + 1;
                        }
                        else
                        {
                            am33 = am33 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c33 = c33 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps33 = ps33 + 1;
                            }
                            else
                            {
                                as33 = as33 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U34)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm34 = pm34 + 1;
                        }
                        else
                        {
                            am34 = am34 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c34 = c34 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps34 = ps34 + 1;
                            }
                            else
                            {
                                as34 = as34 + 1;
                            }
                        }
                        break;
                }
            }
            foreach (solicitudMaquina item in U35)
            {
                switch (item.corresponde.ToString())
                {
                    case "MANTENIMIENTO":
                        if (item.hora_asignacion.ToString() == "0")
                        {
                            pm35 = pm35 + 1;
                        }
                        else
                        {
                            am35 = am35 + 1;
                        }
                        break;
                    case "SMED":
                        if (item.problema_reportado.ToString() == "CAMBIO")
                        {
                            c35 = c35 + 1;
                        }
                        else
                        {
                            if (item.hora_asignacion.ToString() == "0")
                            {
                                ps35 = ps35 + 1;
                            }
                            else
                            {
                                as35 = as35 + 1;
                            }
                        }
                        break;
                }
            }
            #endregion
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                    Grid areaTrabajo = (Grid)UIGlobal.MainPage.Content;
                    foreach (object elemento in areaTrabajo.Children)
                    {
                        if (elemento.GetType() == typeof(Grid))
                        {
                            Grid areaModulos = (Grid)elemento;
                            foreach (object modulo in areaModulos.Children)
                            {
                                if (modulo.GetType() == typeof(ListBox))
                                {
                                    ListBox moduloLista = (ListBox)modulo;
                                    if (moduloLista.Name == "modulo_1")
                                    {
                                        #region fondo
                                        if (c1 == 0)
                                        {
                                            if (pm1 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm1 == 0 && ps1 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am1 > 0 || as1 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm1 == 0 && ps1 == 0 && as1 == 0 && am1 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm1 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm1 == 0 && ps1 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am1 > 0 || as1 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm1 == 0 && ps1 == 0 && as1 == 0 && am1 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c1 == 0)
                                        {
                                            if (ps1 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm1 > 0 && ps1 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am1 > 0 || as1 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm1 == 0 && ps1 == 0 && as1 == 0 && am1 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_2")
                                    {
                                        #region fondo
                                        if (c2 == 0)
                                        {
                                            if (pm2 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm2 == 0 && ps2 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am2 > 0 || as2 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm2 == 0 && ps2 == 0 && as2 == 0 && am2 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm2 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm2 == 0 && ps2 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am2 > 0 || as2 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm2 == 0 && ps2 == 0 && as2 == 0 && am2 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c2 == 0)
                                        {
                                            if (ps2 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm2 > 0 && ps2 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am2 > 0 || as2 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm2 == 0 && ps2 == 0 && as2 == 0 && am2 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_3")
                                    {
                                        #region fondo
                                        if (c3 == 0)
                                        {
                                            if (pm3 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm3 == 0 && ps3 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am3 > 0 || as3 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm3 == 0 && ps3 == 0 && as3 == 0 && am3 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm3 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm3 == 0 && ps3 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am3 > 0 || as3 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm3 == 0 && ps3 == 0 && as3 == 0 && am3 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c3 == 0)
                                        {
                                            if (ps3 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm3 > 0 && ps3 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am3 > 0 || as3 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm3 == 0 && ps3 == 0 && as3 == 0 && am3 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_4")
                                    {
                                        #region fondo
                                        if (c4 == 0)
                                        {
                                            if (pm4 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm4 == 0 && ps4 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am4 > 0 || as4 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm4 == 0 && ps4 == 0 && as4 == 0 && am4 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm4 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm4 == 0 && ps4 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am4 > 0 || as4 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm4 == 0 && ps4 == 0 && as4 == 0 && am4 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c4 == 0)
                                        {
                                            if (ps4 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm4 > 0 && ps4 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am4 > 0 || as4 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm4 == 0 && ps4 == 0 && as4 == 0 && am4 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_5")
                                    {
                                        #region fondo
                                        if (c5 == 0)
                                        {
                                            if (pm5 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm5 == 0 && ps5 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am5 > 0 || as5 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm5 == 0 && ps5 == 0 && as5 == 0 && am5 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm5 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm5 == 0 && ps5 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am5 > 0 || as5 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm5 == 0 && ps5 == 0 && as5 == 0 && am5 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c5 == 0)
                                        {
                                            if (ps5 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm5 > 0 && ps5 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am5 > 0 || as5 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm5 == 0 && ps5 == 0 && as5 == 0 && am5 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_6")
                                    {
                                        #region fondo
                                        if (c6 == 0)
                                        {
                                            if (pm6 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm6 == 0 && ps6 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am6 > 0 || as6 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm6 == 0 && ps6 == 0 && as6 == 0 && am6 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm6 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm6 == 0 && ps6 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am6 > 0 || as6 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm6 == 0 && ps6 == 0 && as6 == 0 && am6 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c6 == 0)
                                        {
                                            if (ps6 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm6 > 0 && ps6 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am6 > 0 || as6 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm6 == 0 && ps6 == 0 && as6 == 0 && am6 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_7")
                                    {
                                        #region fondo
                                        if (c7 == 0)
                                        {
                                            if (pm7 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm7 == 0 && ps7 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am7 > 0 || as7 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm7 == 0 && ps7 == 0 && as7 == 0 && am7 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm7 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm7 == 0 && ps7 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am7 > 0 || as7 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm7 == 0 && ps7 == 0 && as7 == 0 && am7 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c7 == 0)
                                        {
                                            if (ps7 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm7 > 0 && ps7 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am7 > 0 || as7 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm7 == 0 && ps7 == 0 && as7 == 0 && am7 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_8")
                                    {
                                        #region fondo
                                        if (c8 == 0)
                                        {
                                            if (pm8 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm8 == 0 && ps8 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am8 > 0 || as8 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm8 == 0 && ps8 == 0 && as8 == 0 && am8 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm8 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm8 == 0 && ps8 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am8 > 0 || as8 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm8 == 0 && ps8 == 0 && as8 == 0 && am8 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c8 == 0)
                                        {
                                            if (ps8 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm8 > 0 && ps8 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am8 > 0 || as8 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm8 == 0 && ps8 == 0 && as8 == 0 && am8 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_9")
                                    {
                                        #region fondo
                                        if (c9 == 0)
                                        {
                                            if (pm9 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm9 == 0 && ps9 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am9 > 0 || as9 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm9 == 0 && ps9 == 0 && as9 == 0 && am9 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm9 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm9 == 0 && ps9 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am9 > 0 || as9 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm9 == 0 && ps9 == 0 && as9 == 0 && am9 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c9 == 0)
                                        {
                                            if (ps9 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm9 > 0 && ps9 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am9 > 0 || as9 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm9 == 0 && ps9 == 0 && as9 == 0 && am9 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_10")
                                    {
                                        #region fondo
                                        if (c10 == 0)
                                        {
                                            if (pm10 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm10 == 0 && ps10 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am10 > 0 || as10 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm10 == 0 && ps10 == 0 && as10 == 0 && am10 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm10 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm10 == 0 && ps10 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am10 > 0 || as10 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm10 == 0 && ps10 == 0 && as10 == 0 && am10 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c10 == 0)
                                        {
                                            if (ps10 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm10 > 0 && ps10 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am10 > 0 || as10 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm10 == 0 && ps10 == 0 && as10 == 0 && am10 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_11")
                                    {
                                        #region fondo
                                        if (c11 == 0)
                                        {
                                            if (pm11 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm11 == 0 && ps11 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am11 > 0 || as11 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm11 == 0 && ps11 == 0 && as11 == 0 && am11 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm11 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm11 == 0 && ps11 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am11 > 0 || as11 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm11 == 0 && ps11 == 0 && as11 == 0 && am11 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c11 == 0)
                                        {
                                            if (ps11 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm11 > 0 && ps11 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am11 > 0 || as11 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm11 == 0 && ps11 == 0 && as11 == 0 && am11 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_12")
                                    {
                                        #region fondo
                                        if (c12 == 0)
                                        {
                                            if (pm12 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm12 == 0 && ps12 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am12 > 0 || as12 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm12 == 0 && ps12 == 0 && as12 == 0 && am12 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm12 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm12 == 0 && ps12 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am12 > 0 || as12 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm12 == 0 && ps12 == 0 && as12 == 0 && am12 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c12 == 0)
                                        {
                                            if (ps12 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm12 > 0 && ps12 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am12 > 0 || as12 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm12 == 0 && ps12 == 0 && as12 == 0 && am12 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_13")
                                    {
                                        #region fondo
                                        if (c13 == 0)
                                        {
                                            if (pm13 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm13 == 0 && ps13 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am13 > 0 || as13 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm13 == 0 && ps13 == 0 && as13 == 0 && am13 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm13 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm13 == 0 && ps13 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am13 > 0 || as13 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm13 == 0 && ps13 == 0 && as13 == 0 && am13 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c13 == 0)
                                        {
                                            if (ps13 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm13 > 0 && ps13 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am13 > 0 || as13 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm13 == 0 && ps13 == 0 && as13 == 0 && am13 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_14")
                                    {
                                        #region fondo
                                        if (c14 == 0)
                                        {
                                            if (pm14 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm14 == 0 && ps14 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am14 > 0 || as14 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm14 == 0 && ps14 == 0 && as14 == 0 && am14 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm14 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm14 == 0 && ps14 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am14 > 0 || as14 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm14 == 0 && ps14 == 0 && as14 == 0 && am14 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c14 == 0)
                                        {
                                            if (ps14 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm14 > 0 && ps14 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am14 > 0 || as14 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm14 == 0 && ps14 == 0 && as14 == 0 && am14 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_15")
                                    {
                                        #region fondo
                                        if (c15 == 0)
                                        {
                                            if (pm15 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm15 == 0 && ps15 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am15 > 0 || as15 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm15 == 0 && ps15 == 0 && as15 == 0 && am15 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm15 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm15 == 0 && ps15 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am15 > 0 || as15 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm15 == 0 && ps15 == 0 && as15 == 0 && am15 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c15 == 0)
                                        {
                                            if (ps15 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm15 > 0 && ps15 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am15 > 0 || as15 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm15 == 0 && ps15 == 0 && as15 == 0 && am15 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_16")
                                    {
                                        #region fondo
                                        if (c16 == 0)
                                        {
                                            if (pm16 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm16 == 0 && ps16 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am16 > 0 || as16 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm16 == 0 && ps16 == 0 && as16 == 0 && am16 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm16 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm16 == 0 && ps16 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am16 > 0 || as16 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm16 == 0 && ps16 == 0 && as16 == 0 && am16 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c16 == 0)
                                        {
                                            if (ps16 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm16 > 0 && ps16 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am16 > 0 || as16 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm16 == 0 && ps16 == 0 && as16 == 0 && am16 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_17")
                                    {
                                        #region fondo
                                        if (c17 == 0)
                                        {
                                            if (pm17 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm17 == 0 && ps17 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am17 > 0 || as17 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm17 == 0 && ps17 == 0 && as17 == 0 && am17 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm17 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm17 == 0 && ps17 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am17 > 0 || as17 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm17 == 0 && ps17 == 0 && as17 == 0 && am17 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c17 == 0)
                                        {
                                            if (ps17 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm17 > 0 && ps17 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am17 > 0 || as17 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm17 == 0 && ps17 == 0 && as17 == 0 && am17 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_18")
                                    {
                                        #region fondo
                                        if (c18 == 0)
                                        {
                                            if (pm18 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm18 == 0 && ps18 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am18 > 0 || as18 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm18 == 0 && ps18 == 0 && as18 == 0 && am18 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm18 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm18 == 0 && ps18 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am18 > 0 || as18 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm18 == 0 && ps18 == 0 && as18 == 0 && am18 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c18 == 0)
                                        {
                                            if (ps18 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm18 > 0 && ps18 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am18 > 0 || as18 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm18 == 0 && ps18 == 0 && as18 == 0 && am18 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_19")
                                    {
                                        #region fondo
                                        if (c19 == 0)
                                        {
                                            if (pm19 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm19 == 0 && ps19 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am19 > 0 || as19 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm19 == 0 && ps19 == 0 && as19 == 0 && am19 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm19 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm19 == 0 && ps19 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am19 > 0 || as19 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm19 == 0 && ps19 == 0 && as19 == 0 && am19 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c19 == 0)
                                        {
                                            if (ps19 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm19 > 0 && ps19 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am19 > 0 || as19 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm19 == 0 && ps19 == 0 && as19 == 0 && am19 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_20")
                                    {
                                        #region fondo
                                        if (c20 == 0)
                                        {
                                            if (pm20 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm20 == 0 && ps20 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am20 > 0 || as20 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm20 == 0 && ps20 == 0 && as20 == 0 && am20 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm20 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm20 == 0 && ps20 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am20 > 0 || as20 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm20 == 0 && ps20 == 0 && as20 == 0 && am20 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c20 == 0)
                                        {
                                            if (ps20 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm20 > 0 && ps20 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am20 > 0 || as20 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm20 == 0 && ps20 == 0 && as20 == 0 && am20 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_21")
                                    {
                                        #region fondo
                                        if (c21 == 0)
                                        {
                                            if (pm21 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm21 == 0 && ps21 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am21 > 0 || as21 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm21 == 0 && ps21 == 0 && as21 == 0 && am21 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm21 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm21 == 0 && ps21 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am21 > 0 || as21 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm21 == 0 && ps21 == 0 && as21 == 0 && am21 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c21 == 0)
                                        {
                                            if (ps21 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm21 > 0 && ps21 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am21 > 0 || as21 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm21 == 0 && ps21 == 0 && as21 == 0 && am21 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_22")
                                    {
                                        #region fondo
                                        if (c22 == 0)
                                        {
                                            if (pm22 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm22 == 0 && ps22 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am22 > 0 || as22 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm22 == 0 && ps22 == 0 && as22 == 0 && am22 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm22 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm22 == 0 && ps22 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am22 > 0 || as22 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm22 == 0 && ps22 == 0 && as22 == 0 && am22 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c22 == 0)
                                        {
                                            if (ps22 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm22 > 0 && ps22 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am22 > 0 || as22 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm22 == 0 && ps22 == 0 && as22 == 0 && am22 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_23")
                                    {
                                        #region fondo
                                        if (c23 == 0)
                                        {
                                            if (pm23 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm23 == 0 && ps23 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am23 > 0 || as23 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm23 == 0 && ps23 == 0 && as23 == 0 && am23 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm23 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm23 == 0 && ps23 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am23 > 0 || as23 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm23 == 0 && ps23 == 0 && as23 == 0 && am23 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c23 == 0)
                                        {
                                            if (ps23 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm23 > 0 && ps23 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am23 > 0 || as23 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm23 == 0 && ps23 == 0 && as23 == 0 && am23 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_24")
                                    {
                                        #region fondo
                                        if (c24 == 0)
                                        {
                                            if (pm24 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm24 == 0 && ps24 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am24 > 0 || as24 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm24 == 0 && ps24 == 0 && as24 == 0 && am24 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm24 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm24 == 0 && ps24 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am24 > 0 || as24 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm24 == 0 && ps24 == 0 && as24 == 0 && am24 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c24 == 0)
                                        {
                                            if (ps24 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm24 > 0 && ps24 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am24 > 0 || as24 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm24 == 0 && ps24 == 0 && as24 == 0 && am24 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_25")
                                    {
                                        #region fondo
                                        if (c25 == 0)
                                        {
                                            if (pm25 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm25 == 0 && ps25 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am25 > 0 || as25 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm25 == 0 && ps25 == 0 && as25 == 0 && am25 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm25 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm25 == 0 && ps25 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am25 > 0 || as25 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm25 == 0 && ps25 == 0 && as25 == 0 && am25 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c25 == 0)
                                        {
                                            if (ps25 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm25 > 0 && ps25 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am25 > 0 || as25 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm25 == 0 && ps25 == 0 && as25 == 0 && am25 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_26")
                                    {
                                        #region fondo
                                        if (c26 == 0)
                                        {
                                            if (pm26 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm26 == 0 && ps26 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am26 > 0 || as26 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm26 == 0 && ps26 == 0 && as26 == 0 && am26 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm26 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm26 == 0 && ps26 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am26 > 0 || as26 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm26 == 0 && ps26 == 0 && as26 == 0 && am26 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c26 == 0)
                                        {
                                            if (ps26 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm26 > 0 && ps26 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am26 > 0 || as26 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm26 == 0 && ps26 == 0 && as26 == 0 && am26 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_27")
                                    {
                                        #region fondo
                                        if (c27 == 0)
                                        {
                                            if (pm27 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm27 == 0 && ps27 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am27 > 0 || as27 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm27 == 0 && ps27 == 0 && as27 == 0 && am27 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm27 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm27 == 0 && ps27 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am27 > 0 || as27 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm27 == 0 && ps27 == 0 && as27 == 0 && am27 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c27 == 0)
                                        {
                                            if (ps27 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm27 > 0 && ps27 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am27 > 0 || as27 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm27 == 0 && ps27 == 0 && as27 == 0 && am27 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_28")
                                    {
                                        #region fondo
                                        if (c28 == 0)
                                        {
                                            if (pm28 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm28 == 0 && ps28 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am28 > 0 || as28 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm28 == 0 && ps28 == 0 && as28 == 0 && am28 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm28 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm28 == 0 && ps28 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am28 > 0 || as28 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm28 == 0 && ps28 == 0 && as28 == 0 && am28 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c28 == 0)
                                        {
                                            if (ps28 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm28 > 0 && ps28 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am28 > 0 || as28 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm28 == 0 && ps28 == 0 && as28 == 0 && am28 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_29")
                                    {
                                        #region fondo
                                        if (c29 == 0)
                                        {
                                            if (pm29 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm29 == 0 && ps29 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am29 > 0 || as29 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm29 == 0 && ps29 == 0 && as29 == 0 && am29 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm29 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm29 == 0 && ps29 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am29 > 0 || as29 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm29 == 0 && ps29 == 0 && as29 == 0 && am29 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c29 == 0)
                                        {
                                            if (ps29 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm29 > 0 && ps29 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am29 > 0 || as29 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm29 == 0 && ps29 == 0 && as29 == 0 && am29 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_30")
                                    {
                                        #region fondo
                                        if (c30 == 0)
                                        {
                                            if (pm30 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm30 == 0 && ps30 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am30 > 0 || as30 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm30 == 0 && ps30 == 0 && as30 == 0 && am30 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm30 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm30 == 0 && ps30 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am30 > 0 || as30 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm30 == 0 && ps30 == 0 && as30 == 0 && am30 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c30 == 0)
                                        {
                                            if (ps30 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm30 > 0 && ps30 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am30 > 0 || as30 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm30 == 0 && ps30 == 0 && as30 == 0 && am30 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_31")
                                    {
                                        #region fondo
                                        if (c31 == 0)
                                        {
                                            if (pm31 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm31 == 0 && ps31 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am31 > 0 || as31 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm31 == 0 && ps31 == 0 && as31 == 0 && am31 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm31 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm31 == 0 && ps31 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am31 > 0 || as31 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm31 == 0 && ps31 == 0 && as31 == 0 && am31 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c31 == 0)
                                        {
                                            if (ps31 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm31 > 0 && ps31 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am31 > 0 || as31 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm31 == 0 && ps31 == 0 && as31 == 0 && am31 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_32")
                                    {
                                        #region fondo
                                        if (c32 == 0)
                                        {
                                            if (pm32 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm32 == 0 && ps32 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am32 > 0 || as32 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm32 == 0 && ps32 == 0 && as32 == 0 && am32 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm32 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm32 == 0 && ps32 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am32 > 0 || as32 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm32 == 0 && ps32 == 0 && as32 == 0 && am32 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c32 == 0)
                                        {
                                            if (ps32 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm32 > 0 && ps32 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am32 > 0 || as32 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm32 == 0 && ps32 == 0 && as32 == 0 && am32 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_33")
                                    {
                                        #region fondo
                                        if (c33 == 0)
                                        {
                                            if (pm33 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm33 == 0 && ps33 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am33 > 0 || as33 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm33 == 0 && ps33 == 0 && as33 == 0 && am33 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm33 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm33 == 0 && ps33 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am33 > 0 || as33 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm33 == 0 && ps33 == 0 && as33 == 0 && am33 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c33 == 0)
                                        {
                                            if (ps33 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm33 > 0 && ps33 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am33 > 0 || as33 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm33 == 0 && ps33 == 0 && as33 == 0 && am33 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_34")
                                    {
                                        #region fondo
                                        if (c34 == 0)
                                        {
                                            if (pm34 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm34 == 0 && ps34 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am34 > 0 || as34 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm34 == 0 && ps34 == 0 && as34 == 0 && am34 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            if (pm34 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (pm34 == 0 && ps34 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (am34 > 0 || as34 > 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm34 == 0 && ps34 == 0 && as34 == 0 && am34 == 0)
                                            {
                                                moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                            }
                                        }
                                        #endregion
                                        #region borde

                                        //colores de borde

                                        if (c34 == 0)
                                        {
                                            if (ps34 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                            }
                                            else if (pm34 > 0 && ps34 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                            }
                                            else if (am34 > 0 || as34 > 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                            }
                                            else if (pm34 == 0 && ps34 == 0 && as34 == 0 && am34 == 0)
                                            {
                                                moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                            }
                                        }
                                        else
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                        }
                                        #endregion
                                    }
                                    if (moduloLista.Name == "modulo_35")
                                {
                                    #region fondo
                                    if (c35 == 0)
                                    {
                                        if (pm35 > 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Red);
                                        }
                                        else if (pm35 == 0 && ps35 > 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                        }
                                        else if (am35 > 0 || as35 > 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                        }
                                        else if (pm35 == 0 && ps35 == 0 && as35 == 0 && am35 == 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Green);
                                        }
                                    }
                                    else
                                    {
                                        if (pm35 > 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Red);
                                        }
                                        else if (pm35 == 0 && ps35 > 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Orange);
                                        }
                                        else if (am35 > 0 || as35 > 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Yellow);
                                        }
                                        else if (pm35 == 0 && ps35 == 0 && as35 == 0 && am35 == 0)
                                        {
                                            moduloLista.Background = new SolidColorBrush(Colors.Blue);
                                        }
                                    }
                                    #endregion
                                    #region borde

                                    //colores de borde

                                    if (c35 == 0)
                                    {
                                        if (ps35 > 0)
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Orange);
                                        }
                                        else if (pm35 > 0 && ps35 == 0)
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Red);
                                        }
                                        else if (am35 > 0 || as35 > 0)
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Yellow);
                                        }
                                        else if (pm35 == 0 && ps35 == 0 && as35 == 0 && am35 == 0)
                                        {
                                            moduloLista.BorderBrush = new SolidColorBrush(Colors.Green);
                                        }
                                    }
                                    else
                                    {
                                        moduloLista.BorderBrush = new SolidColorBrush(Colors.Blue);
                                    }
                                    #endregion
                                }
                            }

                            }
                        }

                    }
            }));
        }
    }
}

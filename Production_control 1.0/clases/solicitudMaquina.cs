using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzCCO._0.clases
{
    public class solicitudMaquina
    {
        public int id_solicitud { get; set; }
        public string modulo { get; set; }
        public string maquina { get; set; }
        public float operario { get; set; }
        public string problema_reportado { get; set; }
        public string hora_reportada { get; set; }
        public string hora_asignacion { get; set; }
        public string hora_apertura { get; set; }
        public string hora_cierre { get; set; }
        public string corresponde { get; set; }
        public int prioridad { get; set; }
        public double tiempoPausa { get; set; }
        public string color { get; set; }
        public string mecanico { get; set; }
        public string tipoPrioridad { get; set; }
    }
}

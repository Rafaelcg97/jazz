using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class accionSolicitud
    {
        public int id { get; set; }
        public int num_solicitud { get; set;}
        public int codigo { get; set; }
        public string nombre { get; set; }
        public string hora { get; set; }
        public string motivo { get; set; }
        public double tiempor_por_solicitud { get; set; }
        public int tipo { get; set; }
        public int[] acciones { get; set; }
    }
}

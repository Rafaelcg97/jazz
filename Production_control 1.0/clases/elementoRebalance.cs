using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    class elementoRebalance:operacion
    {
        public double tiempoRebalance { get; set; }
        public double eficienciaRebalance { get; set; }
        public double cargaRebalance { get; set; }
    }
}

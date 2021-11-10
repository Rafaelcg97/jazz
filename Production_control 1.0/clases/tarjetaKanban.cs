using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class tarjetaKanban
    {
        public string tarjeta { get; set; }
        public int ordenPrioridad { get; set; }
        public string tanda { get; set; }
        public string lote { get; set; }
        public string cliente { get; set; }
        public string estilo { get; set; }
        public string modulo { get; set; }
        public string temporada { get; set; }
        public string color { get; set; }
        public int make { get; set; }
        public double sam { get; set; }
        public bool copa { get; set; }
        public bool seleccionado { get; set; }
    }
}

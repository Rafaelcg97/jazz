using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzCCO._0.clases
{
    public class loteConfirma: lote
    {
        public string modulo { get; set; }
        public DateTime confirmacionFecha { get; set; }
        public int semana { get; set; }
        public int anio { get; set; }
        public string confirmacion { get; set; }
        public string targetDate { get; set; }
        public string poNumber { get; set; }
        public string color { get; set; }
        public string coordinador { get; set; }
    }
}

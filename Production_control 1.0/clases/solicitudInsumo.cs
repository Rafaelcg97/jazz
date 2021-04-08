using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class solicitudInsumo
    {
        public string partNumber { get; set; }
        public string description { get; set; }
        public int onHand { get; set; }
        public int solicitado { get; set; }
        public double cost { get; set; }
        public string finalCategory { get; set; }
        public string color { get; set; }
        public string autorizado { get; set; }
        public string comentario { get; set; }
    }
}
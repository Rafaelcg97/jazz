using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzCCO._0.clases
{
    public class asistencia
    {
        public int codigo { get; set;}
        public string nombre { get; set;}
        public string modulo { get; set;}
        public int arteria { get; set; }
        public int arteriaAsignada { get; set; }
        public double tiempo { get; set; }
        public string puesto { get; set; }
        public string observaciones { get; set;}
        public string movimiento { get; set;}
        public List<string> modulos { get; set;}
        public string[] movimientos { get; set; }
        public int[] arterias { get; set; }
        public string[] puestos { get; set; }
    }
}

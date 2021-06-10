using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class moduloAdministrado
    {
        public int id { get; set; }
        public string modulo { get; set; }
        public string coordinadorNombre { get; set; }
        public int coordinadorCodigo { get; set; }
        public string ingenieroNombre { get; set; }
        public int ingenieroCodigo { get; set; }
        public string soporteNombre { get; set; }
        public int soporteCodigo { get; set; }
        public List<string> modulos { get; set; }
        public List<string> ingenieros { get; set; }
        public List<string> coordinadores { get; set; }
        public List<string> soportes { get; set; }
    }
}

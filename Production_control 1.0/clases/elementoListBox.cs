using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzCCO._0.clases
{
    public class elementoListBox : Operacion
    {
        public string identificador { get; set; }

        public string nombreEstacion { get; set; }
        public List<operacionesAgregadas> operacionesAgregadas {get;set;}
    }
}

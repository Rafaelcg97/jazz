using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class partNumber
    {
        public string partNumberNombre { get; set; }
        public string categoria { get; set; }
        public int subPaquete { get; set; }
        public double tiempoUnitario { get; set; }
        public double tiempoPaquete { get; set; }
        public double tiempoComplementario { get; set; }
        public string macroCategoria { get; set; }
        public List<int> paquetes { 
            get {
                List<int> listaPaquetes = new List<int>{ 1, 2, 3 };
                return listaPaquetes;
                    } 
        }
        public List<string> macrocategorias
        {
            get
            {
                List<string> listaCategoria = new List<string> { "CAJAS&GANCHOS", "ELASTICOS", "HILOS", "TRANSFER", "TRIMS",  };
                return listaCategoria;
            }
        }
    }
}

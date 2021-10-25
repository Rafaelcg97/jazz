using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class partNumber
    {
        public bool modificado { get; set; }
        public string partNumberNombre { get; set; }
        public string categoria { get; set; }
        public string categoriaOriginal { get; set; }
        public int subPaquete { get; set; }
        public int subPaqueteOriginal { get; set; }
        public double tiempoUnitario { get; set; }
        public double tiempoPaquete { get; set; }
        public double tiempoComplementario { get; set; }
        public string macroCategoria { get; set; }
        public int paquete { get; set; }
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
        public List<string> categorias { get; set; }
    }
}

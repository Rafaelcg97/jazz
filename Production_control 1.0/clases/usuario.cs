using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Production_control_1._0.clases
{
    public class usuario
    {
        public int id { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
        public int nivel { get; set; }
        public string cargo { get; set; }
        public string contrasenia { get; set; }
        public bool produccion { get; set; }
        public bool mantenimiento { get; set; }
        public bool bodega { get; set; }
        public bool ingenieria { get; set; }
        public bool kanban { get; set; }
        public int[] niveles { get; set; }
        public string[] cargos { get; set;}
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JazzCCO._0.clases
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
        public string enganchadorNombre { get; set; }
        public int enganchadorCodigo { get; set; }
        public string empacadorNombre { get; set; }
        public int empacadorCodigo { get; set; }
        public List<string> modulos { get; set; }
        public List<string> ingenieros { get; set; }
        public List<string> coordinadores { get; set; }
        public List<string> soportes { get; set; }
        public List<string> enganchadores { get; set; }
        public List<string> empacadores { get; set; }
    }
}

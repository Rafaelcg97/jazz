namespace ClasesTexops
{
    public class Lote:Estilo
    {
        string numeroLote = "";
        string tarjetaLote = "";
        string tandaLote = "";
        int manufactureIdLote = 0;
        int prioridadLote = 0;
        int make = 0;
        int makeXXS = 0;
        int makeXS = 0;
        int makeS = 0;
        int makeM = 0;
        int makeL = 0;
        int makeXL = 0;
        int makeXXL = 0;
        int makeXXXL = 0;
        int makeXXXXL = 0;
        int make1X = 0;
        int make2X = 0;
        int make3X = 0;
        int terminadoXXS = 0;
        int terminadoXS = 0;
        int terminadoS = 0;
        int terminadoM = 0;
        int terminadoL = 0;
        int terminadoXL = 0;
        int terminadoXXL = 0;
        int terminadoXXXL = 0;
        int terminadoXXXXL = 0;
        int terminado1X = 0;
        int terminado2X = 0;
        int terminado3X = 0;

        public Lote(string numeroLote, int manufactureIdLote, string nombreEstilo, string temporadaEstilo, string colorEstilo, int make)
        {
            this.numeroLote = numeroLote;
            this.manufactureIdLote = manufactureIdLote;
            NombreEstilo = nombreEstilo;
            TemporadaEstilo = temporadaEstilo;
            ColorEstilo = colorEstilo;
            this.make = make;
        }

        public Lote(string numeroLote)
        {
            this.numeroLote = numeroLote;
        }

        public string NumeroLote { get => numeroLote; set => numeroLote = value; }
        public string TarjetaLote { get => tarjetaLote; set => tarjetaLote = value; }
        public string TandaLote { get => tandaLote; set => tandaLote = value; }
        public int PrioridadLote { get => prioridadLote; set => prioridadLote = value; }
        public int MakeXXS { get => makeXXS; set => makeXXS = value; }
        public int MakeXS { get => makeXS; set => makeXS = value; }
        public int MakeS { get => makeS; set => makeS = value; }
        public int MakeM { get => makeM; set => makeM = value; }
        public int MakeL { get => makeL; set => makeL = value; }
        public int MakeXL { get => makeXL; set => makeXL = value; }
        public int MakeXXL { get => makeXXL; set => makeXXL = value; }
        public int MakeXXXL { get => makeXXXL; set => makeXXXL = value; }
        public int MakeXXXXL { get => makeXXXXL; set => makeXXXXL = value; }
        public int Make1X { get => make1X; set => make1X = value; }
        public int Make2X { get => make2X; set => make2X = value; }
        public int Make3X { get => make3X; set => make3X = value; }
        public int TerminadoXXS { get => terminadoXXS; set => terminadoXXS = value; }
        public int TerminadoXS { get => terminadoXS; set => terminadoXS = value; }
        public int TerminadoS { get => terminadoS; set => terminadoS = value; }
        public int TerminadoM { get => terminadoM; set => terminadoM = value; }
        public int TerminadoL { get => terminadoL; set => terminadoL = value; }
        public int TerminadoXL { get => terminadoXL; set => terminadoXL = value; }
        public int TerminadoXXL { get => terminadoXXL; set => terminadoXXL = value; }
        public int TerminadoXXXL { get => terminadoXXXL; set => terminadoXXXL = value; }
        public int TerminadoXXXXL { get => terminadoXXXXL; set => terminadoXXXXL = value; }
        public int Terminado1X { get => terminado1X; set => terminado1X = value; }
        public int Terminado2X { get => terminado2X; set => terminado2X = value; }
        public int Terminado3X { get => terminado3X; set => terminado3X = value; }
        public int MakeTotal { get => makeXXS + makeXS + makeS + makeM + makeL + makeXL + makeXXL + makeXXXL + makeXXXXL + Make1X + make2X + make3X; }
        public int TerminadoTotal { get => terminadoXXS + terminadoXS + terminadoS + terminadoM + terminadoL + terminadoXL + terminadoXXL + terminadoXXXL + terminadoXXXXL + Make1X + terminado2X + terminado3X; }
        public int Make { get => make; }
        public int ManufactureIdLote { get => manufactureIdLote; set => manufactureIdLote = value; }
    }
}

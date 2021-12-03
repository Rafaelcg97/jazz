namespace ClasesTexops
{
    public class Modulo
    {
        string nombreModulo = "";
        int arteriaModulo = 1;
        int ubicacionModulo = 1;

        public Modulo(string nombreModulo, int arteriaModulo, int ubicacionModulo)
        {
            this.nombreModulo = nombreModulo;
            this.arteriaModulo = arteriaModulo;
            this.ubicacionModulo = ubicacionModulo;
        }

        public Modulo(string nombreModulo, int ubicacionModulo)
        {
            this.nombreModulo = nombreModulo;
            this.ubicacionModulo = ubicacionModulo;
        }

        public Modulo(string nombreModulo)
        {
            this.nombreModulo = nombreModulo;
        }

        public Modulo()
        {
        }

        public string NombreModulo { get => nombreModulo; set => nombreModulo = value; }
        public int ArteriaModulo { get => arteriaModulo; set => arteriaModulo = value; }
        public int UbicacionModulo { get => ubicacionModulo; set => ubicacionModulo = value; }
    }
}

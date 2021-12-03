namespace ClasesTexops
{
    public class Estilo:Cliente
    {
        string nombreEstilo = "";
        string temporadaEstilo = "";
        string tipoEstilo = "";
        string empaqueEstilo = "";
        string colorEstilo = "";
        double samCostura = 0;
        double samEmpaque = 0;
        double samEstilo=0;

        public string NombreEstilo { get => nombreEstilo; set => nombreEstilo = value; }
        public string TemporadaEstilo { get => temporadaEstilo; set => temporadaEstilo = value; }
        public string TipoEstilo { get => tipoEstilo; set => tipoEstilo = value; }
        public string EmpaqueEstilo { get => empaqueEstilo; set => empaqueEstilo = value; }
        public double SamCostura { get => samCostura; set => samCostura = value; }
        public double SamEmpaque { get => samEmpaque; set => samEmpaque = value; }
        public double SamEstilo { get => samCostura+ samEmpaque; }
        public string ColorEstilo { get => colorEstilo; set => colorEstilo = value; }
    }
}

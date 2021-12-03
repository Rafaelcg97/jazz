namespace ClasesTexops
{
    public class Celulabmp
    {
        string nombreCelula="";
        string categoriaCelula = "";
        int subcategoriaCelula = 1;
        double samUnitarioConteo = 0;
        double samPaqueteConteo = 0;
        double samComplemento = 0;

        public string NombreCelula { get => nombreCelula; set => nombreCelula = value; }
        public string CategoriaCelula { get => categoriaCelula; set => categoriaCelula = value; }
        public int SubcategoriaCelula { get => subcategoriaCelula; set => subcategoriaCelula = value; }
        public double SamUnitarioConteo { get => samUnitarioConteo; set => samUnitarioConteo = value; }
        public double SamPaqueteConteo { get => samPaqueteConteo; set => samPaqueteConteo = value; }
        public double SamComplemento { get => samComplemento; set => samComplemento = value; }
    }
}

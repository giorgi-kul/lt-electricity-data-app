using CsvHelper.Configuration.Attributes;

namespace ElectricityDataApp.DataParser.Models
{
    public class Record
    {
        [Name("TINKLAS")]
        public string Tinklas { get; set; }

        [Name("OBT_PAVADINIMAS")]
        public string ObtPavadinimas { get; set; }

        [Name("OBJ_GV_TIPAS")]
        public string ObjGvTipas { get; set; }

        [Name("OBJ_NUMERIS")]
        public string ObjNumeris { get; set; }

        [Name("P+")]
        public string PPlus { get; set; }

        [Name("PL_T")]
        public string PlT { get; set; }

        [Name("P-")]
        public string PMinus { get; set; }
    }
}

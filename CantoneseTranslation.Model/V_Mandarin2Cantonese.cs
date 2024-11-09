namespace CantoneseTranslation.Model
{
    public class V_Mandarin2Cantonese
    {
        public int Id { get; set; }
        public string Mandarin { get; set; }
        public string Cantonese { get; set; }
        public string MandarinSynonym { get; set; }
        public string CantoneseSynonym { get; set; }
        public string Exclusion { get; set; }       
        public string MandarinRegex { get; set; }
        public string CantoneseRegex { get; set; }
        public int PatternId { get; set; }       
        public string Pattern { get; set; }
        public bool IsStart { get; set; }
        public bool IsMiddle { get; set; }
        public bool IsEnd { get; set; }
        public string PatternNotes { get; set; }
    }
}

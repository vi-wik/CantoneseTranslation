namespace CantoneseTranslation.Business.Model
{
    public enum TranslateType
    {
        Mandarin2Cantonese=1,
        Cantonese2Mandarin=2
    }

    public class TranslationResult
    {
        public List<string> Contents { get; set; } = new List<string>();
        public List<string> Examples { get; set; } = new List<string>();
        public string PatternNotes { get; set; }
    }
}

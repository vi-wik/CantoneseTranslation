namespace CantoneseTranslation.Model
{
    public class CantoneseSentencePattern
    {
        public int Id { get; set; }
        public string Pattern { get; set; }
        public bool IsStart { get; set; }
        public bool IsMiddle { get; set; }
        public bool IsEnd { get; set; }
        public bool IsMood { get; set; }
        public string Notes { get; set; }
    }
}

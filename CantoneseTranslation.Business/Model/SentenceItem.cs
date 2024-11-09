namespace CantoneseTranslation.Business.Model
{
    public class SentenceItem
    {
        public string Word { get; set; }
        public int StartIndex { get; set; }

        public bool IsMatched { get; set; }

        public string ReplaceValue { get; set; }

        public int StopIndex
        {
            get { return this.StartIndex + this.Word.Length -1; }
        }
    }
}

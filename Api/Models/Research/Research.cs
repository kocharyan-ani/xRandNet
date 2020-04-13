namespace Api.Models.Research
{
    public class Research
    {
        public string name { get; set; }
        public string research { get; set; }
        public string model { get; set; }
        public string generation { get; set; }
        public bool connected { get; set; }
        public int count { get; set; }
        public string storage { get; set; }
        public AnalyzeOption[] analyzeOptions { get; set; }
        public Parameter[] parameters { get; set; }
    }
}
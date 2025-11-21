namespace Lab6.Config
{
    public class BlogConfig
    {
        public string DateFormat { get; set; } = "Standard";

        public int SummaryWordCount { get; set; }

        public string DateFormatSwitch =>
            DateFormat switch
            {
                "USA" => "MM/dd/yyyy",
                "Standard" or _ => "yyyy-MM-dd"
            };
    }
}

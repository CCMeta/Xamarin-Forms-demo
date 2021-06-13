namespace Xamarin_Forms_demo.Models
{
    public class ExamTranscripts
    {
        public int id { get; set; }
        public int uid { get; set; }
        public int duration { get; set; }
        public string title { get; set; }
        public string major { get; set; }
        public double score { get; set; }
        public string created_at { get; set; }

        public string DurationFormat => string.Format("{0:D2}:{1:D2}",
            System.TimeSpan.FromSeconds(duration).Minutes, System.TimeSpan.FromSeconds(duration).Seconds);

    }
}

namespace Xamarin_Forms_demo_api.Models
{
    public class ExamAnswers
    {
        public int id { get; set; }
        public int uid { get; set; }
        public int questionId { get; set; }
        public string answer { get; set; }
        public double point { get; set; }
        public string created_at { get; set; }

    }
}

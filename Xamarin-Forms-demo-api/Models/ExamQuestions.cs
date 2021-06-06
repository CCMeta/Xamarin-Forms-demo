namespace Xamarin_Forms_demo_api.Models
{
    public class ExamQuestions
    {
        public int id { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public string answer { get; set; }
        public string analysis { get; set; }
        public string created_at { get; set; }
        public int exam_id { get; set; }
        public string op_a { get; set; }
        public string op_b { get; set; }
        public string op_c { get; set; }
        public string op_d { get; set; }
    }
}

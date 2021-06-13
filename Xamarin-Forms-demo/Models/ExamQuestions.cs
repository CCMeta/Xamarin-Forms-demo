namespace Xamarin_Forms_demo.Models
{
    public class ExamQuestions
    {
        public int id { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public string true_answer { get; set; }
        public string analysis { get; set; }
        public string created_at { get; set; }
        public int exam_id { get; set; }
        public string op_a { get; set; }
        public string op_b { get; set; }
        public string op_c { get; set; }
        public string op_d { get; set; }
        public bool isCheckA { get; set; }
        public bool isCheckB { get; set; }
        public bool isCheckC { get; set; }
        public bool isCheckD { get; set; }

        public bool IsAnswered => isCheckA || isCheckB || isCheckC || isCheckD;
        public bool IsUnanswered => !IsAnswered;
    }
}

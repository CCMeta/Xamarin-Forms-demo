using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;
using System.Linq;
using System;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ExamQuestionsViewModel : BaseViewModel
    {
        public readonly int _exam_id;
        private readonly string path = "/api/exams/{0}/questions";//i should take exam_id to query not path
        public List<ExamAnswers> examAnswers = new List<ExamAnswers>();
        public ObservableCollection<ExamQuestions> examQuestions = new ObservableCollection<ExamQuestions>();
        public ObservableCollection<ExamQuestions> ExamQuestions
        {
            get => examQuestions;
            set
            {
                foreach (var item in value)
                {
                    examQuestions.Add(item);
                }
            }
        }

        public ExamQuestionsViewModel(int exam_id) : base()
        {
            _exam_id = exam_id;
            Title = "ExamQuestions";
        }

        public void OnAnswerClick(int questionId, string answer)
        {
            examAnswers.First(answer => answer.questionId == questionId).answer = answer;
        }

        public async void GetListAsync()
        {
            var queryParams = new Dictionary<string, string>() { };
            ExamQuestions = await HttpRequest.GetAsync<ObservableCollection<ExamQuestions>>(string.Format(path, _exam_id), queryParams: queryParams);
            foreach (var question in ExamQuestions)
            {
                examAnswers.Add(new ExamAnswers { questionId = question.id });
            }
            IsBusy = false;
        }


    }
}
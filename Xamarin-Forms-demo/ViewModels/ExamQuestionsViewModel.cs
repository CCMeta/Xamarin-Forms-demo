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
        private int _exam_id;
        private readonly string path = "/api/exams/{0}/questions";
        public Dictionary<int, ExamAnswers> examAnswers = new Dictionary<int, ExamAnswers>();
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

        public ICommand OnCommitPaperCommand { protected set; get; }

        public ExamQuestionsViewModel(int exam_id) : base()
        {
            _exam_id = exam_id;
            Title = "ExamQuestions";
            OnCommitPaperCommand = new Command(async () =>
            {
                await PostListAsync();
            });
        }

        public void OnAnswerClick(int questionId, string answer)
        {
            examAnswers[questionId].answer = answer;
        }

        public async void GetListAsync()
        {
            var queryParams = new Dictionary<string, string>() { };
            ExamQuestions = await HttpRequest.GetAsync<ObservableCollection<ExamQuestions>>(string.Format(path, _exam_id), queryParams: queryParams);
            foreach (var question in ExamQuestions)
            {
                examAnswers.Add(question.id, new ExamAnswers { questionId = question.id });
            }
            IsBusy = false;
        }

        public async Task<bool> PostListAsync()
        {
            var result = await HttpRequest.PostAsync(path, examAnswers);
            if (result is Dictionary<int, ExamAnswers>)
                return true;
            return false;
        }
    }
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ExamQuestionsViewModel : BaseViewModel
    {
        private int _exam_id;
        private readonly string path = "/api/exams/{0}/questions";
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

        public ICommand OnAnswerClickCommand { protected set; get; }

        public ExamQuestionsViewModel(int exam_id) : base()
        {
            _exam_id = exam_id;
            Title = "ExamQuestions";
            OnAnswerClickCommand = new Command(() =>
            {
                OnAnswerClickAsync();
            });
        }

        public async void OnAnswerClickAsync()
        {

        }

        public async void GetListAsync()
        {
            var queryParams = new Dictionary<string, string>() { };
            ExamQuestions = await HttpRequest.GetAsync<ObservableCollection<ExamQuestions>>(string.Format(path, _exam_id), queryParams: queryParams);
            IsBusy = false;
        }

        public async Task<bool> PostAsync()
        {
            var queryParams = new List<ExamAnswers>();
            var result = await HttpRequest.PostAsync(path, queryParams);
            if (result is List<ExamAnswers>)
                return true;
            return false;
        }
    }
}
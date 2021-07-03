using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ExamAnswersViewModel : BaseViewModel
    {
        private readonly string path = "/api/ExamTranscripts/Answers";
        private ObservableCollection<ExamAnswers> examAnswers = new ObservableCollection<ExamAnswers>();
        public ObservableCollection<ExamAnswers> ExamAnswers
        {
            get => examAnswers;
            set
            {
                foreach (var item in value)
                {
                    examAnswers.Add(item);
                }
            }
        }

        public ICommand GetListCommand { protected set; get; }

        public ExamAnswersViewModel() 
        {
        }

        public async Task<int> PostListAsync(ExamAnswers[] examAnswers)
        {
            var result = await HttpRequest.PostAsync(path, examAnswers.ToArray());
            if (result is ExamAnswers[] && result.Length > 0)
                return result[0].transcriptId;
            return -1;
        }

        public async Task GetListByTranscriptIdAsync(int transcriptId)
        {
            var queryParams = new Dictionary<string, string>() {
                { "transcriptId", transcriptId.ToString() },
            };
            ExamAnswers = await HttpRequest.GetAsync<ObservableCollection<ExamAnswers>>(path, queryParams: queryParams);
            IsBusy = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ExamQuestionsViewModel : BaseViewModel
    {
        private readonly string path = "/api/ExamQuestions";
        public ObservableCollection<ExamQuestions> examQuestions = new ObservableCollection<ExamQuestions>();
        public ObservableCollection<ExamQuestions> ExamQuestions
        {
            get { return examQuestions; }
            set
            {
                foreach (var item in value)
                {
                    examQuestions.Insert(0, item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public ExamQuestionsViewModel() : base()
        {
            Title = "ExamQuestions";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            var page = Math.Ceiling((double)(ExamQuestions.Count() + 1) / 5).ToString();
            var queryParams = new Dictionary<string, string>() {
                    { "p", page }
            };
            ExamQuestions = await HttpRequest.GetAsync<ObservableCollection<ExamQuestions>>(path, queryParams: queryParams);
            IsBusy = false;
        }

    }
}
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
    public class ExamTranscriptsViewModel : BaseViewModel
    {
        private readonly string path = "/api/ExamTranscripts";
        public ObservableCollection<ExamTranscripts> examTranscripts = new ObservableCollection<ExamTranscripts>();
        public ObservableCollection<ExamTranscripts> ExamTranscripts
        {
            get { return examTranscripts; }
            set
            {
                foreach (var item in value)
                {
                    examTranscripts.Insert(0, item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public ExamTranscriptsViewModel() : base()
        {
            Title = "ExamTranscriptsViewModel";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            var queryParams = new Dictionary<string, string>() { };
            ExamTranscripts = await HttpRequest.GetAsync<ObservableCollection<ExamTranscripts>>(path, queryParams: queryParams);
            IsBusy = false;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;

namespace Xamarin_Forms_demo.ViewModels
{
    public class SubjectsViewModel : BaseViewModel
    {
        private readonly string path = "/api/subjects";
        public ObservableCollection<Subjects> subjects = new ObservableCollection<Subjects>();
        public ObservableCollection<Subjects> Subjects
        {
            get { return subjects; }
            set
            {
                foreach (var item in value)
                {
                    subjects.Add(item);
                }
            }
        }
        public ICommand GetSubjectsCommand { protected set; get; }

        public SubjectsViewModel() : base()
        {
            Title = "SubjectsViewModel";
            GetSubjectsCommand = new Command(() =>
            {
                GetSubjectsAsync();
            });
            GetSubjectsAsync();
        }

        public async void GetSubjectsAsync()
        {
            if (IsBusy)
                return;
            IsBusy = true;
            int maxId = Subjects.Count > 0 ? Subjects[0].id : 0;
            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            Subjects = await HttpRequest.GetAsync<ObservableCollection<Subjects>>(path, queryParams: queryParams);
            IsBusy = false;
        }

        public ICommand OpenWebCommand { get; }
    }
}
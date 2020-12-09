using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
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
                OnPropertyChanged("Subjects");
            }
        }
        public ICommand GetSubjectsCommand { protected set; get; }

        public SubjectsViewModel()
        {
            Title = "About";
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
            var page = (Math.Ceiling((double)Subjects.Count() / 5) + 1).ToString();
            var queryParams = new Dictionary<string, string>() {
                    { "fuck", page }
            };
            var list = await HttpRequest.GetAsync<ObservableCollection<Subjects>>(path, queryParams: queryParams);
            if (list.Count() > 0)
            {
                Subjects = list;
                IsBusy = false;
            }
        }

        public ICommand OpenWebCommand { get; }
    }
}
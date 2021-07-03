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
    public class ExamsViewModel : BaseViewModel
    {
        private readonly string path = "/api/exams";
        public ObservableCollection<Exams> exams = new ObservableCollection<Exams>();
        public ObservableCollection<Exams> Exams
        {
            get { return exams; }
            set
            {
                foreach (var item in value)
                {
                    exams.Insert(0, item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public ExamsViewModel()
        {
            Title = "Exams";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            int maxId = Exams.Count > 0 ? Exams[0].id : 0;
            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            Exams = await HttpRequest.GetAsync<ObservableCollection<Exams>>(path, queryParams: queryParams);
            IsBusy = false;
        }

    }
}
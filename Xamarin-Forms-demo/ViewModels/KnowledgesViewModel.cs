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
    public class KnowledgesViewModel : BaseViewModel
    {
        private readonly string path = "/api/Knowledges";
        public ObservableCollection<Knowledges> knowledges = new ObservableCollection<Knowledges>();
        public ObservableCollection<Knowledges> Knowledges
        {
            get { return knowledges; }
            set
            {
                foreach (var item in value)
                {
                    knowledges.Insert(0, item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public KnowledgesViewModel()
        {
            Title = "Knowledges";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            int maxId = Knowledges.Count > 0 ? Knowledges[0].id : 0;
            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            Knowledges = await HttpRequest.GetAsync<ObservableCollection<Knowledges>>(path, queryParams: queryParams);
            IsBusy = false;
        }

    }
}
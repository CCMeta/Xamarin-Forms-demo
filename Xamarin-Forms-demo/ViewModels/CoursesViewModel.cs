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
    public class CoursesViewModel : BaseViewModel
    {
        private readonly string path = "/api/courses";
        public ObservableCollection<Courses> courses = new ObservableCollection<Courses>();
        public ObservableCollection<Courses> Courses
        {
            get { return courses; }
            set
            {
                foreach (var item in value)
                {
                    courses.Insert(0, item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public CoursesViewModel() : base()
        {
            Title = "CoursesViewModel";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            var page = Math.Ceiling((double)(Courses.Count() + 1) / 5).ToString();
            var queryParams = new Dictionary<string, string>() {
                    { "p", page }
            };
            Courses = await HttpRequest.GetAsync<ObservableCollection<Courses>>(path, queryParams: queryParams);
            IsBusy = false;
        }

    }
}
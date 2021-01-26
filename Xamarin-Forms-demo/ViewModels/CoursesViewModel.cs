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
            Title = "Courses";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            int maxId = Courses.Count > 0 ? Courses[0].id : 0;
            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            using var _ = HttpRequest.GetAsync<ObservableCollection<Courses>>(path, queryParams: queryParams);
            Courses = await _;
            IsBusy = false;
        }

    }
}
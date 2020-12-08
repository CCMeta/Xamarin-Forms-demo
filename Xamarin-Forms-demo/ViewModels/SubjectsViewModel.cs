using System.Collections;
using System.Collections.ObjectModel;
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

        public SubjectsViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public async Task<ObservableCollection<Subjects>> GetSubjectsAsync()
        {
            return await HttpRequest.GetAsync<ObservableCollection<Subjects>>(path);
        }

        public ICommand OpenWebCommand { get; }
    }
}
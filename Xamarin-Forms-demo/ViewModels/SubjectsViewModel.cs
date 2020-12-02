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
        private const string JSON_DATA = "https://ccmeta.com/Subjects.json";

        public SubjectsViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public async Task<ObservableCollection<Subjects>> GetSubjectsAsync()
        {
            var httpClient = await new HttpClient().GetAsync(JSON_DATA);
            var stream = await httpClient.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ObservableCollection<Subjects>>(stream);
        }

        public ICommand OpenWebCommand { get; }
    }
}
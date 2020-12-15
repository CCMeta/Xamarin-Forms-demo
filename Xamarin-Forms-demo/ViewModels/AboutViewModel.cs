using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Xamarin_Forms_demo.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel() : base()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }

        public ICommand OpenWebCommand { get; }
    }
}
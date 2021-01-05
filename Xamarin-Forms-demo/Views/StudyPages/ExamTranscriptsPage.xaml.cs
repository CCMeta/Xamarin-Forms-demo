using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ExamTranscriptsPage : ContentPage
    {
        public ExamTranscriptsPage()
        {
            InitializeComponent();
        }

        protected override async void OnDisappearing()
        {
            await Navigation.PopToRootAsync();
        }
    }
}
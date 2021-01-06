using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class DataIndexPage : ContentPage
    {
        private readonly ExamTranscriptsViewModel _examTranscriptsViewModel;
        public DataIndexPage()
        {
            InitializeComponent();
            BindingContext = _examTranscriptsViewModel = new ExamTranscriptsViewModel();
        }
        private async void OnEnterExamTranscriptsPageAsync(object sender, SelectionChangedEventArgs e)
        {
            var selected_id = ((ExamTranscripts)e.CurrentSelection[0]).id;
            await Navigation.PushAsync(new ExamTranscriptsPage(selected_id));
        }
    }
}
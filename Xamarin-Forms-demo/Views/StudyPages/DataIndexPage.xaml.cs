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

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _examTranscriptsViewModel.GetListAsync();
        }

        private async void OnEnterExamTranscriptsPageAsync(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
                return;
            var selected_id = ((ExamTranscripts)e.CurrentSelection[0]).id;
            await Navigation.PushAsync(new ExamTranscriptsPage(selected_id));
            (sender as CollectionView).SelectedItem = null;
        }
    }
}
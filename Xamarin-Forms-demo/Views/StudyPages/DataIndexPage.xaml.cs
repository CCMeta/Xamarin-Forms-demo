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
            Title = "数据中心";
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
            await Navigation.PushAsync(new ExamTranscriptsPage((e.CurrentSelection[0] as ExamTranscripts).id));
            (sender as CollectionView).SelectedItem = null;
        }

        private void OnTypeButtonToggle(object sender, EventArgs e)
        {
            //make all box to transparent.
            BoxView boxView;
            foreach (var child in listTabNavbar.Children)
            {
                boxView = ((StackLayout)child).Children[1] as BoxView;
                boxView.Color = Color.Transparent;
            }

            //this is a important thing to get a element in a event just remeber the |as| act
            boxView = (((Button)sender).Parent as StackLayout).Children[1] as BoxView;
            Console.WriteLine(boxView.ClassId);
            boxView.Color = Color.FromHex("#00cccc");
        }

    }
}
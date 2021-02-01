using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ClassIndexPage : ContentPage
    {
        private readonly CoursesViewModel _coursesViewModel;

        public ClassIndexPage()
        {
            InitializeComponent();
            BindingContext = _coursesViewModel = new CoursesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            _coursesViewModel.GetListAsync();
        }

        private async void OnCoursesSelectedAsync(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count < 1)
                return;

            string videoUrl = (e.CurrentSelection[0] as Courses).video;
            await Navigation.PushAsync(new VideoPage(videoUrl));
            //await Navigation.PushModalAsync(new VideoPage(videoUrl));
            (sender as CollectionView).SelectedItem = null;
        }
    }
}
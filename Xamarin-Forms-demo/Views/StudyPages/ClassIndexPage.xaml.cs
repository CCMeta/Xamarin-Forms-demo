using System;
using System.ComponentModel;
using Xamarin.Forms;
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

    }
}
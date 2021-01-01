using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class TestIndexPage : ContentPage
    {
        private readonly ExamsViewModel ExamsViewModel;
        public TestIndexPage()
        {
            InitializeComponent();
            BindingContext = ExamsViewModel = new ExamsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            ExamsViewModel.GetListAsync();
        }

        private async void OnEnterExamQuestionsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExamQuestionsPage(1));
        }
    }
}
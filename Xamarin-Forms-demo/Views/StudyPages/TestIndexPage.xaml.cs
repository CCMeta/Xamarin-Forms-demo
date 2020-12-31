using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class TestIndexPage : ContentPage
    {
        private readonly ExamQuestionsViewModel ExamQuestionsViewModel;
        public TestIndexPage()
        {
            InitializeComponent();
            BindingContext = ExamQuestionsViewModel = new ExamQuestionsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            ExamQuestionsViewModel.GetListAsync();
        }

        private async void OnEnterExamQuestionsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExamQuestionsPage());
        }
    }
}
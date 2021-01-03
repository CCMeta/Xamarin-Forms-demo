using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
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
            ExamsViewModel.GetListAsync();
        }

        private async void OnEnterExamQuestionsPageAsync(object sender, SelectionChangedEventArgs e)
        {
            var selected_id = ((Exams)e.CurrentSelection[0]).id;
            await Navigation.PushAsync(new ExamQuestionsPage(selected_id));
        }

    }
}
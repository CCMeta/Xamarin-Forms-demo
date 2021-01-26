using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
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
            if (e.CurrentSelection.Count < 1)
                return;
            int selected_id = ((Exams)e.CurrentSelection[0]).id;
            (sender as CollectionView).SelectedItem = null;
            await Navigation.PushAsync(new ExamQuestionsPage(selected_id));
        }
    }
}
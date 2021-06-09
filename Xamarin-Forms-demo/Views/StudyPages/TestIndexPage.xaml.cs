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
            (sender as CollectionView).SelectedItem = null;
            //int selected_id = ((Exams)e.CurrentSelection[0]).id;
            var selected = (Exams)e.CurrentSelection[0];
            await Navigation.PushAsync(new ExamQuestionsPage(selected));
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
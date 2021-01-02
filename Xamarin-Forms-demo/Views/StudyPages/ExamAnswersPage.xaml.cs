using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ExamAnswersPage : ContentPage
    {

        public ExamAnswersPage()
        {
            InitializeComponent();
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current_id = ((ExamQuestions)e.CurrentSelection[0]).id;
            ((ExamQuestionsViewModel)BindingContext).OnAnswerClick(current_id, $"Answer = {current_id}");
        }
    }
}
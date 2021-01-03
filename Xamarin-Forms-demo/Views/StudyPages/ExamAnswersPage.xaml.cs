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
        private readonly ExamQuestionsViewModel _examQuestionsViewModel;
        public ExamAnswersPage(ExamQuestionsViewModel examQuestionsViewModel)
        {
            InitializeComponent();
            BindingContext = _examQuestionsViewModel = examQuestionsViewModel;
        }

        private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var current_id = ((ExamQuestions)e.CurrentSelection[0]).id;
            _examQuestionsViewModel.OnAnswerClick(current_id, $"Answer = {current_id}");
        }

        private async void OnCommitPaperAsync(object sender, EventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            var examAnswers = _examQuestionsViewModel.examAnswers;
            var _examAnswersViewModel = new ExamAnswersViewModel(_examQuestionsViewModel._exam_id);
            Content.IsEnabled = false;

            //This using is super cool for async var to go with life before not async expressions.
            using var result = _examAnswersViewModel.PostListAsync(examAnswers.ToArray());
            if (!await result)
            {
                throw new Exception("ExamAnswersViewModel.PostListAsync");
            }
            await DisplayAlert("Result", "Success", "Cancel");
            Content.IsEnabled = true;
        }
    }
}
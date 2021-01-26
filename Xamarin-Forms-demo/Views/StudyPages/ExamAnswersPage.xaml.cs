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
            if (e.CurrentSelection.Count < 1)
                return;
            var current_id = ((ExamQuestions)e.CurrentSelection[0]).id;
            var answer = $"Answer = {current_id}";
            _examQuestionsViewModel.OnAnswerClick(current_id, answer);
            (sender as CollectionView).SelectedItem = null;
        }

        private async void OnCommitPaperAsync(object sender, EventArgs e)
        {
            Content.IsEnabled = false;
            var examAnswers = _examQuestionsViewModel.examAnswers;
            var _examAnswersViewModel = new ExamAnswersViewModel(_examQuestionsViewModel._exam_id);

            //This using is super cool for async var to go with life before not async expressions.
            using var result = _examAnswersViewModel.PostListAsync(examAnswers.ToArray());
            if (!await result)
            {
                throw new Exception("ExamAnswersViewModel.PostListAsync");
            }
            await DisplayAlert("Result", "Success", "OK");
            Content.IsEnabled = true;
            await Navigation.PushAsync(new ExamTranscriptsPage());
        }
    }
}
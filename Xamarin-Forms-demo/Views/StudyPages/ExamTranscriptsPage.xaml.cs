using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ExamTranscriptsPage : ContentPage
    {
        private readonly ExamAnswersViewModel _examAnswersViewModel;

        public ExamTranscriptsPage(int transcriptId, string title = "")
        {
            InitializeComponent();

            Title = title;
            BindingContext = _examAnswersViewModel = new ExamAnswersViewModel();
            Task.Run(async () => await _examAnswersViewModel.GetListByTranscriptIdAsync(transcriptId)).Wait();
            var _ = _examAnswersViewModel.ExamAnswers;

            TotalCountSpan.Text = $"{ _examAnswersViewModel.ExamAnswers.Count:D2}";
            SetCurrentPositionText(position: 0);
        }

        protected override async void OnDisappearing()
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnEnterAnswerCard(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TranscriptAnswersPage(_examAnswersViewModel));
        }

        private void OnOrderNumberChanged(object sender, PositionChangedEventArgs e)
        {
            SetCurrentPositionText(e.CurrentPosition);
        }

        private void SetCurrentPositionText(int position)
        {
            CurrentPositionSpan.Text = $"  {++position:D2} /  ";
        }

        public void SetCurrentPosition(int position)
        {
            ExamQuestionsView.Position = position;
        }

        private void OnShowAnalysisModal(object sender, EventArgs e)
        {
            var currentLayout = ExamQuestionsView.VisibleViews[0] as AbsoluteLayout;
            currentLayout.Children[1].IsVisible = !currentLayout.Children[1].IsVisible;
        }
    }
}
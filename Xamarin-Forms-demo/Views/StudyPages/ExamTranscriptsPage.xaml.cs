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
        public ExamTranscriptsPage(int examTranscriptId)
        {
            InitializeComponent();
            BindingContext = _examAnswersViewModel = new ExamAnswersViewModel();
            var _ =Task.Run(async () => await _examAnswersViewModel.GetListByTranscriptIdAsync(examTranscriptId)).Result;

        }

        protected override async void OnDisappearing()
        {
            await Navigation.PopToRootAsync();
        }
    }
}
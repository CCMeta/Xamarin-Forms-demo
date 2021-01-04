using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ExamQuestionsPage : ContentPage
    {
        private readonly ExamQuestionsViewModel _examQuestionsViewModel;
        public ExamQuestionsPage(int exam_id)
        {
            InitializeComponent();
            BindingContext = _examQuestionsViewModel = new ExamQuestionsViewModel(exam_id);
            _examQuestionsViewModel.GetListAsync();
        }

        private void OnAnswerSelected(object sender, CheckedChangedEventArgs e)
        {
            var current_id = ((ExamQuestions)ExamQuestionsView.CurrentItem).id;
            var answer = (sender as RadioButton).Value.ToString();
            _examQuestionsViewModel.OnAnswerClick(current_id, answer);
        }

        private async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExamAnswersPage(_examQuestionsViewModel));
        }
    }
}
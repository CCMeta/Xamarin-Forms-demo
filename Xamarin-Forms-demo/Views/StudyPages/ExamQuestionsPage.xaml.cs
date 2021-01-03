using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
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


        private async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExamAnswersPage(_examQuestionsViewModel));
        }
    }
}
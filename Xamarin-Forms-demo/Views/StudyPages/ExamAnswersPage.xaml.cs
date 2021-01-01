using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ExamAnswersPage : ContentPage
    {
        private readonly ExamQuestionsViewModel ExamQuestionsViewModel;

        public ExamAnswersPage(ExamQuestionsViewModel ExamQuestionsViewModel)
        {
            InitializeComponent();
            Title = "ExamAnswersPage";
            BindingContext = ExamQuestionsViewModel;
        }
    }
}
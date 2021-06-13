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

        public ExamQuestionsPage(Exams currentExam)
        {
            //[RecyclerView] Cannot scroll to position a LayoutManager set. Call setLayoutManager with a non-null argument.
            //var fuck = new CarouselView();



            InitializeComponent();
            Title = currentExam.title;
            BindingContext = _examQuestionsViewModel = new ExamQuestionsViewModel(currentExam.id);
            Task.Run(async () =>
                await _examQuestionsViewModel.GetListAsync()
            ).Wait();
            TotalCountSpan.Text = $"{ _examQuestionsViewModel.ExamQuestions.Count:D2}";
            SetCurrentPositionText(position: 0);
        }

        private void OnAnswerSelected2(object sender, CheckedChangedEventArgs e)
        {
            if (e.Value == false)
            {
                return;
            }
            StackLayout baba = ((CheckBox)sender).Parent as StackLayout;
            StackLayout yeye = baba.Parent as StackLayout;
            foreach (StackLayout shushu in yeye.Children)
            {
                if (!shushu.Equals(baba))
                {
                    (shushu.Children[0] as CheckBox).IsChecked = false;
                }
            }
            string checkBoxValue = (baba.Children[1] as Entry).Text;
            Console.WriteLine($"{checkBoxValue}");
        }

        private void OnAnswerSelected(object sender, CheckedChangedEventArgs e)
        {
            int current_id = ((ExamQuestions)ExamQuestionsView.CurrentItem).id;
            string answer = (sender as RadioButton).Value.ToString();
            if (e.Value == false)
            {
                Console.WriteLine($"Cancel current_id={current_id} and answer={answer}");
                return;
            }
            _examQuestionsViewModel.OnAnswerClick(current_id, answer);
        }

        private async void OnEnterAnswerCard(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExamAnswersPage(_examQuestionsViewModel));
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
    }
}
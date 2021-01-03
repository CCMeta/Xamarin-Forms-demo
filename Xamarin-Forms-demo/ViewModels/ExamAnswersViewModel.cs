using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ExamAnswersViewModel : BaseViewModel
    {
        private readonly int _exam_id;
        private readonly string path = "/api/exams/{0}/answers";

        public ICommand GetListCommand { protected set; get; }

        public ExamAnswersViewModel(int exam_id) : base()
        {
            _exam_id = exam_id;
        }

        public async Task<bool> PostListAsync(ExamAnswers[] examAnswers)
        {
            var result = await HttpRequest.PostAsync(path, examAnswers.ToArray());
            if (result is ExamAnswers[])
                return true;
            return false;
        }
    }
}
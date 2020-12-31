using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ExamQuestionsPage : ContentPage
    {
        public ExamQuestionsPage()
        {
            InitializeComponent();
        }
        void OnMediaOpened(object sender, EventArgs e)
        {
            Console.WriteLine("Media opened.");
        }

        void OnMediaFailed(object sender, EventArgs e)
        {
            Console.WriteLine("Media failed.");
        }

        void OnMediaEnded(object sender, EventArgs e)
        {
            Console.WriteLine("Media ended.");
        }

        void OnSeekCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Seek completed.");
        }
    }
}
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class SubjectPage : ContentPage
    {
        public SubjectPage(Subjects subject)
        {
            InitializeComponent();
            Title = subject.vname;
            BindingContext = subject;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudyTabbedPage : TabbedPage
    {
        public StudyTabbedPage()
        {
            InitializeComponent();
            //BindingContext = new BaseViewModel();
            //binding current Major  
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = BaseViewModel.major1;
        }
    }
}
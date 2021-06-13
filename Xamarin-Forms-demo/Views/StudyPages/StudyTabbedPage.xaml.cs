using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Xamarin_Forms_demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StudyTabbedPage : TabbedPage
    {
        public StudyTabbedPage()
        {
            InitializeComponent();
            Title = "学习模式";
        }
    }
}
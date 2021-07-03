using System;
using Xamarin.Forms;
using Xamarin_Forms_demo.Services;
using Xamarin_Forms_demo.Views;

namespace Xamarin_Forms_demo
{
    public partial class App : Application
    {
        public App()
        {
            Device.SetFlags(new string[] { "MediaElement_Experimental" });
            InitializeComponent();
            MainPage = new LoginPage();
        }


        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

    }
}

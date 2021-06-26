using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        private readonly Action SetMainPage;

        public LoginPage(Action SetMainPage)
        {
            InitializeComponent();
            this.SetMainPage = SetMainPage;
        }

        private void OnLoginClick(object sender, System.EventArgs e)
        {
            MessageLabel.Text = "LOADING...";
            BaseViewModel.username = "wyy";
            SetMainPage.Invoke();
        }
    }
}
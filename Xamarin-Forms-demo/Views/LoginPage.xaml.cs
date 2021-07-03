using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;
using Xamarin_Forms_demo.Models;
using Microsoft.Extensions.Configuration;
using Xamarin_Forms_demo.Services;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class LoginPage : ContentPage
    {
        private Action SetMainPage;

        public LoginPage()
        {
            InitializeComponent();
            this.SetMainPage = () => Application.Current.MainPage = new MainPage();

            var AppConfiguration = Services.AppConfiguration.GetInstence();
            var httpClient = new HttpRequest(AppConfiguration.GetValue<string>("Host"));
            Task.Run(async () =>
            {
                var users = await httpClient.GetAsync<List<Users>>("/api/token", new Dictionary<string, string>());
                Console.WriteLine(users.Count);
                collectionView.BindingContext = users;
            }).Wait();
        }

        private void OnLoginClick(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;
            collectionView.SelectedItem = null;
            BaseViewModel.username = (e.CurrentSelection[0] as Users).username;

            MessageLabel.Text = "LOADING...";
            collectionView.IsVisible = false;
            Device.BeginInvokeOnMainThread(SetMainPage);
        }

    }
}
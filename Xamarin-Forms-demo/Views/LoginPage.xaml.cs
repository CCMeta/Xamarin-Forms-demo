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
        private readonly Action SetMainPage;

        public LoginPage()
        {
            InitializeComponent();
            SetMainPage = () => Application.Current.MainPage = new MainPage();

            var AppConfiguration = Services.AppConfiguration.GetInstence();
            var httpClient = new HttpRequest(AppConfiguration.GetValue<string>("Host"));
            Task.Run(async () =>
            {
                Console.WriteLine(httpClient._host);
                var users = await httpClient.GetAsync<List<Users>>("/api/token", new Dictionary<string, string>());
                Console.WriteLine(users.Count);
                collectionView.BindingContext = users;
            }).Wait();
        }

        private void OnLoginClick(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;
            MessageLabel.Text = "LOADING...";
            collectionView.IsVisible = false;
            collectionView.SelectedItem = null;
                BaseViewModel.GetInstance().OnLogin(username: (e.CurrentSelection[0] as Users).username);
                Device.BeginInvokeOnMainThread(SetMainPage);
        }

    }
}
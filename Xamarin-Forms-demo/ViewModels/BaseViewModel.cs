using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public static ChatHub _chatHub;
        //public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();
        private readonly static IConfiguration _appConfiguration = Services.AppConfiguration.GetInstence();
        public static IConfiguration AppConfiguration => _appConfiguration;

        private static readonly HttpRequest _httpRequest = new HttpRequest(AppConfiguration.GetValue<string>("Host"));
        public static HttpRequest HttpRequest { get => _httpRequest; }

        private bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        private string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private static Users me;
        public Users Me
        {
            get => me;
            set => SetProperty(ref me, value);
        }
        public static string username;
        public string currentMajor = "Unknown";
        public string CurrentMajor
        {
            get => currentMajor;
            set => SetProperty(ref currentMajor, value);
        }

        private static BaseViewModel _instance;
        public static BaseViewModel GetInstance()
        {
            if (_instance == null)
                _instance = new BaseViewModel();
            return _instance;
        }

        protected BaseViewModel()
        {
            string username = BaseViewModel.username is null ?
                AppConfiguration.GetValue<string>("Identity:Username") : BaseViewModel.username;
            var password = AppConfiguration.GetValue<string>("Identity:Password");
            Login(username, password);
        }

        protected void Login(string username, string password)
        {
            if (!string.IsNullOrEmpty(HttpRequest.Token))
                return;
            var identity = new Users { username = username, password = password };

            var user = Task.Run(async () => await HttpRequest.PostAsync("/api/token", identity)).Result;
            if (string.IsNullOrEmpty(user.username))
                throw new Exception($"No token responsed result = {user}");

            //check success setting token 
            HttpRequest.Token = user.token;
            Me = user;

            //init local message signalr
            _chatHub = new ChatHub(AppConfiguration.GetValue<string>("Host") + "/chathub", Me.token);

            //init contacts
            ContactsViewModel.GetListAsync();
        }

        public void OnToggleMajor(string major)
        {
            CurrentMajor = major;
        }

        //Tookit region begin
        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}

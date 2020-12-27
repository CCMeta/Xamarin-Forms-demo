using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        readonly LibVLC _libVLC;
        private readonly ItemsViewModel _ItemsViewModel = new ItemsViewModel();
        private readonly string[] _libVLCOptions = new string[] {
                "--rtsp-caching=100", " --file-caching=100", "--live-caching=100",
            "--realrtsp-caching=100",  "--network-caching=0",
            "--skip-frames", "--sout-keep", "--sout-all",
            "--drop-late-frames","--rtsp-tcp"};

        public ItemsPage()
        {
            InitializeComponent();

            Core.Initialize();
            _libVLC = new LibVLC(enableDebugLogs: false, _libVLCOptions);
            BindingContext = _ItemsViewModel;
        }
        private void OnPlayStarted(object sender, EventArgs e)
        {
            VlcVideoView.MediaPlayer = new MediaPlayer(_libVLC);
            VlcVideoView.MediaPlayer.Play(new Media(
                _libVLC, _ItemsViewModel.FFPLAY_DEFAULT_SDP_PATH, FromType.FromPath));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }
    }
}
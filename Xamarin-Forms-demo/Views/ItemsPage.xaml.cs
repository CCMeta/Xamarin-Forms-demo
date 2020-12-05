using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;
        readonly LibVLC _libvlc;
        readonly string FULL_SDP_PATH = ItemsViewModel.FULL_SDP_PATH;
        readonly string[] _VLCOptions = new string[] {
                "--rtsp-caching=100", " --file-caching=100", "--live-caching=100",
            "--realrtsp-caching=100",  "--network-caching=0",
            "--skip-frames", "--sout-keep", "--sout-all",
            "--drop-late-frames","--rtsp-tcp"};

        public ItemsPage()
        {
            InitializeComponent();

            Core.Initialize();
            _libvlc = new LibVLC(enableDebugLogs: false, _VLCOptions);
            viewModel = new ItemsViewModel();
        }
        private void OnPlayStarted(object sender, EventArgs e)
        {
            VlcVideoView.MediaPlayer = new MediaPlayer(_libvlc);
            VlcVideoView.MediaPlayer.Play(new Media(_libvlc, FULL_SDP_PATH, FromType.FromPath));
        }

        async void OnItemSelected(object sender, EventArgs args)
        {
            var layout = (BindableObject)sender;
            var item = (Item)layout.BindingContext;
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(item)));
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            //Console.WriteLine("AddItem_Clicked");
            await Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Items.Count == 0)
                viewModel.IsBusy = true;
        }

    }
}
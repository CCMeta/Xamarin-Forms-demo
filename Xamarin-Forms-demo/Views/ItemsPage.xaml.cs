using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
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
        const string VIDEO_URL = "rtsp://202.69.69.180:443/webcast/bshdlive-mobile";//bad
        readonly string FULL_SDP_PATH = ItemsViewModel.FULL_SDP_PATH;
        //const string VIDEO_URL = "rtsp://10.0.2.2:8554/hello";//bad


        public ItemsPage()
        {
            InitializeComponent();

            FULL_SDP_PATH = ItemsViewModel.FULL_SDP_PATH;
            Core.Initialize();
            var options = new string[] { "-vvv", "--sout-keep", "--sout-all", "--rtsp-timeout=300", "--rtp-timeout=300", "--loop", "--rtsp-tcp" };
            options = new string[] { 
                "--rtsp-caching=100", " --file-caching=100", "--live-caching=100",
            "--realrtsp-caching=100",  "--network-caching=0",
            "--skip-frames",
            "--drop-late-frames",};
            _libvlc = new LibVLC(enableDebugLogs: true, options);

            BindingContext = viewModel = new ItemsViewModel();
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            VlcVideoView.MediaPlayer = new MediaPlayer(_libvlc);
            VlcVideoView.MediaPlayer.Play(new Media(_libvlc, FULL_SDP_PATH, FromType.FromPath));
        }
    }
}
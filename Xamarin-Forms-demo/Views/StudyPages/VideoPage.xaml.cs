﻿using LibVLCSharp.Shared;
using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;

using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class VideoPage : ContentPage
    {
        private readonly string _videoUrl;

        public VideoPage(string videoUrl)
        {
            InitializeComponent();
            _videoUrl = videoUrl;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Core.Initialize();
            VlcVideoView.LibVLC = new LibVLC(enableDebugLogs: false);
            using var media = new Media(VlcVideoView.LibVLC, new Uri(_videoUrl));
            VlcVideoView.MediaPlayer = new MediaPlayer(media)
            {
                EnableHardwareDecoding = true,
                Fullscreen = true,
                Scale = 0,
            };
            media.Dispose();
            var result = VlcVideoView.MediaPlayer.Play();
            if (result is false)
                throw new Exception("[ccmeta]VlcVideoView.MediaPlayer.Play() is fucked");
        }

        protected override void OnDisappearing()
        {
            base.OnAppearing();
            VlcVideoView.MediaPlayer.Dispose();
        }

    }
}
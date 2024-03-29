﻿using LibVLCSharp.Shared;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class CanvasPage : ContentPage
    {
        //SKPaint configure
        private readonly SKPaint _brush = new SKPaint
        {
            Style = SKPaintStyle.StrokeAndFill,
            Color = Color.Red.ToSKColor(),
            StrokeWidth = 5
        };
        private static readonly ItemsViewModel itemsViewModel = new ItemsViewModel();

        //VLC configure
        private LibVLC _libVLC;
        private readonly string[] _lib_options = new string[] {
            " --file-caching=100", "--live-caching=100",
            "--network-caching=0", "--skip-frames",
            "--sout-keep", "--sout-all",
            "--drop-late-frames","--rtsp-tcp",
        };

        public CanvasPage()
        {
            InitializeComponent();
        }

        private bool OnPlayStarted()
        {
            VlcVideoView.MediaPlayer = new MediaPlayer(_libVLC);
            if (VlcVideoView.MediaPlayer.IsPlaying == false)
                return VlcVideoView.MediaPlayer.Play(new Media(_libVLC, itemsViewModel.FFPLAY_DEFAULT_SDP_PATH, FromType.FromPath));
            return VlcVideoView.MediaPlayer.IsPlaying;
        }

        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            while (ItemsViewModel.drawPointsQueue.TryDequeue(out List<List<float>> _SKPoints))
            {
                SKPath path = new SKPath();
                path.MoveTo(new SKPoint(_SKPoints[0][0], _SKPoints[0][1]));
                path.QuadTo(new SKPoint(_SKPoints[1][0], _SKPoints[1][1]),
                    new SKPoint(_SKPoints[2][0], _SKPoints[2][1]));
                args.Surface.Canvas.DrawPath(path, _brush);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //libVLC initital
            Core.Initialize();
            _libVLC = new LibVLC(enableDebugLogs: false, _lib_options);

            //binding callbacks
            itemsViewModel.OnDrawCanvas += (object sender, EventArgs e) =>
            {
                //this is the fucking point in ios with fucking exception
                Device.BeginInvokeOnMainThread(canvasView.InvalidateSurface);
            };
            itemsViewModel.OnLocalRtpSession += (object sender, EventArgs e) =>
            {
                if (!OnPlayStarted())
                    throw new Exception("[CCMeta]VlcVideoView.MediaPlayer.Play failed");
            };
        }

    }
}
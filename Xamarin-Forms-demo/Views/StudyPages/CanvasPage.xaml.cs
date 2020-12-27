using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class CanvasPage : ContentPage
    {
        private readonly SKPaint _brush = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.Red.ToSKColor(),
            StrokeWidth = 5
        };
        private static readonly ItemsViewModel itemsViewModel = new ItemsViewModel();

        public CanvasPage()
        {
            InitializeComponent();
            Title = "CanvasPage";
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
            itemsViewModel.DrawCanvasEvent += (object sender, EventArgs e) =>
            {
                canvasView.InvalidateSurface();
            };
        }
    }
}
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Text.Json;

using Xamarin_Forms_demo.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class CanvasPage : ContentPage
    {
        private readonly SKPaint brush = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.Red.ToSKColor(),
            StrokeWidth = 5
        };
        private static readonly ItemsViewModel itemsViewModel = new ItemsViewModel();
        
        public CanvasPage()
        {
            Title = "CanvasPage";
            InitializeComponent();

        }

        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            while (ItemsViewModel.drawPointsQueue.TryDequeue(out List<SKPoint> _SKPoints))
            {
                SKPath path = new SKPath();
                path.MoveTo(_SKPoints[0]);
                path.QuadTo(_SKPoints[1], _SKPoints[2]);
                args.Surface.Canvas.DrawPath(path, brush);
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
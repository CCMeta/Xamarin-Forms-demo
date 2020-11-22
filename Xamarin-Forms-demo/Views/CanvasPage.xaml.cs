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
        ItemsViewModel viewModel;

        public CanvasPage()
        {
            InitializeComponent();
            Title = "Simple Circle";
        }

        //SKCanvas canvas;
        SKPaint brush = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.Red.ToSKColor(),
            StrokeWidth = 10
        };
        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;
            //canvas.Clear();

            if (sKPoints.Count > 0)
            {
                SKPath path = new SKPath();
                path.MoveTo(sKPoints[0]);
                path.QuadTo(sKPoints[1], sKPoints[2]);
                canvas.DrawPath(path, brush);
            }
        }

        List<SKPoint> sKPoints = new List<SKPoint>(3);
        private async Task Drawing(List<List<float>> pointsList, bool isNeedSend = true)
        {
            sKPoints.Clear();
            sKPoints.Add(new SKPoint(pointsList[0][0], pointsList[0][1]));
            sKPoints.Add(new SKPoint(pointsList[1][0], pointsList[1][1]));
            sKPoints.Add(new SKPoint(pointsList[2][0], pointsList[2][1]));
            //Console.WriteLine("FUCK TO CALL ");
            canvasView.InvalidateSurface();
            //SKPath path = new SKPath();
            //path.MoveTo(beginPoint);
            ////path.QuadTo(controlPoint, endPoint);
            //path.LineTo(endPoint);
            //canvas.DrawPath(path, brush);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Button_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string drawPoint = ((Button)sender).Text;
            if (string.IsNullOrEmpty(drawPoint))
                return;
            var pointsList = JsonSerializer.Deserialize<List<List<float>>>(drawPoint);
            //Console.WriteLine("I dont give a fuck " + drawPoint.ToString());
            Task.Run(async () =>
            {
                await Drawing(pointsList, isNeedSend: false);
            });
        }

    }
}
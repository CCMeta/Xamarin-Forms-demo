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

        public CanvasPage()
        {
            Title = "Simple Circle";
            InitializeComponent();
            new ItemsViewModel().drawCanvasEvent += (object sender, EventArgs e) =>
            {
                canvasView.InvalidateSurface();
            };
        }

        //SKCanvas canvas;
        SKPaint brush = new SKPaint
        {
            Style = SKPaintStyle.Stroke,
            Color = Color.Red.ToSKColor(),
            StrokeWidth = 5
        };
        public void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            var canvas = args.Surface.Canvas;
            //canvas.Clear();

            while (ItemsViewModel.drawPointsQueue.TryDequeue(out List<SKPoint> sKPoints))
            {
                counter = counter + 1;
                Console.WriteLine("fuck counter = " + counter);
                SKPath path = new SKPath();
                path.MoveTo(sKPoints[0]);
                path.QuadTo(sKPoints[1], sKPoints[2]);
                canvas.DrawPath(path, brush);
            }
        }


        Queue<List<SKPoint>> numbers = new Queue<List<SKPoint>>();
        //unused
        public void Drawing(List<List<float>> pointsList, bool isNeedSend = true)
        {
            List<SKPoint> sKPoints = new List<SKPoint>(3);
            sKPoints.Add(new SKPoint(pointsList[0][0], pointsList[0][1]));
            sKPoints.Add(new SKPoint(pointsList[1][0], pointsList[1][1]));
            sKPoints.Add(new SKPoint(pointsList[2][0], pointsList[2][1]));
            numbers.Enqueue(sKPoints);
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

        //unused
        static int counter = 0;
        private void Button_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            return;
            counter = counter + 1;
            Console.WriteLine("fuck counter = " + counter);
            string drawPoint = ((Button)sender).Text;
            if (string.IsNullOrEmpty(drawPoint))
                return;
            var pointsList = JsonSerializer.Deserialize<List<List<float>>>(drawPoint);
            //Console.WriteLine("I dont give a fuck " + drawPoint.ToString());

            Drawing(pointsList, isNeedSend: false);
        }

    }
}
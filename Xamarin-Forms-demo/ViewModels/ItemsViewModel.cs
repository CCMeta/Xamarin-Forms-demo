using Microsoft.Extensions.Logging;
using Serilog;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public static ClientWebSocket ClientWebSocket = new ClientWebSocket();
        public static Queue<List<SKPoint>> drawPointsQueue = new Queue<List<SKPoint>>();
        public event EventHandler DrawCanvasEvent;

        public ItemsViewModel() : base()
        {
            AddConsoleLogger();
            var WebSocketService = new WebSocketService();
            Task.Run(() =>
            {
                WebSocketService.ListeningWebSocket();
                WebSocketService.ListeningWebRTCAsync();
            });
        }

        private static void AddConsoleLogger()
        {
            var loggerFactory = new LoggerFactory();
            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug)
                .WriteTo.Debug()
                .CreateLogger();
            loggerFactory.AddSerilog(loggerConfig);
        }
    }
}
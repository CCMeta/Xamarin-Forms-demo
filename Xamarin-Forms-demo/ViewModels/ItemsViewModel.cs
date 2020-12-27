using Microsoft.Extensions.Logging;
using Serilog;
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
        public static Queue<List<List<float>>> drawPointsQueue = new Queue<List<List<float>>>();
        public event EventHandler DrawCanvasEvent;
        public readonly string FFPLAY_DEFAULT_SDP_PATH = WebSocketService.FFPLAY_DEFAULT_SDP_PATH;

        public ItemsViewModel() : base()
        {
            AddConsoleLogger();

            var webSocketService = new WebSocketService();
            Task.Run(async () =>
            {
                await webSocketService.ListeningWebSocketAsync((pointsList) =>
                {
                    drawPointsQueue.Enqueue(pointsList);
                    DrawCanvasEvent(this, EventArgs.Empty);
                });
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
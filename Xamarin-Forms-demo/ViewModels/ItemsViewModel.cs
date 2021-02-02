using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public static StringDictionary Contacts => WebSocketService.contacts;
        public static Queue<List<List<float>>> drawPointsQueue = new Queue<List<List<float>>>();
        public event EventHandler OnDrawCanvas;
        public event EventHandler OnLocalRtpSession;
        public readonly string FFPLAY_DEFAULT_SDP_PATH = WebSocketService.FFPLAY_DEFAULT_SDP_PATH;

        public ItemsViewModel() : base()
        {
            AddConsoleLogger();

            var _ = new WebSocketService((pointsList) =>
            {
                drawPointsQueue.Enqueue(pointsList);
                OnDrawCanvas(this, EventArgs.Empty);
            }, () =>
            {
                OnLocalRtpSession(this, EventArgs.Empty);
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
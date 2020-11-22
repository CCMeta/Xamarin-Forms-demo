using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Views;
using SIPSorcery.Net;
using System.Linq;
using System.IO;
using LibVLCSharp.Shared;
using System.ComponentModel;
using SkiaSharp;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public static ClientWebSocket ClientWebSocket { get; set; }
        public static ArraySegment<byte> response_buffer;
        public static StringDictionary rtc_session = new StringDictionary();
        public static RTCPeerConnection _pc;
        public static RTPSession rtpSession;
        //public static MediaElement mediaElement = new MediaElement();
        public string drawPoints;
        public string DrawPoints
        {
            get => drawPoints;
            set { SetProperty(ref drawPoints, value, "DrawPoints"); }
        }
        public static Queue<List<SKPoint>> drawPointsQueue = new Queue<List<SKPoint>>();

        //public event PropertyChangedEventHandler PropertyChanged;
        //private LibVLC _libVLC;
        //public LibVLC LibVLC
        //{
        //    get => _libVLC;
        //    private set => Set(nameof(LibVLC), ref _libVLC, value);
        //}
        //private MediaPlayer _mediaPlayer;
        //public MediaPlayer MediaPlayer
        //{
        //    get => _mediaPlayer;
        //    private set => Set(nameof(MediaPlayer), ref _mediaPlayer, value);
        //}

        //private void Set<T>(string propertyName, ref T field, T value)
        //{
        //    if (field == null && value != null || field != null && !field.Equals(value))
        //    {
        //        field = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        private const string FFPLAY_DEFAULT_SDP_PATH = "local.sdp";
        private const string FFPLAY_DEFAULT_COMMAND = "ffplay -probesize 32 -protocol_whitelist \"file,rtp,udp\" -i {0}";
        private const int RTP_SESSION_PORT = 5014;
        private const int FFPLAY_DEFAULT_AUDIO_PORT = 5016;
        private const int FFPLAY_DEFAULT_VIDEO_PORT = 5018;
        public static readonly string FULL_SDP_PATH = Xamarin.Essentials.FileSystem.CacheDirectory + "/" + FFPLAY_DEFAULT_SDP_PATH;

        public ItemsViewModel()
        {
            //LibVLCSharp start
            //Core.Initialize();
            //LibVLC = new LibVLC(enableDebugLogs: true);

            //Title = "https://sec.ch9.ms/ch9/5d93/a1eab4bf-3288-4faf-81c4-294402a85d93/XamarinShow_mid.mp4";
            //VideoUri = MediaSource.FromUri(new Uri(Title));

            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            MessagingCenter.Subscribe<NewItemPage, Item>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Item;
                Items.Add(newItem);
                await DataStore.AddItemAsync(newItem);
            });
            AddConsoleLogger();
            FuckServerWebSocket();
            FuckServerWebRTCAsync();
        }

        async void FuckServerWebSocket()
        {
            ClientWebSocket = new ClientWebSocket();
            var uri = new Uri("wss://ccmeta.com:9502/websocket");
            response_buffer = WebSocket.CreateClientBuffer(4096 * 20, 4096 * 20);
            await ClientWebSocket.ConnectAsync(uri, CancellationToken.None);
            while (true)
            {
                WebSocketReceiveResult response = await ClientWebSocket.ReceiveAsync(response_buffer, CancellationToken.None);
                if (response.EndOfMessage)
                {
                    var fucksegment = response_buffer.Array;
                    Array.Resize(ref fucksegment, response.Count);
                    var response_string = System.Text.Encoding.UTF8.GetString(fucksegment);
                    var response_json = JsonSerializer.Deserialize<Hashtable>(response_string);
                    var action_type = response_json["type"].ToString();
                    var action_data = response_json["data"].ToString();
                    switch (action_type)
                    {
                        case "set-sid":
                            Fuck("Switch to set-sid");
                            rtc_session.Add("sid", action_data);
                            break;
                        case "description":
                            Fuck("Switch to description");
                            var response_data_Hashtable = JsonSerializer.Deserialize<Hashtable>(action_data);
                            switch (response_data_Hashtable["type"].ToString())
                            {
                                case "offer":
                                    //setremote
                                    var init = new RTCSessionDescriptionInit()
                                    {
                                        sdp = response_data_Hashtable["sdp"].ToString(),
                                        type = RTCSdpType.offer,
                                    };
                                    for (int i = 0; i < 50; i++)
                                    {
                                    }
                                    _pc.setRemoteDescription(init);

                                    //setLocalDescription
                                    var answer = _pc.createAnswer(new RTCAnswerOptions());
                                    await _pc.setLocalDescription(answer);

                                    //send answer to offerman
                                    string request_json = JsonSerializer.Serialize(new
                                    {
                                        type = "description",
                                        data = new { type = answer.type.ToString(), answer.sdp },
                                    });
                                    await ClientWebSocket.SendAsync(
                                        new ArraySegment<byte>(System.Text.Encoding.ASCII.GetBytes(request_json)),
                                        WebSocketMessageType.Binary, true, CancellationToken.None);
                                    break;
                                case "answer":
                                    _pc.SetRemoteDescription(
                                        SIPSorcery.SIP.App.SdpType.answer,
                                        SDP.ParseSDPDescription(response_data_Hashtable["sdp"].ToString()));
                                    break;
                                default:
                                    Fuck("switch2 enter in default fucked");
                                    break;
                            }
                            break;
                        case "ice-candidate":
                            Fuck("Switch to ice-candidate");
                            var _RTCIceCandidateInit = JsonSerializer.Deserialize<RTCIceCandidateInit>(action_data);
                            _pc.addIceCandidate(_RTCIceCandidateInit);
                            break;
                        default:
                            Fuck($"Switch to in default fucked----{response_string}");
                            break;
                    } //end of switch
                }//end of if
            }//end of while loop
        }

        async void FuckServerWebRTCAsync()
        {
            await CreatePeerConnection();

            //await CrossMediaManager.Current.Play("https://ia800806.us.archive.org/15/items/Mp3Playlist_555/AaronNeville-CrazyLove.mp3");

            //var success = ThreadPool.QueueUserWorkItem(async (callback) =>
            //{
            //    var uri = "https://wenba-ooo-qiniu.xueba100.com/1v1-office-course-video1.mp4?333";
            //    var HttpClient = new HttpClient();
            //    Stream stream = await HttpClient.GetStreamAsync(uri);
            //});


            var audioTrack = new MediaStreamTrack(
                SDPMediaTypesEnum.audio, false,
                new List<SDPMediaFormat> { new SDPMediaFormat(SDPMediaFormatsEnum.PCMA), new SDPMediaFormat(SDPMediaFormatsEnum.OPUS) },
                MediaStreamStatusEnum.SendRecv);
            _pc.addTrack(audioTrack);
            //there is need to try android track 

            _pc.OnRtpPacketReceived += (IPEndPoint, media, rtpPkt) =>
            {
                //Fuck("OnRtpPacketReceived");
                if (!_pc.IsDtlsNegotiationComplete)
                {
                    return;
                }
                //CreateRtpSession
                if (rtpSession == null)
                {
                    Console.WriteLine($"CreateRtpSession.");
                    rtpSession = CreateLocalRtpSession(_pc.AudioRemoteTrack?.Capabilities, _pc.VideoRemoteTrack?.Capabilities);
                }

                if (media == SDPMediaTypesEnum.audio && rtpSession.AudioDestinationEndPoint != null)
                {
                    //Console.WriteLine($"Forwarding {media} RTP packet to ffplay timestamp {rtpPkt.Header.Timestamp}.");
                    rtpSession.SendRtpRaw(media, rtpPkt.Payload, rtpPkt.Header.Timestamp, rtpPkt.Header.MarkerBit, rtpPkt.Header.PayloadType);
                }
                else if (media == SDPMediaTypesEnum.video && rtpSession.VideoDestinationEndPoint != null)
                {
                    //logger.LogDebug($"Forwarding {media} RTP packet to ffplay timestamp {rtpPkt.Header.Timestamp}.");
                    rtpSession.SendRtpRaw(media, rtpPkt.Payload, rtpPkt.Header.Timestamp, rtpPkt.Header.MarkerBit, rtpPkt.Header.PayloadType);
                }
            };
            _pc.OnReceiveReport += (IPEndPoint, SDPMediaTypesEnum, RTPPacket) => { };
            _pc.OnRtpEvent += (IPEndPoint, RTPEvent, RTPHeader) => { };
            _pc.onicecandidate += (_RTCIceCandidate) =>
            {
                //Fuck($"onicecandidate  fuck-me ");
            };
            _pc.onconnectionstatechange += (state) => Fuck($"Peer connection state change to {state}.");
            _pc.onnegotiationneeded += () => Fuck("onnegotiationneeded");
            _pc.onsignalingstatechange += () => Fuck("onsignalingstatechange");
            _pc.onicegatheringstatechange += (state) => Fuck("onicegatheringstatechange" + state);
            _pc.onicecandidateerror += (candidate, info) => Fuck("onicecandidateerror" + candidate + "-----" + info);
            _pc.oniceconnectionstatechange += (state) => Fuck($"ICE connection state change to {state}.");

            _pc.ondatachannel += (dc) =>
            {
                dc.onopen += () => Console.WriteLine($"{_pc}: Data channel now open label {dc.label}, stream ID {dc.id}.");
                dc.onerror += (info) => Console.WriteLine($"{_pc}: Data channel error = {info}");
                dc.onclose += () => Console.WriteLine($"{_pc}: Data channel = close");
                dc.onmessage += (info) =>
                {
                    var receviceData = JsonSerializer.Deserialize<Hashtable>(info);
                    switch (receviceData["type"].ToString())
                    {
                        case "draw-canvas":
                            DrawPoints = receviceData["data"].ToString();
                            var pointsList = JsonSerializer.Deserialize<List<List<float>>>(DrawPoints);
                            List<SKPoint> sKPoints = new List<SKPoint>(3);
                            sKPoints.Add(new SKPoint(pointsList[0][0], pointsList[0][1]));
                            sKPoints.Add(new SKPoint(pointsList[1][0], pointsList[1][1]));
                            sKPoints.Add(new SKPoint(pointsList[2][0], pointsList[2][1]));
                            drawPointsQueue.Enqueue(sKPoints);

                            //Fuck(DrawPoints);
                            break;
                        default:
                            Fuck(info);
                            break;
                    }
                };
            };

            static async Task CreatePeerConnection()
            {
                var ssl_file = await new HttpClient().GetAsync("https://shadow-board.ccmeta.com/ssl.pfx");
                var localhostCert = new X509Certificate2(await ssl_file.Content.ReadAsByteArrayAsync(), "0");
                var presetCertificates = new List<RTCCertificate> {
                new RTCCertificate { Certificate = localhostCert },
            };
                var IceServersStun = new RTCIceServer
                {
                    urls = "stun:137.220.233.101:3333",
                };
                var IceServersTurn = new RTCIceServer
                {
                    urls = "turn:137.220.233.101:3333",
                    username = "username1",
                    credential = "key1",
                };
                var presetIceServers = new List<RTCIceServer> {
                    IceServersStun,
                    IceServersTurn,
                };
                var RTCConfiguration = new RTCConfiguration
                {
                    certificates = presetCertificates,
                    X_BindAddress = IPAddress.Any,
                    iceServers = presetIceServers,
                };
                _pc = new RTCPeerConnection(RTCConfiguration);
            }
        }
        static int counter = 0;
        private RTPSession CreateLocalRtpSession(List<SDPMediaFormat> audioFormats, List<SDPMediaFormat> videoFormats)
        {
            var rtpSession = new RTPSession(false, false, false, IPAddress.Loopback, RTP_SESSION_PORT);
            bool hasAudio = false;
            bool hasVideo = false;

            if (audioFormats != null && audioFormats.Count > 0)
            {
                MediaStreamTrack audioTrack = new MediaStreamTrack(SDPMediaTypesEnum.audio, false, audioFormats, MediaStreamStatusEnum.SendRecv);
                rtpSession.addTrack(audioTrack);
                hasAudio = true;
            }

            if (videoFormats != null && videoFormats.Count > 0)
            {
                MediaStreamTrack videoTrack = new MediaStreamTrack(SDPMediaTypesEnum.video, false, videoFormats, MediaStreamStatusEnum.SendRecv);
                rtpSession.addTrack(videoTrack);
                hasVideo = true;
            }

            var sdpOffer = rtpSession.CreateOffer(null);

            // Because the SDP being written to the file is the input to ffplay the connection ports need to be changed
            // to the ones ffplay will be listening on.
            if (hasAudio)
            {
                sdpOffer.Media.Single(x => x.Media == SDPMediaTypesEnum.audio).Port = FFPLAY_DEFAULT_AUDIO_PORT;
            }

            if (hasVideo)
            {
                sdpOffer.Media.Single(x => x.Media == SDPMediaTypesEnum.video).Port = FFPLAY_DEFAULT_VIDEO_PORT;
            }

            File.WriteAllText(FULL_SDP_PATH, sdpOffer.ToString());
            //Console.WriteLine(string.Format(FFPLAY_DEFAULT_COMMAND, full_sdp_path));

            rtpSession.Start();
            rtpSession.SetDestination(SDPMediaTypesEnum.audio, new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_AUDIO_PORT), new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_AUDIO_PORT + 1));
            rtpSession.SetDestination(SDPMediaTypesEnum.video, new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_VIDEO_PORT), new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_VIDEO_PORT + 1));

            return rtpSession;
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private static void Fuck(string log)
        {
            Console.WriteLine($"[fuck] : {log} \r\n");
        }

        private static void AddConsoleLogger()
        {
            var loggerFactory = new LoggerFactory();
            var loggerConfig = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Is(Serilog.Events.LogEventLevel.Debug)
                //.WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();
            loggerFactory.AddSerilog(loggerConfig);
            //SIPSorcery.Sys.Log.LoggerFactory = loggerFactory;
        }

    }
}
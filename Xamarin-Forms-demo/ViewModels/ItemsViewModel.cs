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
using SkiaSharp;


namespace Xamarin_Forms_demo.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public static ClientWebSocket ClientWebSocket = new ClientWebSocket();
        public static ArraySegment<byte> response_buffer;
        public static StringDictionary rtc_session = new StringDictionary();
        public static RTCPeerConnection _pc;
        public static RTPSession rtpSession;
        public static Queue<List<SKPoint>> drawPointsQueue = new Queue<List<SKPoint>>();
        public event EventHandler DrawCanvasEvent;

        private const string FFPLAY_DEFAULT_SDP_PATH = "local.sdp";
        private const int RTP_SESSION_PORT = 15014;
        private const int FFPLAY_DEFAULT_AUDIO_PORT = 15016;
        private const int FFPLAY_DEFAULT_VIDEO_PORT = 15018;
        public static readonly string FULL_SDP_PATH = Xamarin.Essentials.FileSystem.CacheDirectory + "/" + FFPLAY_DEFAULT_SDP_PATH;

        public ItemsViewModel()
        {
            AddConsoleLogger();
            if (ClientWebSocket.State == WebSocketState.None)
            {
                FuckServerWebSocket();
                FuckServerWebRTCAsync();
            }
        }

        async void FuckServerWebSocket()
        {
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
                            rtc_session.Add("sid", action_data);
                            break;
                        case "description":
                            var response_data_Hashtable = JsonSerializer.Deserialize<Hashtable>(action_data);
                            switch (response_data_Hashtable["type"].ToString())
                            {
                                case "offer":
                                    //setremote
                                    Fuck(response_data_Hashtable["sdp"].ToString());
                                    var remoteSDP = new RTCSessionDescriptionInit()
                                    {
                                        sdp = response_data_Hashtable["sdp"].ToString(),
                                        type = RTCSdpType.offer,
                                    };
                                    _pc.setRemoteDescription(remoteSDP);

                                    //setLocalDescription
                                    var answer = _pc.createAnswer(new RTCAnswerOptions());
                                    await _pc.setLocalDescription(answer);

                                    //send answer to offer client
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
                            Fuck(action_data);
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

            var audioTrack = new MediaStreamTrack(
                SDPMediaTypesEnum.audio, false,
                new List<SDPMediaFormat> {
                    new SDPMediaFormat(SDPMediaFormatsEnum.OPUS) ,
                    new SDPMediaFormat(SDPMediaFormatsEnum.PCMA) ,
                    new SDPMediaFormat(SDPMediaFormatsEnum.PCMU) ,
                },
                MediaStreamStatusEnum.SendRecv);
            _pc.addTrack(audioTrack);

            _pc.OnRtpPacketReceived += (IPEndPoint, media, rtpPkt) =>
            {
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
            }; // End of _pc.OnRtpPacketReceived
            _pc.OnReceiveReport += (IPEndPoint, SDPMediaTypesEnum, RTPPacket) => { };
            _pc.OnRtpEvent += (IPEndPoint, RTPEvent, RTPHeader) => { };
            _pc.onicecandidate += async (candidate) =>
            {
                if (_pc.signalingState == RTCSignalingState.have_local_offer ||
                    _pc.signalingState == RTCSignalingState.have_remote_offer)
                {
                    var localCandidate = new RTCIceCandidateInit()
                    {
                        candidate = "candidate:" + candidate.ToString(),
                        sdpMid = candidate.sdpMid,
                        sdpMLineIndex = candidate.sdpMLineIndex,
                        usernameFragment=candidate.usernameFragment
                    };
                    string request_json = JsonSerializer.Serialize(new
                    {
                        type = "ice-candidate",
                        data = localCandidate
                    });
                    Fuck(request_json);
                    await ClientWebSocket.SendAsync(
                        new ArraySegment<byte>(System.Text.Encoding.ASCII.GetBytes(request_json)),
                        WebSocketMessageType.Binary, true, CancellationToken.None);
                }
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
                            var pointsList = JsonSerializer.Deserialize<List<List<float>>>(receviceData["data"].ToString());
                            List<SKPoint> sKPoints = new List<SKPoint>(3)
                            {
                                new SKPoint(pointsList[0][0], pointsList[0][1]),
                                new SKPoint(pointsList[1][0], pointsList[1][1]),
                                new SKPoint(pointsList[2][0], pointsList[2][1])
                            };
                            drawPointsQueue.Enqueue(sKPoints);
                            DrawCanvasEvent(this, EventArgs.Empty);
                            break;
                        default:
                            Fuck(info);
                            break;
                    }
                };
            }; // end of _pc.ondatachannel

        }

        async Task CreatePeerConnection()
        {
            var ssl_file = await new HttpClient().GetAsync("https://shadow-board.ccmeta.com/ssl.pfx");
            var localhostCert = new X509Certificate2(await ssl_file.Content.ReadAsByteArrayAsync(), "0");
            var presetCertificates = new List<RTCCertificate> {
                    new RTCCertificate { Certificate = localhostCert },
                };
            var IceServersStun = new RTCIceServer
            {
                urls = "stun:137.220.233.101:13333",
            };
            var IceServersTurn = new RTCIceServer
            {
                urls = "turn:137.220.233.101:13333",
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

            rtpSession.Start();
            rtpSession.SetDestination(SDPMediaTypesEnum.audio, new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_AUDIO_PORT), new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_AUDIO_PORT + 1));
            rtpSession.SetDestination(SDPMediaTypesEnum.video, new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_VIDEO_PORT), new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_VIDEO_PORT + 1));

            return rtpSession;
        }

        public static void Fuck(string log)
        {
            Console.WriteLine($"[fuck] : {log} \r\n");
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
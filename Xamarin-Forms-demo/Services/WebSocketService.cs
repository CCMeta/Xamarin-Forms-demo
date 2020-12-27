﻿using SIPSorcery.Net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Services
{
    public class WebSocketService
    {
        public static ClientWebSocket ClientWebSocket = new ClientWebSocket();
        public static StringDictionary sidList = new StringDictionary();
        public static RTCPeerConnection _pc;
        public static RTPSession localRtpSession;

        private const int RTP_SESSION_PORT = 15014;
        private const int FFPLAY_DEFAULT_AUDIO_PORT = 15016;
        private const int FFPLAY_DEFAULT_VIDEO_PORT = 15018;
        private const string WS_URL = "wss://ccmeta.com:9502/websocket";
        public static readonly string FFPLAY_DEFAULT_SDP_PATH = Xamarin.Essentials.FileSystem.CacheDirectory + "/" + "local.sdp";

        public async Task ListeningWebSocketAsync(Action<List<List<float>>> OnDrawCanvas)
        {
            var uri = new Uri(WS_URL);
            var response_buffer = WebSocket.CreateClientBuffer(4096 * 20, 4096 * 20);
            await ClientWebSocket.ConnectAsync(uri, CancellationToken.None);
            await ListeningWebRTCAsync(OnDrawCanvas);
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
                            sidList.Add("sid", action_data);
                            break;
                        case "description":
                            var sdpWithType = JsonSerializer.Deserialize<Hashtable>(action_data);
                            switch (sdpWithType["type"].ToString())
                            {
                                case "offer":
                                    //set remote offer
                                    _pc.setRemoteDescription(new RTCSessionDescriptionInit()
                                    {
                                        sdp = sdpWithType["sdp"].ToString(),
                                        type = RTCSdpType.offer,
                                    });

                                    //setLocalDescription
                                    await _pc.setLocalDescription(_pc.createAnswer(new RTCAnswerOptions()));

                                    //send answer to offer client
                                    string request_json = JsonSerializer.Serialize(new
                                    {
                                        type = "description",
                                        data = new
                                        {
                                            type = _pc.localDescription.type.ToString(),
                                            sdp = _pc.localDescription.sdp.ToString(),
                                        },
                                    });

                                    await ClientWebSocket.SendAsync(
                                        new ArraySegment<byte>(System.Text.Encoding.ASCII.GetBytes(request_json)),
                                        WebSocketMessageType.Binary, true, CancellationToken.None);
                                    break;
                                case "answer":
                                    _pc.setRemoteDescription(new RTCSessionDescriptionInit()
                                    {
                                        sdp = sdpWithType["sdp"].ToString(),
                                        type = RTCSdpType.offer,
                                    });
                                    break;
                                default:
                                    break;
                            }
                            break;
                        case "ice-candidate":
                            _pc.addIceCandidate(JsonSerializer.Deserialize<RTCIceCandidateInit>(action_data));
                            break;
                        default:
                            break;
                    } //end of switch
                }//end of if
            }//end of while loop
        }

        private async Task ListeningWebRTCAsync(Action<List<List<float>>> OnDrawCanvas)
        {
            _pc = await CreatePeerConnectionAsync();

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
                    return;

                //CreateRtpSession
                if (localRtpSession == null)
                {
                    Console.WriteLine($"CreateLocalRtpSession.");
                    localRtpSession = CreateLocalRtpSession(_pc.AudioRemoteTrack?.Capabilities, _pc.VideoRemoteTrack?.Capabilities);
                }

                if (media == SDPMediaTypesEnum.audio && localRtpSession.AudioDestinationEndPoint != null)
                {
                    //Console.WriteLine($"Forwarding {media} RTP packet to ffplay timestamp {rtpPkt.Header.Timestamp}.");
                    localRtpSession.SendRtpRaw(media, rtpPkt.Payload, rtpPkt.Header.Timestamp, rtpPkt.Header.MarkerBit, rtpPkt.Header.PayloadType);
                }
                else if (media == SDPMediaTypesEnum.video && localRtpSession.VideoDestinationEndPoint != null)
                {
                    //logger.LogDebug($"Forwarding {media} RTP packet to ffplay timestamp {rtpPkt.Header.Timestamp}.");
                    localRtpSession.SendRtpRaw(media, rtpPkt.Payload, rtpPkt.Header.Timestamp, rtpPkt.Header.MarkerBit, rtpPkt.Header.PayloadType);
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
                        usernameFragment = candidate.usernameFragment
                    };
                    string request_json = JsonSerializer.Serialize(new
                    {
                        type = "ice-candidate",
                        data = localCandidate
                    });
                    await ClientWebSocket.SendAsync(
                        new ArraySegment<byte>(System.Text.Encoding.ASCII.GetBytes(request_json)),
                        WebSocketMessageType.Binary, true, CancellationToken.None);
                }
            };
            //_pc.onconnectionstatechange += (state) => Fuck($"Peer connection state change to {state}.");
            //_pc.onnegotiationneeded += () => Fuck("onnegotiationneeded");
            //_pc.onsignalingstatechange += () => Fuck("onsignalingstatechange");
            //_pc.onicegatheringstatechange += (state) => Fuck("onicegatheringstatechange" + state);
            //_pc.onicecandidateerror += (candidate, info) => Fuck("onicecandidateerror" + candidate + "-----" + info);
            //_pc.oniceconnectionstatechange += (state) => Fuck($"ICE connection state change to {state}.");
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
                            OnDrawCanvas.Invoke(
                                JsonSerializer.Deserialize<List<List<float>>>(receviceData["data"].ToString()));
                            break;
                        default:
                            break;
                    }
                };
            }; // end of _pc.ondatachannel
        }

        private async Task<RTCPeerConnection> CreatePeerConnectionAsync()
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
            return new RTCPeerConnection(RTCConfiguration);
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

            File.WriteAllText(FFPLAY_DEFAULT_SDP_PATH, sdpOffer.ToString());

            rtpSession.Start();
            rtpSession.SetDestination(SDPMediaTypesEnum.audio, new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_AUDIO_PORT), new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_AUDIO_PORT + 1));
            rtpSession.SetDestination(SDPMediaTypesEnum.video, new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_VIDEO_PORT), new IPEndPoint(IPAddress.Loopback, FFPLAY_DEFAULT_VIDEO_PORT + 1));

            return rtpSession;
        }
    }
}

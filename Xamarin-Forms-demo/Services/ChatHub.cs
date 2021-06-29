using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace Xamarin_Forms_demo.Services
{
    public class ChatHub
    {
        private readonly HubConnection connection;
        private readonly Dictionary<MessageType, Action<string, string>> _mapper;

        public ChatHub(string url, string _myAccessToken)
        {
            connection = new HubConnectionBuilder().WithUrl(url, options =>
            {
                //options.AccessTokenProvider = () => Task.FromResult(_myAccessToken);
                options.Headers.Add("Authorization", _myAccessToken);
            }).Build();
            connection.Closed += OnConnectionClosed();

            //init mapper bind on event 
            _mapper = InitEventMapper();
            foreach (var item in Enum.GetValues(typeof(MessageType)))
            {
                connection.On(item.ToString(), _mapper[(MessageType)item]);
            }

            //go connect
            try
            {
                Task.Run(async () => await connection.StartAsync()).Wait();
                // no need call from client i am whoops.
                //Task.Run(() => SendMessage(MessageType.OnEventOnline, "", "online")); // i am online !
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
        }

        private Dictionary<MessageType, Action<string, string>> InitEventMapper()
        {
            return new Dictionary<MessageType, Action<string, string>>
            {
                { MessageType.OnEventChatSend, OnEventChatSend() },
                { MessageType.OnEventOnline, OnEventOnline() }
            };
        }

        private Func<Exception, Task> OnConnectionClosed()
        {
            return async (error) =>
            {
                await Task.Delay(5 * 1000);
                await connection.StartAsync();
            };
        }

        private Action<string, string> OnEventChatSend()
        {
            return (caller, message) =>
            {
                MessagingCenter.Send(this, MessageType.OnEventChatSend.ToString(), KeyValuePair.Create(caller, message));
            };
        }

        private Action<string, string> OnEventOnline()
        {
            return (caller, message) =>
            {
                MessagingCenter.Send(this, MessageType.OnEventOnline.ToString(), KeyValuePair.Create(caller, message));
            };
        }

        private async void SendMessage(MessageType messageType, string user, string message)
        {
            await connection.InvokeAsync(messageType.ToString(), user, message);
        }
    }

    public enum MessageType
    {
        OnEventOnline,
        OnEventChatSend
    }

}
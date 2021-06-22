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
                Task.Run(() => SendMessage(MessageType.OnEventOnline, "", "online")); // i am online !
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
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private Action<string, string> OnEventChatSend()
        {
            return (user, message) =>
            {
                var newMessage = $"fuck{user}: {message}";
                Console.WriteLine(newMessage);
            };
        }

        private Action<string, string> OnEventOnline()
        {
            return (caller, message) =>
            {
                var newMessage = $"fuck{caller}: {message}";
                // go update caller online state
                MessagingCenter.Send(this, MessageType.OnEventOnline.ToString(), KeyValuePair.Create(caller, message));
                if (message == "online")
                {
                    // this is online not offline
                }
                //Console.WriteLine(newMessage);
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
using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace Xamarin_Forms_demo.Services
{
    public class ChatHub
    {
        private readonly HubConnection connection;

        public ChatHub(string url, string _myAccessToken)
        {
            connection = new HubConnectionBuilder().WithUrl(url, options =>
            {
                options.Headers.Add("Authorization", _myAccessToken);
            }).Build();

            connection.Closed += OnConnectionClosed();

            foreach (var item in Enum.GetValues(typeof(MessageType)))
            {
                connection.On(item.ToString(), OnReceiveMessage());
            }

            try
            {
                Task.Run(async () => await connection.StartAsync()).Wait();
                Task.Run(() => SendMessage(MessageType.OnEventOnline, "", "")); // i am online !
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Func<Exception, Task> OnConnectionClosed()
        {
            return async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
        }

        private Action<string, string> OnReceiveMessage()
        {
            return (user, message) =>
            {
                var newMessage = $"fuck{user}: {message}";
                Console.WriteLine(newMessage);
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
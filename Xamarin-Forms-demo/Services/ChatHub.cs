using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace Xamarin_Forms_demo.Services
{
    public class ChatHub
    {
        HubConnection connection;

        public ChatHub(string url, string _myAccessToken)
        {
            connection = new HubConnectionBuilder().WithUrl(url, options =>
            {
                options.Headers.Add("Authorization", _myAccessToken);
            }).Build();

            connection.Closed += OnConnectionClosed();
            connection.On("ReceiveMessage", OnReceiveMessage());

            try
            {
                Task.Run(async () => await connection.StartAsync()).Wait();
                Task.Run(() => SendMessage());
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

        private static Action<string, string> OnReceiveMessage()
        {
            return (user, message) =>
            {
                Console.WriteLine("ChatHub ReceiveMessage OnConnectedAsync");
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
            };
        }

        private async void SendMessage()
        {
            await connection.InvokeAsync("SendMessage", "content", "content2");
        }
    }
}
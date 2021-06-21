using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;

namespace Xamarin_Forms_demo.Services
{
    public class ChatHub
    {
        HubConnection connection;

        public ChatHub()
        {
            connection = new HubConnectionBuilder().WithUrl("https://xamarin.ccmeta.com/chathub").Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine("ChatHub ReceiveMessage OnConnectedAsync");
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
            });
            connection.On<string, string>("SendMessage", (user, message) =>
            {
                Console.WriteLine("ChatHub SendMessage OnConnectedAsync");
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
            });
            try
            {
                Task.Run(async () => await connection.StartAsync()).Wait();
                Task.Run(() => sendButton_Click());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async void connectButton_Click()
        {
            connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var newMessage = $"{user}: {message}";
                Console.WriteLine(newMessage);
            });
        }

        private async void sendButton_Click()
        {
            await connection.InvokeAsync("SendMessage", "content", "content2");
        }
    }
}
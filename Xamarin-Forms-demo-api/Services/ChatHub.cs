using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Services
{
    public class ChatHub : Hub
    {

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("ChatHubOnConnectedAsync" + Context.User);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public Task SendMessage(string user, string message)
        {
            Console.WriteLine(Context.UserIdentifier);
            Console.WriteLine(Context.User.Identities.Count());
            Console.WriteLine(Context.User.Claims.Count());
            return Clients.All.SendAsync("ReceiveMessage", user, message + $"{Context.UserIdentifier}");
        }

        public Task SendMessageToCaller(string user, string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string user, string message)
        {
            return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);
        }
    }
}

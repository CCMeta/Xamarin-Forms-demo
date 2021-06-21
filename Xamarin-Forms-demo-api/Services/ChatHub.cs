using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin_Forms_demo_api.Models;

namespace Xamarin_Forms_demo_api.Services
{
    public class ChatHub : Hub
    {
        public readonly Dictionary<MessageType, Action<string, string>> _mapper;
        private readonly ContactsRepository _ContactsRepository;

        public ChatHub(ContactsRepository ContactsRepository)
        {
            _ContactsRepository = ContactsRepository;
        }

        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("ChatHubOnConnectedAsync" + Context.User);
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task<Task> OnEventOnline(string user, string message)
        {
            //select users
            var contacts = await _ContactsRepository.GetList(int.Parse(Context.UserIdentifier));
            var users = contacts.Select(i => i.partner_id.ToString()).ToList();
            Console.WriteLine("[fuck] Start OnEventOnline");
            return SendAsync(MessageType.OnEventOnline.ToString(), user, message, users);
        }

        private Task SendAsync(string messageType, string user, string message, List<string> users = null)
        {
            if (users is not null)
            {
                return Clients.Users(users).SendAsync(messageType, user, message + $"UserIdentifier = {Context.UserIdentifier}");
            }
            return Clients.All.SendAsync(messageType, user, message + $"UserIdentifier = {Context.UserIdentifier}");
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

    public enum MessageType
    {
        OnEventOnline,
        OnEventChatSend
    }
}

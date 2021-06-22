using Microsoft.AspNetCore.Authorization;
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
            Console.WriteLine($"[fuck] user:{Context.UserIdentifier} is OnConnectedAsync");
            await await OnEventOnline(Context.UserIdentifier, "online"); // call for contacts i am online
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task<Task> OnEventOnline(string caller, string message)
        {
            //select users
            var contacts = await _ContactsRepository.GetList(int.Parse(caller));
            List<string> users = contacts.Select(i => i.partner_id.ToString()).ToList();
            Console.WriteLine($"[fuck] UserIdentifier={Context.UserIdentifier} is Start OnEventOnline caller={caller} message={message}");
            return SendAsync(MessageType.OnEventOnline.ToString(), caller, message, users);
        }

        public async Task<Task> OnEventChatSend(string caller, string partner, string message)
        {
            //select users
            Console.WriteLine($"[fuck] UserIdentifier={Context.UserIdentifier} is Start OnEventChatSend caller={caller} message={message}");
            var contacts = await _ContactsRepository.GetByPartnerId(int.Parse(caller), int.Parse(partner));
            var users = contacts.Select(i => i.partner_id.ToString()).ToList();
            return SendAsync(MessageType.OnEventChatSend.ToString(), caller, message, users);
        }

        private Task SendAsync(string messageType, string caller, string message, List<string> users = null)
        {
            if (users is not null)
                return Clients.Users(users).SendAsync(messageType, caller, message);
            return Clients.All.SendAsync(messageType, caller, message);
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

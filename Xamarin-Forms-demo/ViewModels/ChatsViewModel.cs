using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ChatsViewModel : BaseViewModel
    {
        private readonly string path = "/api/chats";
        private readonly Contacts _partner;
        public ObservableCollection<Chats> chats = new ObservableCollection<Chats>();
        public ObservableCollection<Chats> Chats
        {
            get { return chats; }
            set
            {
                foreach (var item in value)
                {
                    if (item.uid == _partner.partner_id)
                    {
                        //by other
                        item.isMine = false;
                    }
                    else
                    {
                        // by myself
                        item.isMine = true;
                    }
                    chats.Add(item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public ChatsViewModel(Contacts partner) : base()
        {
            _partner = partner;
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });

            MessagingCenter.Subscribe<ChatHub, KeyValuePair<string, string>>(_chatHub, MessageType.OnEventChatSend.ToString(), (sender, arg) => OnEventChatSendhandler(arg.Key, arg.Value));
        }

        public async void GetListAsync()
        {
            var max_id = Chats.LastOrDefault() is null ? 0 : Chats.LastOrDefault().id;
            var queryParams = new Dictionary<string, string>() {
                { "partner_id", _partner.partner_id.ToString() },
                { "max_id", max_id.ToString() },
            };
            using var _ = HttpRequest.GetAsync<ObservableCollection<Chats>>(path, queryParams: queryParams);
            Chats = await _;
            IsBusy = false;
        }

        public async Task<bool> PostAsync(int partner_id, string content)
        {
            Chats queryParams = new Chats
            {
                content = content,
                partner_id = partner_id
            };
            Chats result = await HttpRequest.PostAsync(path, queryParams);
            if (result is Chats)
            {
                result.isMine = true;
                chats.Add(result);
                return true;
            }
            return false;
        }

        private void OnEventChatSendhandler(string caller, string message)
        {
            //go update this partner chat log;
            GetListAsync();
        }
    }
}
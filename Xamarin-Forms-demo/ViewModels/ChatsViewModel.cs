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
        private static readonly string path = "/api/chats";
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

            MessagingCenter.Subscribe<ChatHub, KeyValuePair<string, string>>(_chatHub, MessageType.OnEventChatSend.ToString(), (sender, arg) => OnEventChatSendHandler(arg.Key, arg.Value));
        }

        public async void GetListAsync()
        {
            //this get is use local db
            var max_id = Chats.LastOrDefault() is null ? 0 : Chats.LastOrDefault().id;
            var queryParams = new Dictionary<string, string>() {
                { "partner_id", _partner.partner_id.ToString() },
                { "max_id", max_id.ToString() },
            };
            using var _ = HttpRequest.GetAsync<ObservableCollection<Chats>>(path, queryParams: queryParams);
            Chats = await _;
            IsBusy = false;
        }

        public static async void GetListRemoteAsync(int partner)
        {
            //this get is use remote api
            var db = new ChatsStore();
            var chats = await db.ListAsync(partner);
            var max_id = chats.LastOrDefault() is null ? 0 : chats.LastOrDefault().id;
            var queryParams = new Dictionary<string, string>() {
                { "partner_id", partner.ToString() },
                { "max_id", max_id.ToString() },
            };
            var result = await HttpRequest.GetAsync<List<Chats>>(path, queryParams: queryParams);
            foreach (var i in result)
            {
                await db.SaveAsync(i);
            }
        }

        public static async void GetUnreadCountGroup()
        {
            var db = new ChatsStore();
            var unread_list = await db.ListUnreadAsync();
            unread_list.GroupBy(i=>i.partner_id);

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

        private async void OnEventChatSendHandler(string caller, string message)
        {
            var contact = ContactsViewModel.Contacts.FirstOrDefault(i => i.partner_id == int.Parse(caller));
            if (contact is null)
                return;
            await new ChatSessionsStore().SaveAsync(contact);
            //go update this partner chat log;


            GetListAsync();
        }
    }
}
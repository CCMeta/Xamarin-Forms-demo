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

        public ChatsViewModel(Contacts partner)
        {
            _partner = partner;
            GetListCommand = new Command(async () =>
            {
                await GetListAsync();
            });

            //This guy is not ready when this class is not build this is wrong
            MessagingCenter.Subscribe<ChatHub, KeyValuePair<string, string>>(_chatHub, MessageType.OnEventChatSend.ToString(), (sender, arg) => OnEventChatSendHandler(arg.Key, arg.Value));
        }

        public async Task GetListAsync()
        {
            var db = new ChatsStore(uid: GetInstance().Me.id);
            List<Chats> chats = await db.ListAsync(_partner.partner_id);
            Console.WriteLine(chats.Count);
            Chats = new ObservableCollection<Chats>(chats);
        }

        public static async Task GetListRemoteAsync(int partner)
        {
            //this get is use remote api
            var db = new ChatsStore(uid: GetInstance().Me.id);
            var chats = await db.ListAsync(partner);
            var max_id = chats.LastOrDefault() is null ? 0 : chats.LastOrDefault().id;
            var queryParams = new Dictionary<string, string>() {
                { "partner_id", partner.ToString() },
                { "max_id", max_id.ToString() },
            };
            var result = await HttpRequest.GetAsync<List<Chats>>(path, queryParams: queryParams);
            foreach (var i in result)
            {
                var _ = await db.SaveAsync(i);
                Console.WriteLine($"[fuck]SaveAsync {i.content}");
            }
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

        public void OnEventChatSendHandler(string caller, string message)
        {
            if (int.Parse(caller) == _partner.partner_id)
            {//current user
                Task.Run(async () =>
                {
                    await GetListRemoteAsync(_partner.partner_id);
                    await GetListAsync();
                    //从本地数据库 读取的内容标记已读
                });
            }
        }
    }
}
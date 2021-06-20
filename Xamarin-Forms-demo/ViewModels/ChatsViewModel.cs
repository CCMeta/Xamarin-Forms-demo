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
                    if (item.partner_id == _partner.partner_id)
                    {
                        //by myself
                        item.isMine = true;
                    }
                    else
                    {
                        // by other
                        item.isMine = false;
                    }
                    chats.Add(item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public ChatsViewModel(Contacts partner) : base()
        {
            Title = "聊天会话";
            _partner = partner;
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            var queryParams = new Dictionary<string, string>() { { "partner_id", _partner.partner_id.ToString() } };
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
            var result = await HttpRequest.PostAsync(path, queryParams);
            if (result is Chats)
                return true;
            return false;
        }

    }
}
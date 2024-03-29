﻿using System;
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
    public class ContactsViewModel : BaseViewModel
    {
        private static readonly string path = "/api/contacts";
        public static ObservableCollection<Contacts> contacts = new ObservableCollection<Contacts>();
        public static ObservableCollection<Contacts> Contacts
        {
            get => contacts;
            set
            {
                foreach (var item in value)
                {
                    item.state = "offline";
                    contacts.Add(item);
                }
            }
        }
        public static ICommand GetListCommand { protected set; get; }

        static ContactsViewModel()
        {
            //MessagingCenter.Subscribe<ChatHub, KeyValuePair<string, string>>(_chatHub, MessageType.OnEventChatSend.ToString(),
                //(sender, arg) => GetListAsync());

            //MessagingCenter.Subscribe<ChatHub, KeyValuePair<string, string>>(_chatHub, MessageType.OnEventOnline.ToString(),
                //(sender, arg) => OnEventOnlinehandler(arg.Key, arg.Value));
            //if OnEventChatSend and user at chat GUI,then should fresh
        }

        private static void OnEventOnlinehandler(string caller, string message)
        {
            var item = Contacts.FirstOrDefault(i => i.partner_id == int.Parse(caller));
            var index = Contacts.IndexOf(item);
            item.state = message;
            Contacts[index] = item;
        }

        public static async void GetListAsync()
        {
            var queryParams = new Dictionary<string, string>() { };
            var result = await HttpRequest.GetAsync<ObservableCollection<Contacts>>(path, queryParams: queryParams);
            // getallchats of remote and 写到本地数据库
            // 在本地数据库统计所有未读的信息 写到badge上
            contacts.Clear();
            Contacts = result;
        }

        public static async Task<bool> PostAsync(int partner_id)
        {
            Contacts queryParams = new Contacts
            {
                partner_id = partner_id
            };
            var result = await HttpRequest.PostAsync(path, queryParams);
            if (result is Contacts)
                return true;
            return false;
        }
    }
}
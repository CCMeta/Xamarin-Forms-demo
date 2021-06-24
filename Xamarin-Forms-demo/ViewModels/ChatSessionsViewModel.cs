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
    public class ChatSessionsViewModel : BaseViewModel
    {
        private ObservableCollection<ChatSessions> chatSessions;
        public ObservableCollection<ChatSessions> ChatSessions
        {
            get => chatSessions;
            set
            {
                SetProperty(ref chatSessions, value);//this is big fucker
            }
        }

        private static readonly ChatSessionsStore chatSessionsStore = new ChatSessionsStore();

        public ChatSessionsViewModel()
        {
        }

        public async void GetListAsync()
        {
            List<ChatSessions> sessions = await chatSessionsStore.ListAsync();
            ChatSessions = new ObservableCollection<ChatSessions>(sessions);
        }

        public async void Update(int session_id)
        {
            var item = ChatSessions.FirstOrDefault(i => i.ID == session_id);
            item.Unread = 0;
            await chatSessionsStore.SaveAsync(item);
        }
    }
}
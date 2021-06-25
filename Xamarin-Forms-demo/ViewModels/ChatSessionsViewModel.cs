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
        //private ObservableCollection<ChatSessions> chatSessions;
        //public ObservableCollection<ChatSessions> ChatSessions
        //{
        //    get => chatSessions;
        //    set
        //    {
        //        SetProperty(ref chatSessions, value);//this is big fucker
        //    }
        //}

        //private static readonly ChatSessionsStore chatSessionsStore = new ChatSessionsStore();

        //public ChatSessionsViewModel()
        //{
        //    SetAsync(5, 2);
        //}

        //public async void GetListAsync()
        //{
        //    //List<ChatSessions> sessions = await chatSessionsStore.ListAsync();
        //    ////left join contacts info
        //    //var contacts = ContactsViewModel.Contacts.ToList();
        //    //if (sessions.Count == 0 || contacts.Count == 0)
        //    //    return;
        //    ////var sessions_with_contact =
        //    ////    from session in sessions
        //    ////    join contact in contacts on session.Partner equals contact.partner_id
        //    ////    select new { };
        //    //var sessions_with_contact = contacts.Join(sessions,
        //    //    session => session.partner_id,
        //    //    contact => contact.Partner,
        //    //    (x, y) => y);
        //    //Console.WriteLine(sessions_with_contact.Count());
        //    //foreach (var i in sessions_with_contact)
        //    //{
        //    //    //Console.WriteLine($"\"{i.Partner}\" is owned by {i}");
        //    //}
        //    //ChatSessions = new ObservableCollection<ChatSessions>(sessions_with_contact);
        //}

        //public async void Update(int session_id)
        //{
        //    var item = ChatSessions.FirstOrDefault(i => i.session_id == session_id);
        //    item.Unread = 0;
        //    await chatSessionsStore.SaveAsync(item);
        //}

        //public async void SetAsync(int partner, int unread = 0)
        //{
        //    var result = await chatSessionsStore.GetAsync(partner);
        //    if (result is null)
        //    {
        //        var contact = ContactsViewModel.Contacts.FirstOrDefault(i => i.partner_id == partner);
        //        var chatsession = new ChatSessions()
        //        {
        //            Unread = unread,
        //            Partner = partner,
        //            contact.
        //        };

        //        await chatSessionsStore.SaveAsync(new ChatSessions { Partner = partner, Unread = unread });
        //    }
        //    else
        //    {
        //        result.Unread = unread;
        //        await chatSessionsStore.SaveAsync(result);
        //    }
        //}
    }
}
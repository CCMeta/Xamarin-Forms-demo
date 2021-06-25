using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin_Forms_demo.Models
{
    public class ChatSessions : Contacts
    {
        [PrimaryKey, AutoIncrement]
        public int session_id { get; set; }
        public int Partner { get; set; }
        public int Unread { get; set; }
    }
}

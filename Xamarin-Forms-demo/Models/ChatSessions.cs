using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Xamarin_Forms_demo.Models
{
    public class ChatSessions
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Models
{
    public class Chats
    {
        [PrimaryKey, AutoIncrement]
        public int local_id { get; set; }
        public int id { get; set; }
        public int uid { get; set; }
        public int partner_id { get; set; }
        public int isRead { get; set; }
        public string created_at { get; set; }
        public string content { get; set; }
        public string avatar { get; set; }
        public bool isMine { get; set; }
        public bool isPartner { get => !isMine; }
    }
}

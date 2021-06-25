using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo.Models
{
    public class Contacts
    {
        public int id { get; set; }
        public int uid { get; set; }
        public int partner_id { get; set; }
        public string created_at { get; set; }
        public string mark { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public string intro { get; set; }
        public string state { get; set; }
        public int unread { get; set; }
    }
}

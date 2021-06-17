using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class Contacts
    {
        public int id { get; set; }
        public int uid { get; set; }
        public int partner_id { get; set; }
        public string created_at { get; set; }
        public string mark { get; set; }
    }
}

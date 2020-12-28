using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class Courses
    {
        public int id { get; set; }
        public string title { get; set; }
        public string created_at { get; set; }
        public string speaker { get; set; }
        public string image { get; set; }
        public string knowledge { get; set; }
        public string major { get; set; }
        public string summary { get; set; }
        public int grade { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Xamarin_Forms_demo_api.Models
{
    public class Posts
    {
        public long id { get; set; }
        public string Content { get; set; }
        public string Created_at { get; set; }
        public int Uid { get; set; }

    }
}

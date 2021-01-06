using System;

namespace Xamarin_Forms_demo.Models
{
    public class Subjects
    {
        public int id { get; set; }
        public string img { get; set; }

        public Uri ImgUri
        {
            get => new Uri("https:" + img);
        }
        public string info { get; set; }
        public string score { get; set; }
        public string vtype { get; set; }
        public string summary { get; set; }
        public string vname { get; set; }

    }
}

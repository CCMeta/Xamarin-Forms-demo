using System;

namespace Xamarin_Forms_demo.Models
{
    public class Subjects
    {
        public long id { get; set; }
        public string img { get; set; }
        public Uri Img
        {
            //get => new Uri("https://imgs.gamersky.com/ku/2019/ku_persona5r.jpg");
            get => new Uri("https:" + img);
        }
        public string info { get; set; }
        public string score { get; set; }
        public string vtype { get; set; }
        public string summary { get; set; }
        public string vname { get; set; }

    }
}

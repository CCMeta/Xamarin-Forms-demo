
namespace Xamarin_Forms_demo.Models
{
    public class Posts
    {
        public int id { get; set; }
        public string content { get; set; }
        public string created_at { get; set; }
        public int uid { get; set; }
        public string nickname { get; set; }
        public string avatar { get; set; }
        public string FollowState { get; set; } = "+ Follow";
    }
}

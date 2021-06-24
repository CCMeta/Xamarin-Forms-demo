using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class PostsViewModel : BaseViewModel
    {

        private readonly string path = "/api/posts";
        public List<Posts> posts = new List<Posts>();
        public List<Posts> Posts
        {
            get => posts;
            set
            {
                value.AddRange(posts);
                SetProperty(ref posts, value);
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public PostsViewModel() : base()
        {
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
            var _ = new ChatSessionsStore();
        }

        public async void GetListAsync()
        {
            int maxId = Posts.Count > 0 ? Posts[0].id : 0;
            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            Posts = await HttpRequest.GetAsync<List<Posts>>(path, queryParams: queryParams);
            IsBusy = false;
        }

        public async Task<bool> PostAsync(string content)
        {
            var queryParams = new Posts();
            queryParams.content = content;
            var result = await HttpRequest.PostAsync(path, queryParams);
            if (result is Posts)
                return true;
            return false;
        }

    }
}
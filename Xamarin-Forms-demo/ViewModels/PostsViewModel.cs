using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;
using MvvmHelpers;

namespace Xamarin_Forms_demo.ViewModels
{
    public class PostsViewModel : BaseViewModel
    {

        private readonly string path = "/api/posts";
        public ObservableRangeCollection<Posts> posts = new ObservableRangeCollection<Posts>();
        public ObservableRangeCollection<Posts> Posts
        {
            get { return posts; }
            set
            {
                value.AddRange(posts);
                posts.ReplaceRange(value);
            }
        }

        public ICommand GetListCommand { protected set; get; }

        public PostsViewModel() : base()
        {
            GetListCommand = new Command(() =>
            {
                Task.Run(async () => await GetListAsync());
            });
            //var _ = new ChatSessionsStore();
        }

        public async Task GetListAsync()
        {
            //int maxId = Posts.Count > 0 ? Posts[0].id : 0;
            int maxId = Posts.Count > 0 ? posts[0].id : 0;

            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            Posts = await HttpRequest.GetAsync<ObservableRangeCollection<Posts>>(path, queryParams: queryParams);
            //posts.MyPushRange(result);
            IsBusy = false;
        }

        public async Task<bool> PostAsync(string content)
        {
            Posts queryParams = new Posts
            {
                content = content
            };
            var result = await HttpRequest.PostAsync(path, queryParams);
            if (result is Posts)
                return true;
            return false;
        }

    }
}
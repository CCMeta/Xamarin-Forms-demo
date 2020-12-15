using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;

namespace Xamarin_Forms_demo.ViewModels
{
    public class PostsViewModel : BaseViewModel
    {

        private readonly string path = "/api/posts";
        public ObservableCollection<Posts> posts = new ObservableCollection<Posts>();
        public ObservableCollection<Posts> Posts
        {
            get { return posts; }
            set
            {
                foreach (var item in value)
                {
                    posts.Insert(0, item);
                }
                IsBusy = false;
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public PostsViewModel() : base()
        {
            Title = "PostsViewModel";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
            GetListAsync();
        }

        public async void GetListAsync()
        {
            var page = Math.Ceiling((double)(Posts.Count() + 1) / 5).ToString();
            var queryParams = new Dictionary<string, string>() {
                    { "p", page }
            };
            Posts = await HttpRequest.GetAsync<ObservableCollection<Posts>>(path, queryParams: queryParams);
        }
    }
}
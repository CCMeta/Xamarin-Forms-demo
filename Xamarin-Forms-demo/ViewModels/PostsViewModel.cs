﻿using MvvmHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;
using System;

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
                //value.AddRange(posts);
                posts.ReplaceRange(value);
            }
        }

        public ICommand GetListCommand { protected set; get; }

        public PostsViewModel()
        {
            GetListCommand = new Command(() =>
            {
                Task.Run(async () => await GetListAsync());
            });
            //var _ = new ChatSessionsStore();
        }

        public void OnFollowStateChange(int uid, string act = "Following")
        {
            var result = posts.Select(i =>
            {
                if (i.uid == uid)
                    i.FollowState = act;
                return i;
            });
            Posts = new ObservableRangeCollection<Posts>(result);
        }

        public async Task GetListAsync()
        {
            //int maxId = Posts.Count > 0 ? Posts[0].id : 0嗡嗡嗡
            int maxId = Posts.Count > 0 ? posts[0].id : 0;

            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            var result = await HttpRequest.GetAsync<ObservableRangeCollection<Posts>>(path, queryParams: queryParams);
            if (result.Count > 0)
            {
                //find followed and mark
                result.Where(i => ContactsViewModel.Contacts.Select(i => i.partner_id).ToList().Contains(i.uid))
                    .Select(i => { i.FollowState = "Following"; return i; }).ToList();
                result.AddRange(posts);
                posts.ReplaceRange(result);
            }
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
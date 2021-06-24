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

namespace Xamarin_Forms_demo.ViewModels
{
    public class PostsViewModel : BaseViewModel
    {

        private readonly string path = "/api/posts";
        public MyStack<Posts> posts = new MyStack<Posts>();
        public MyStack<Posts> Posts
        {
            get { return posts; }
            set
            {
                //var fuck = posts;
                foreach (var item in value)
                {
                    //posts.Add(item);// 
                    posts.MyPush(item);
                }
                //SetProperty(ref posts, value);

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
            //int maxId = Posts.Count > 0 ? Posts[0].id : 0;
            int maxId = Posts.Count > 0 ? posts.Peek().id : 0;

            var queryParams = new Dictionary<string, string>() {
                    { "p",maxId.ToString() }
            };
            Posts = await HttpRequest.GetAsync<MyStack<Posts>>(path, queryParams: queryParams);
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

    public class MyStack<T> : Stack<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private int _blockReentrancyCount;
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void OnCountPropertyChanged() => OnPropertyChanged(EventArgsCache.CountPropertyChanged);
        private void OnIndexerPropertyChanged() => OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        private void OnCollectionChanged(NotifyCollectionChangedAction action, object? q)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(action, q));
        }
        public void MyPush(T q)
        {
            base.Push(q);
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(T)));

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, q);
        }
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedEventHandler? handler = CollectionChanged;
            if (handler != null)
            {
                // Not calling BlockReentrancy() here to avoid the SimpleMonitor allocation.
                _blockReentrancyCount++;
                try
                {
                    handler(this, e);
                }
                finally
                {
                    _blockReentrancyCount--;
                }
            }
        }
    }
    internal static class EventArgsCache
    {
        internal static readonly PropertyChangedEventArgs CountPropertyChanged = new PropertyChangedEventArgs("Count");
        internal static readonly PropertyChangedEventArgs IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");
        internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}
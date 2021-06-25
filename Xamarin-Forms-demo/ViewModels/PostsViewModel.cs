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
        public void MyPushRange(IEnumerable<T> range)
        {
            foreach (var i in range)
            {
                base.Push(i);
            }
            //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(T)));

            OnCountPropertyChanged();
            OnIndexerPropertyChanged();
            OnCollectionChanged(NotifyCollectionChangedAction.Add, range);
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
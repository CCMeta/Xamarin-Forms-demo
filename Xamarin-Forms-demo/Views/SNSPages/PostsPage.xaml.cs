using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using Xamarin_Forms_demo.ViewModels;
using System.Collections.ObjectModel;
using Xamarin_Forms_demo.Models;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class PostsPage : ContentPage
    {
        private readonly PostsViewModel _postsViewModel;

        public PostsPage()
        {
            InitializeComponent();
            BindingContext = _postsViewModel = new PostsViewModel();
            //collectionView.RemainingItemsThresholdDirection;
            //linearLayoutManager
            //LinearItemsLayout.Vertical.
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            _postsViewModel.GetListAsync();
        }

        async void OnPushPost(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new SendPostPage());
            _postsViewModel.GetListAsync();
        }
        private void OnTypeButtonToggle(object sender, EventArgs e)
        {
            //make all box to transparent.
            BoxView boxView;
            foreach (var child in listTabNavbar.Children)
            {
                boxView = ((StackLayout)child).Children[1] as BoxView;
                boxView.Color = Color.White;
            }

            //this is a important thing to get a element in a event just remeber the |as| act
            boxView = (((Button)sender).Parent as StackLayout).Children[1] as BoxView;
            Console.WriteLine(boxView.ClassId);
            boxView.Color = Color.FromHex("#00cccc");
        }
    }

    public class Fuck : CollectionView
    {

        public new IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, ((System.Collections.Generic.IEnumerable<Posts>)value).Reverse());
        }
    }
}
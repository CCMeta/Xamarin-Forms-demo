using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsBusy = true;
            _postsViewModel.GetListAsync();
        }

        async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SendPostPage());
        }
    }
}
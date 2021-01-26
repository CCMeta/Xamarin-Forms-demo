using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class SendPostPage : ContentPage
    {
        private readonly PostsViewModel _postsViewModel;
        public SendPostPage()
        {
            InitializeComponent();
            BindingContext = _postsViewModel = new PostsViewModel();
        }

        async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            var result = await _postsViewModel.PostAsync(PostEditor.Text);
            if (result)
                await DisplayAlert("Success", "You have been Success", "OK");
            await Navigation.PopAsync();
        }
    }
}
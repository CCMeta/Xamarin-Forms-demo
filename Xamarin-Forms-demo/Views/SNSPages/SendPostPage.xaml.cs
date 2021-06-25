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
            //var _ = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height;
            //PostEditor.HeightRequest = Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height / 2;
        }

        async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            if (PostEditor.Text.Length == 0)
                return;
            var result = await _postsViewModel.PostAsync(PostEditor.Text);
            if (result)
                await DisplayAlert("提交", "提交動態成功", "確定");
            await Navigation.PopAsync();
        }
    }
}
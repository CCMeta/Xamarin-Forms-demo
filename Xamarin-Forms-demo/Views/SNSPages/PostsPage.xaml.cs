using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class PostsPage : ContentPage
    {
        public PostsPage()
        {
            InitializeComponent();
        }
        void OnMediaOpened(object sender, EventArgs e)
        {
            Console.WriteLine("Media opened.");
        }

        void OnMediaFailed(object sender, EventArgs e)
        {
            Console.WriteLine("Media failed.");
        }

        void OnMediaEnded(object sender, EventArgs e)
        {
            Console.WriteLine("Media ended.");
        }

        void OnSeekCompleted(object sender, EventArgs e)
        {
            Console.WriteLine("Seek completed.");
        }

        async void ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SendPostPage());
        }
    }
}
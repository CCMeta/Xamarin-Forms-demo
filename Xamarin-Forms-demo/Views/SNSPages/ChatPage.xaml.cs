using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ChatPage : ContentPage
    {
        public ChatPage()
        {
            InitializeComponent();
            Title = "梁子";
        }

        private async void OnEnterContactsAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactsPage());
        }
    }
}
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ChatsPage : ContentPage
    {
        public ChatsPage()
        {
            InitializeComponent();
        }

        private async void OnEnterContactsAsync(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ChatPage(partner_id: 2));
            await Navigation.PushAsync(new ContactsPage());
        }
    }
}
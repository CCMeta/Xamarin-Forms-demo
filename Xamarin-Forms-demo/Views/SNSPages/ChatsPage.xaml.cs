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

        private async Task OnEnterContactsAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ContactsPage());
        }
    }
}
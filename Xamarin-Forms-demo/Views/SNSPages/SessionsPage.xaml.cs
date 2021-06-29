using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class SessionsPage : ContentPage
    {
        //private readonly ChatSessionsViewModel _chatSessionsViewModel;

        public SessionsPage()
        {
            InitializeComponent();
            collectionView.BindingContext = ContactsViewModel.Contacts;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //select all in database chats and find unread items grouping
        }

        private async void OnEnterContactsAsync(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ChatPage(partner_id: 2));
            await Navigation.PushAsync(new ContactsPage());
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;
            var partner = e.CurrentSelection[0] as Contacts;
            (sender as CollectionView).SelectedItem = null;
            await Navigation.PushAsync(new ChatPage(partner));
        }
    }
}
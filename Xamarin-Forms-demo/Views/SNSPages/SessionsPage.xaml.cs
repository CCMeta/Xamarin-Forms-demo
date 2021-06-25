using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class SessionsPage : ContentPage
    {
        private readonly ChatSessionsViewModel _chatSessionsViewModel;

        public SessionsPage()
        {
            InitializeComponent();
            BindingContext = _chatSessionsViewModel = new ChatSessionsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _chatSessionsViewModel.GetListAsync();
        }

        private async void OnEnterContactsAsync(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new ChatPage(partner_id: 2));
            await Navigation.PushAsync(new ContactsPage());
        }

        private async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            //var partner = e.CurrentSelection[0] as Contacts;
            //(sender as CollectionView).SelectedItem = null;
            //await Navigation.PushAsync(new ChatPage(partner));
        }
    }
}
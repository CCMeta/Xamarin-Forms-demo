using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class ContactsPage : ContentPage
    {
        //private readonly ContactsViewModel _contactsViewModel;

        public ContactsPage()
        {
            InitializeComponent();
            collectionView.BindingContext = ContactsViewModel.Contacts;
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

        private async void OnContactsSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.Count == 0)
                return;
            var partner = e.CurrentSelection[0] as Contacts;
            (sender as CollectionView).SelectedItem = null;
            await Navigation.PushAsync(new ChatPage(partner));
        }
    }
}
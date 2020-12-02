using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubjectsPage : ContentPage
    {
        SubjectsViewModel subjectsViewModel;
        public ObservableCollection<Subjects> subjects { get; set; }

        public SubjectsPage()
        {
            GetSubjectsAsync();
            InitializeComponent();
        }

        public async void GetSubjectsAsync()
        {
            subjectsViewModel = new SubjectsViewModel();
            subjects = await subjectsViewModel.GetSubjectsAsync();
            Console.WriteLine("[fuck]"+ subjects.Count);
            CollectionView.ItemsSource = subjects;
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}

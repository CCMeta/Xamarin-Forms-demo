using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public ObservableCollection<Subjects> Subjects { get; set; }

        public SubjectsPage()
        {
            InitializeComponent();
            GetSubjectsAsync();
        }

        public async void GetSubjectsAsync()
        {
            subjectsViewModel = new SubjectsViewModel();
            Subjects = await subjectsViewModel.GetSubjectsAsync();
            CollectionView.ItemsSource = Subjects;
        }

        async void OnSelectionItemChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = e.CurrentSelection.FirstOrDefault() as Subjects;
            await Navigation.PushModalAsync(new NavigationPage(new SubjectPage(selected)));
            //await DisplayAlert($"{selected.vname}", $"{selected.info}", "OK");
        }
    }
}

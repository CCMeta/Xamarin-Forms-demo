﻿using System;
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

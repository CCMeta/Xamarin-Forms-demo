using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.ViewModels;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        private List<FlyoutPageItem> flyoutPageItems { get; set; } = new List<FlyoutPageItem>();

        public MenuPage()
        {
            InitializeComponent();
            BindingContext = new BaseViewModel().Me;

            foreach (var item in Enum.GetValues(typeof(MenuItemType)))
            {
                flyoutPageItems.Add(new FlyoutPageItem { Id = Convert.ToInt32(item), Title = item.ToString(), IconSource = "" });
            }

            //ListViewMenu.ItemsSource = flyoutPageItems;
            //ListViewMenu.ItemSelected += async (sender, e) =>
            //{
            //    if (e.SelectedItem == null)
            //        return;
            //    var id = ((FlyoutPageItem)e.SelectedItem).Id;
            //    await RootPage.NavigateFromMenu(id);
            //};
        }
    }
}
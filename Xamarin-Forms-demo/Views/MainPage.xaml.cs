using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

using Xamarin_Forms_demo.Models;

namespace Xamarin_Forms_demo.Views
{
    [DesignTimeVisible(false)]
    public partial class MainPage : FlyoutPage
    {
        Dictionary<MenuItemType, NavigationPage> MenuPages = new Dictionary<MenuItemType, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
            MenuPages.Add(MenuItemType.Index, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(MenuItemType id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                MenuPages.Add(id, new NavigationPage(FlyoutPageItem.GetPageById(id)));
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}
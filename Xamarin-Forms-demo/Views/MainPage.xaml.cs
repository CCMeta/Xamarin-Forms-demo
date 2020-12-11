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
        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();
            FlyoutLayoutBehavior = FlyoutLayoutBehavior.Popover;
            MenuPages.Add((int)MenuItemType.Browse, (NavigationPage)Detail);
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                MenuPages.Add(id, new NavigationPage(HomeMenuItem.GetPageById(id)));
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
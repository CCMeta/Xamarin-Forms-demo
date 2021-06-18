using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin_Forms_demo.Models;
using Xamarin_Forms_demo.Services;

namespace Xamarin_Forms_demo.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        private readonly string path = "/api/contacts";
        public ObservableCollection<Contacts> contacts = new ObservableCollection<Contacts>();
        public ObservableCollection<Contacts> Contacts
        {
            get { return contacts; }
            set
            {
                foreach (var item in value)
                {
                    contacts.Insert(0, item);
                }
            }
        }
        public ICommand GetListCommand { protected set; get; }

        public ContactsViewModel() : base()
        {
            Title = "通讯录";
            GetListCommand = new Command(() =>
            {
                GetListAsync();
            });
        }

        public async void GetListAsync()
        {
            var queryParams = new Dictionary<string, string>() {};
            using var _ = HttpRequest.GetAsync<ObservableCollection<Contacts>>(path, queryParams: queryParams);
            Contacts = await _;
            IsBusy = false;
        }

    }
}
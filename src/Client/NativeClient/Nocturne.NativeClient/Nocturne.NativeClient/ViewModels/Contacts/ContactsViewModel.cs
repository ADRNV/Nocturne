using MvvmCross.Commands;
using Nocturne.Api.Client;
using Nocturne.NativeClient.ViewModels.Common;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Nocturne.NativeClient.ViewModels.Contacts
{
    public class ContactsViewModel : ViewModelBase, IContactsViewModel
    {
        private readonly Client _client;

        private IEnumerable<Group> _contacts = Enumerable.Empty<Group>();

        public ContactsViewModel(Client client) 
        { 
            _client = client;
        }

        public IEnumerable<Group> Contacts
        {
            get => _contacts;

            set
            {
                _contacts = value;
                RaisePropertyChanged(nameof(Contacts));
            }
        }

        public IMvxAsyncCommand GetContactsCommand => new MvxAsyncCommand(GetContacts);

        public async Task GetContacts()
        {
            _ = await _client.GroupsAsync();
        }
    }
}

using MvvmCross.Commands;
using Nocturne.ApiClient;
using System.Threading.Tasks;

namespace Nocturne.NativeClient.ViewModels.Contacts
{
    public class ContactsViewModel
    {
        private readonly Client _client;

        public ContactsViewModel(Client client) 
        { 

        }

        public IMvxAsyncCommand GetContactsCommand => new MvxAsyncCommand(GetContacts);

        public async Task GetContacts()
        {

        }
    }
}

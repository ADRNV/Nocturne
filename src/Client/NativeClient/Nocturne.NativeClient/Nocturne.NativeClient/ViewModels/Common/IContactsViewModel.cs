using MvvmCross.Commands;
using Nocturne.Api.Client;
using System.Collections.Generic;

namespace Nocturne.NativeClient.ViewModels.Common
{
    public interface IContactsViewModel
    {
        IEnumerable<Group> Contacts { get; set; }

        IMvxAsyncCommand GetContactsCommand { get; }
    }
}

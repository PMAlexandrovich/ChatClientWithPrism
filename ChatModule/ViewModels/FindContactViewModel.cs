using ChatModule.Business;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatModule.ViewModels
{
    public class FindContactViewModel : BindableBase
    {
        private readonly Contacts _contacts;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { SetProperty(ref _userName, value); }
        }

        private DelegateCommand _addContact;
        public DelegateCommand AddContact =>
            _addContact ?? (_addContact = new DelegateCommand(ExecuteAddContact));

        async void ExecuteAddContact()
        {
            var result = await _contacts.AddContact(UserName);
            //_contacts.ContactList.Add(new Contact(UserName));
        }

        public FindContactViewModel(Contacts contacts)
        {
            _contacts = contacts;
        }
    }
}

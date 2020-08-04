using ChatModule.Business;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChatModule.ViewModels
{
    public class InboxListViewModel : BindableBase
    {
        private readonly Contacts _contacts;        

        private DelegateCommand<string> _acceptContact;
        public DelegateCommand<string> AcceptContact =>
            _acceptContact ?? (_acceptContact = new DelegateCommand<string>(ExecuteAcceptContact));

        async void ExecuteAcceptContact(string name)
        {
            var result = await _contacts.AcceptRequest(name);
            
        }

        private DelegateCommand<string> _rejectRequest;
        public DelegateCommand<string> RejectRequest =>
            _rejectRequest ?? (_rejectRequest = new DelegateCommand<string>(ExecuteRejectRequest));

        async void ExecuteRejectRequest(string name)
        {
            var result = await _contacts.Reject(name);
        }

        private ObservableCollection<Contact> _inboxList;
        public ObservableCollection<Contact> InboxList
        {
            get { return _inboxList; }
            set { SetProperty(ref _inboxList, value); }
        }

        public InboxListViewModel(Contacts contacts)
        {
            _contacts = contacts;
            InboxList = _contacts.InboxList;
        }
    }
}

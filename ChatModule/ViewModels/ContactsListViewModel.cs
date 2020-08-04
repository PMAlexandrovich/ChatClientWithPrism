using ChatModule.Business;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ChatModule.ViewModels
{
    public class ContactsListViewModel : BindableBase
    {
        private readonly Contacts _contacts;
        private readonly IRegionManager _regionManager;
        IEventAggregator _ea;

        private ObservableCollection<Contact> _contactList;
        public ObservableCollection<Contact> ContactList
        {
            get { return _contactList; }
            set { SetProperty(ref _contactList, value); }
        }

        private DelegateCommand<string> _deleteContact;
        public DelegateCommand<string> DeleteContact =>
            _deleteContact ?? (_deleteContact = new DelegateCommand<string>(ExecuteDeleteContact));

        void ExecuteDeleteContact(string name)
        {
            _contacts.DeleteContact(name);
        }

        private DelegateCommand<string> _openChat;
        public DelegateCommand<string> OpenChat =>
            _openChat ?? (_openChat = new DelegateCommand<string>(ExecuteOpenChat));

        void ExecuteOpenChat(string userName)
        {
            //var parameters = new NavigationParameters();
            //parameters.Add("userName", userName);
            _ea.GetEvent<SelectedChatEvent>().Publish(new Chat() { Name = userName });

            //_regionManager.RequestNavigate("MainZon","Chat", parameters);

        }

        public ContactsListViewModel(Contacts contacts, IRegionManager regionManager, IEventAggregator ea)
        {
            _contacts = contacts;
            _regionManager = regionManager;
            _ea = ea;
            LoadAsync();
        }

        private async Task LoadAsync()
        {
            //await _contacts.GetContacts();
            ContactList = _contacts.ContactList;
        } 
    }
}

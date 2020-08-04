using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
//using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.SignalR.Client;
using Infrastructure;
using System.Windows.Data;

namespace ChatModule.Business
{
    public class Contacts: INotifyPropertyChanged,IDisposable
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion //INotifyPropertyChanged

        HttpClient _httpClient;
        HubConnection _connection;
        object _lock = new object();

        //private ObservableCollection<Contact> _contactList = new ObservableCollection<Contact>() { new Contact("Max"),new Contact("Vasya Pupcin")};
        private ObservableCollection<Contact> _contactList;
        public ObservableCollection<Contact> ContactList 
        { 
            get => _contactList; 
            private set {
                _contactList = value;
                OnPropertyChanged();
            }
        }

        //private ObservableCollection<Contact> _inboxList = new ObservableCollection<Contact>() { new Contact("FutureFriend"), new Contact("Good Boy") };
        private ObservableCollection<Contact> _inboxList;
        public ObservableCollection<Contact> InboxList
        {
            get => _inboxList;
            private set
            {
                _inboxList = value;
                OnPropertyChanged();
            }
        }

        //private ObservableCollection<SendedRequest> _outboxList = new ObservableCollection<SendedRequest>() { new SendedRequest("Alex"), new SendedRequest("Melman",false) };
        private ObservableCollection<SendedRequest> _outboxList;
        public ObservableCollection<SendedRequest> OutList
        {
            get => _outboxList;
            private set
            {
                _outboxList = value;
                OnPropertyChanged();
            }
        }

        public Contacts(AppData appData)
        {
            _connection = new HubConnectionBuilder().WithUrl(appData.ServerUrl + "/hubs/contacts", (options) =>
            {
                options.AccessTokenProvider = async () => appData.Token;
            }).WithAutomaticReconnect().Build();
            _connection.On<string>("ReceivedContact", ReceivedContact);
            _connection.On<string>("RejectedContact", RejectedContact);
            _connection.On<string>("CanceledContact", CanceledContact);
            _connection.On<string>("DeletedContact", DeletedContact);
            _connection.On<string>("AcceptContact", AcceptedContact);
            _connection.StartAsync();
            _httpClient = ServiceLocator.Current.GetInstance<HttpClient>();
            GetContacts();
        }

        public async Task GetContacts()
        {
            var result = await _httpClient.GetAsync(@"api/Contacts/Get");
            ContactList = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(result.Content.ReadAsStringAsync().Result);
            BindingOperations.EnableCollectionSynchronization(ContactList, _lock);

            var result2 = await _httpClient.GetAsync(@"api/Contacts/GetInboxList");
            InboxList = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(result2.Content.ReadAsStringAsync().Result);
            BindingOperations.EnableCollectionSynchronization(InboxList, _lock);

            var result3 = await _httpClient.GetAsync(@"api/Contacts/GetOutboxList");
            OutList = JsonConvert.DeserializeObject<ObservableCollection<SendedRequest>>(result3.Content.ReadAsStringAsync().Result);
            BindingOperations.EnableCollectionSynchronization(OutList, _lock);
        }

        //public async Task<HttpResponseMessage> AddContact(string username)
        //{
        //    var values = new Dictionary<string, string>()
        //    {
        //        {"username",username }
        //    };
        //    var request = new FormUrlEncodedContent(values);
        //    var result = await _httpClient.PostAsync(@"api/Contacts", request);

        //    if (result.IsSuccessStatusCode)
        //        ContactList.Add(new Contact() { Name = username });
        //    return result;
        //}

        //public void DeleteContact(Contact contact)
        //{

        //}


        public async Task<HttpResponseMessage> AddContact(string name)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",name },
            };

            var request = new FormUrlEncodedContent(values);

            var result = await _httpClient.PostAsync("api/Contacts/AddContact", request);
            if (result.IsSuccessStatusCode)
                OutList.Add(new SendedRequest(name));

            return result;
        }

        public async Task<HttpResponseMessage> Reject(string name)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",name },
            };

            var request = new FormUrlEncodedContent(values);
            var result = await _httpClient.PostAsync("api/Contacts/RejectContact", request);
            if (result.IsSuccessStatusCode)
            {
                var contact = InboxList.FirstOrDefault((x) => x.Name == name);
                InboxList.Remove(contact);
            }
            return result;
        }

        public async Task<HttpResponseMessage> RequestAgain(string name)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",name },
            };

            var request = new FormUrlEncodedContent(values);
            var result = await _httpClient.PostAsync("api/Contacts/RequestAgainContact", request);
            if (result.IsSuccessStatusCode)
            {
                var contact = OutList.FirstOrDefault((x) => x.Name == name);
                contact.IsCanceled = false;
            }
            return result;
        }

        public async Task<HttpResponseMessage> CancelRequest(string name)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",name },
            };

            var request = new FormUrlEncodedContent(values);
            var result = await _httpClient.PostAsync("api/Contacts/CancelContact", request);
            if (result.IsSuccessStatusCode)
            {
                var contact = OutList.FirstOrDefault((x) => x.Name == name);
                OutList.Remove(contact);
            }
            return result;
        }

        public async Task<HttpResponseMessage> AcceptRequest(string name)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",name },
            };

            var request = new FormUrlEncodedContent(values);
            var result = await _httpClient.PostAsync("api/Contacts/AcceptContact", request);
            if (result.IsSuccessStatusCode)
            {
                ContactList.Add(new Contact(name));
                var contact = InboxList.FirstOrDefault((x) => x.Name == name);
                InboxList.Remove(contact);
            }
            return result;
        }

        public async Task<HttpResponseMessage> DeleteContact(string name)
        {
            var values = new Dictionary<string, string>()
            {
                {"username",name },
            };

            var request = new FormUrlEncodedContent(values);
            var result = await _httpClient.PostAsync("api/Contacts/DeleteContact", request);
            if (result.IsSuccessStatusCode)
            {
                var contact = ContactList.FirstOrDefault((x) => x.Name == name);
                ContactList.Remove(contact);
            }
            return result;
        }

        //Hub methods
        public void AcceptedContact(string name)
        {
            ContactList.Add(new Contact(name));
            var contact = OutList.FirstOrDefault((x) => x.Name == name);
            OutList.Remove(contact);
        }

        public void ReceivedContact(string sender)
        {
            InboxList.Add(new Contact(sender));
        }

        public void RejectedContact(string name)
        {
            var contact = OutList.FirstOrDefault((x) => x.Name == name);
            if(contact != null)
                contact.IsCanceled = true;
        }
        public void CanceledContact(string name)
        {
            var contact = _inboxList.FirstOrDefault((x) => x.Name == name);
            if(contact != null)
                InboxList.Remove(contact);
        }
        public void DeletedContact(string name)
        {
            var contact = ContactList.FirstOrDefault((x) => x.Name == name);
            if(contact != null)
                ContactList.Remove(contact);
        }

        public Contact FindContact(string UserName)
        {
            return null;
        }

        public void SelectChatRoom(Contact contact)
        {
            //CurrentChatRoom = new ChatRoom(contact);
        }

        public void Dispose()
        {
        }
    }
}

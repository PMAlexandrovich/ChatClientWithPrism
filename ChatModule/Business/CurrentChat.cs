using CommonServiceLocator;
using Infrastructure;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace ChatModule.Business
{
    public class CurrentChat:INotifyPropertyChanged
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
        IEventAggregator _ea;
        object _lock = new object();

        string userName;
        public string InterlocutorName { get; set; }

        public CurrentChat(AppData appData, IEventAggregator ea)
        {
            _connection = new HubConnectionBuilder().WithUrl(appData.ServerUrl + "/hubs/PrivateChat", (options) =>
            {
                options.AccessTokenProvider = async () => appData.Token;
            }).WithAutomaticReconnect().Build();
            _connection.On<string,string,DateTime>("ReceivedMessage", ReceivedMessage);
            _connection.StartAsync();
            _httpClient = ServiceLocator.Current.GetInstance<HttpClient>();
            userName = appData.UserName;
            _ea = ea;
        }

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public void ReceivedMessage(string userName,string messageContent,DateTime dateCreate)
        {
            if (userName == InterlocutorName)
                Messages.Add(new Message { Sender = userName, Content = messageContent, DateCreate = dateCreate });
            else
                _ea.GetEvent<MessageReceivedEvent>().Publish(new Chat(userName, 1, dateCreate));
            MessageBox.Show(Thread.CurrentThread.Name);
        }

        public async Task<HttpResponseMessage> SendMessage(string interlocutorName, string messageContent)
        {
            var dateCreate = DateTime.Now;
            var values = new Dictionary<string, string>()
            {
                {"username", interlocutorName },
                {"message", messageContent },
                {"dateCreate", dateCreate.ToString() },
            };

            var request = new FormUrlEncodedContent(values);

            var result = await _httpClient.PostAsync("api/PrivateChat/SendMessage", request);
            if (result.IsSuccessStatusCode)
                Messages.Add(new Message() {Sender = userName, Content = messageContent, DateCreate = dateCreate});

            return result;
        }

        public async Task<HttpResponseMessage> GetMessages(string userName)
        {
            //var values = new Dictionary<string, string>()
            //{
            //    {"username", userName },
            //};

            //var request = new FormUrlEncodedContent(values);

            var result = await _httpClient.GetAsync("api/PrivateChat/GetMessagesWith/" + userName);
            if (result.IsSuccessStatusCode)
            {
                Messages = JsonConvert.DeserializeObject<ObservableCollection<Message>>(result.Content.ReadAsStringAsync().Result);
                BindingOperations.EnableCollectionSynchronization(Messages, _lock);
            }

            return result;
        }
    }
}

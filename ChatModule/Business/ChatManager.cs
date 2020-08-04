using CommonServiceLocator;
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
using System.Threading.Tasks;

namespace ChatModule.Business
{
    public class ChatManager:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion //INotifyPropertyChanged

        IEventAggregator _ea;
        HttpClient _httpClient;
        CurrentChat _currentChat;

        private ObservableCollection<Chat> _chats;
        public ObservableCollection<Chat> Chats
        {
            get { return _chats; }
            set
            {
                _chats = value;
                OnPropertyChanged();
            }
        }

        public ChatManager(IEventAggregator ea, CurrentChat currentChat)
        {
            _ea = ea;
            _ea.GetEvent<MessageReceivedEvent>().Subscribe(OnReceivedMessage);
            //_ea.GetEvent<SelectedChatEvent>().Subscribe(OnSelectedChat);
            _httpClient = ServiceLocator.Current.GetInstance<HttpClient>();
            _currentChat = ServiceLocator.Current.GetInstance<CurrentChat>();
        }

        public async Task Initialization()
        {
            var result = await _httpClient.GetAsync(@"/api/PrivateChat/GetAllVisiblePrivateChats");
            Chats = JsonConvert.DeserializeObject<ObservableCollection<Chat>>(result.Content.ReadAsStringAsync().Result);
        }

        public void OnReceivedMessage(Chat chat)
        {
            var _chat = Chats.FirstOrDefault((x) => x.Name == chat.Name);
            if (_chat == null)
            {
                chat.CountOfUnreadMessages = 1;
                Chats.Add(chat);
            }
            else
            {
                _chat.CountOfUnreadMessages++;
                _chat.LastMessageWas = chat.LastMessageWas;
            }
        }

        //public void OnSelectedChat(Chat chat)
        //{
        //    var _chat = Chats.FirstOrDefault((x) => x.Name == chat.Name);
        //    if (_chat == null)
        //    {
        //        chat.CountOfUnreadMessages = 0;
        //        Chats.Add(chat);
        //    }
        //    else
        //    {
        //        _chat.CountOfUnreadMessages = 0;
        //        _httpClient.PostAsync(@"/api/PrivateChat/AllMessagesWasReadWith", null);
        //    }
        //}

        public async Task<HttpResponseMessage> AllMessagesWasReadWith(string userName)
        {
           return await _httpClient.PostAsync(@"/api/PrivateChat/AllMessagesWasReadWith/"+userName, null);
        }
    }
}

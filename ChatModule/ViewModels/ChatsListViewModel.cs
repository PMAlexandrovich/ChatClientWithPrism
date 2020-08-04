using ChatModule.Business;
using CommonServiceLocator;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;

namespace ChatModule.ViewModels
{
    public class ChatsListViewModel : BindableBase
    {
        IEventAggregator _ea;
        ChatManager _chatManager;
        IRegionManager _regionManager;

        private ObservableCollection<Chat> _chats;
        public ObservableCollection<Chat> Chats
        {
            get { return _chats; }
            set
            {
                SetProperty(ref _chats, value);
            }
        }

        private Chat _selectedChat;
        public Chat SelectedChat
        {
            get { return _selectedChat; }
            set 
            { 
                SetProperty(ref _selectedChat, value);
                AllMessagesWasRead(value);
                NavigateToChatWith(value);
            }
        }

        public ChatsListViewModel(ChatManager chatManager, IEventAggregator ea, IRegionManager regionManager)
        {
            _ea = ea;
            _chatManager = chatManager;
            _regionManager = regionManager;
            _ea.GetEvent<SelectedChatEvent>().Subscribe(OnSelectedChat);

            Initialization();
        }

        private async void Initialization()
        {
            await _chatManager.Initialization();
            Chats = _chatManager.Chats;
        }

        public void OnSelectedChat(Chat chat)
        {
            var _chat = Chats.FirstOrDefault((x) => x.Name == chat.Name);
            if (_chat == null)
            {
                chat.CountOfUnreadMessages = 0;
                Chats.Add(chat);
                SelectedChat = chat;
            }
            else
            {
                SelectedChat = _chat;
            }
        }

        public void AllMessagesWasRead(Chat chat)
        {
            if (chat.CountOfUnreadMessages > 0)
            {
                chat.CountOfUnreadMessages = 0;
                _chatManager.AllMessagesWasReadWith(chat.Name);
            }
        }

        public void NavigateToChatWith(Chat chat)
        {
            var parameters = new NavigationParameters();
            parameters.Add("userName", chat.Name);

            _regionManager.RequestNavigate("MainZon", "Chat", parameters);
        }
    }
}

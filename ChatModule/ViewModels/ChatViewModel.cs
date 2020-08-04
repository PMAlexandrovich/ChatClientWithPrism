using ChatModule.Business;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChatModule.ViewModels
{
    public class ChatViewModel : BindableBase, INavigationAware
    {
        CurrentChat _currentChat;

        private ObservableCollection<Message> _messages;
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set 
            {
                SetProperty(ref _messages, value);
            }
        }

        private string _interlocutorName;
        public string InterlocutorName
        {
            get { return _interlocutorName; }
            set 
            {
                SetProperty(ref _interlocutorName, value);
            }
        }
        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                SetProperty(ref _message, value);
                SendMessage.RaiseCanExecuteChanged();
            }
        }

        private DelegateCommand _sendMessage;
        public DelegateCommand SendMessage =>
            _sendMessage ?? (_sendMessage = new DelegateCommand(ExecuteSendMessage, CanExecuteCommand));

        void ExecuteSendMessage()
        {
            _currentChat.SendMessage(InterlocutorName, Message);
            Message = "";
        }
        
        bool CanExecuteCommand()
        {
            return !string.IsNullOrEmpty(Message);
        }

        public ChatViewModel(CurrentChat currentChat)
        {
            _currentChat = currentChat;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            var userName = navigationContext.Parameters["userName"] as string;
            InterlocutorName = userName;
            _currentChat.InterlocutorName = userName;
            await _currentChat.GetMessages(InterlocutorName);
            Messages = _currentChat.Messages;
            Message = "";

            //Messages = new ObservableCollection<Message>(new List<Message>()
            //{
            //    new Message() {Sender = "Max",Content = "Привет.", DateCreate = new DateTime(2020,3,25,5,50,0) },
            //    new Message() {Sender = "Test",Content = "Привет, как дела ?", DateCreate = new DateTime(2020,3,25,5,50,0) },
            //    new Message() {Sender = "Test",Content = "Нормально, как у тебя ?", DateCreate = new DateTime(2020,3,25,5,50,0) },
            //    new Message() {Sender = "Max",Content = "Отлично !", DateCreate = new DateTime(2020,3,25,5,50,0)}
            //});
        }
    }
}

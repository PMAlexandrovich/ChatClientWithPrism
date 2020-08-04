using ChatModule.Business;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ChatModule.ViewModels
{
    public class OutboxListViewModel : BindableBase
    {
        private readonly Contacts _contacts;

        private DelegateCommand<string> _cancelRequest;
        public DelegateCommand<string> CancelRequest =>
            _cancelRequest ?? (_cancelRequest = new DelegateCommand<string>(ExecuteCommand));

        async void ExecuteCommand(string name)
        {
            var result = await _contacts.CancelRequest(name);
        }

        private DelegateCommand<string> _sendAgain;
        public DelegateCommand<string> SendAgain =>
            _sendAgain ?? (_sendAgain = new DelegateCommand<string>(ExecuteSendAgain));

        async void ExecuteSendAgain(string name)
        {
            var result = await _contacts.RequestAgain(name);
        }

        private ObservableCollection<SendedRequest> _outList;
        public ObservableCollection<SendedRequest> OutList
        {
            get { return _outList; }
            set { SetProperty(ref _outList, value); }
        }

        public OutboxListViewModel(Contacts contacts)
        {
            _contacts = contacts;
            OutList = _contacts.OutList;
        }
    }
}

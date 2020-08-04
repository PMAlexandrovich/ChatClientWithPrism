using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule.Business
{
    public class Chat:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion //INotifyPropertyChanged

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private int _countOfUnreadMessages;
        public int CountOfUnreadMessages
        {
            get => _countOfUnreadMessages;
            set
            {
                _countOfUnreadMessages = value;
                OnPropertyChanged();
            }
        }

        private DateTime _lastMessageWas;
        public DateTime LastMessageWas
        {
            get => _lastMessageWas;
            set
            {
                _lastMessageWas = value;
                OnPropertyChanged();
            }
        }

        public Chat()
        {

        }
        public Chat(string name, int count, DateTime dateTime)
        {
            Name = name;
            CountOfUnreadMessages = count;
            LastMessageWas = dateTime;
        }
    }
}

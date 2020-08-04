using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule.Business
{
    public class SendedRequest:INotifyPropertyChanged
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
        private bool _isCanceled;
        public bool IsCanceled { get=> _isCanceled;
            set 
            {
                _isCanceled = value;
                OnPropertyChanged();
            }
        }

        public SendedRequest() 
        {

        }
        public SendedRequest(string name)
        {
            Name = name;
        }
        public SendedRequest(string name, bool isCanceled) : this(name)
        {
            IsCanceled = isCanceled;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule.Business
{
    public class Contact:INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName]string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        #endregion //INotifyPropertyChanged
        //public int UserId { get; set; }
        private string _name;
        public string Name { get => _name;
            set {
                _name = value;
                OnPropertyChanged();
            } 
        }
        public Contact()
        {
        }
        public Contact(string name)
        {
            Name = name;
        }
    }
}

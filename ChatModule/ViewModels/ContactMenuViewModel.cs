using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ChatModule.ViewModels
{
    public class ContactMenuViewModel : BindableBase
    {
        //IRegionManager _regionManager;

        //private DelegateCommand<string> _changeMenu;
        //public DelegateCommand<string> ChangeMenu =>
        //    _changeMenu ?? (_changeMenu = new DelegateCommand<string>(ExecuteChangeMenu));

        //void ExecuteChangeMenu(string parameter)
        //{
        //    switch (parameter)
        //    {
        //        case "contacts":
        //            _regionManager.RequestNavigate("CurrentMenuContact", "ContactsList");
        //            break;
        //        case "outbox":
        //            _regionManager.RequestNavigate("CurrentMenuContact", "OutboxList");
        //            break;
        //        case "inbox":
        //            _regionManager.RequestNavigate("CurrentMenuContact", "InboxList");
        //            break;
        //    }
        //}

        public ContactMenuViewModel(IRegionManager regionManager)
        {
            //_regionManager = regionManager;
        }
    }
}

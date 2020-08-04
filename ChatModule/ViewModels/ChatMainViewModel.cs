using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ChatModule.ViewModels
{
    public class ChatMainViewModel : BindableBase
    {
        IRegionManager _regionManager;

        private DelegateCommand _navigateToContactMenu;
        public DelegateCommand NavigateToContactMenu =>
            _navigateToContactMenu ?? (_navigateToContactMenu = new DelegateCommand(ExecuteNavigateToContactMenu));

        void ExecuteNavigateToContactMenu()
        {
            _regionManager.RequestNavigate("MainZon", "ContactMenu");
        }

        public ChatMainViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }
    }
}

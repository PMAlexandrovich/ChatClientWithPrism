using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthenticationModule.ViewModels
{
    public class RegistrationCompletionViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        string _login;
        private DelegateCommand _navigateToLogIn;
        public DelegateCommand NavigateToSignIn =>
            _navigateToLogIn ?? (_navigateToLogIn = new DelegateCommand(ExecuteNavigateToSignIn));

        void ExecuteNavigateToSignIn()
        {
            var parameters = new NavigationParameters();
            parameters.Add("login", _login);
            _regionManager.RequestNavigate("Auth", "SignIn", parameters);
        }

        public RegistrationCompletionViewModel()
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _login = navigationContext.Parameters["login"] as string;
        }
    }
}

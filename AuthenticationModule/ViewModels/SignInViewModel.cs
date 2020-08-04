using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using AuthenticationModule.Business;
using System.Windows.Controls;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace AuthenticationModule.ViewModels
{
    public class SignInViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private readonly Authentication _authentication;

        private string login;
        public string Login
        {
            get { return login; }
            set 
            { 
                SetProperty(ref login, value);
                SignIn.RaiseCanExecuteChanged();
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        private DelegateCommand _navigateToSignUp;
        public DelegateCommand NavigateToSignUp =>
            _navigateToSignUp ?? (_navigateToSignUp = new DelegateCommand(ExecuteNavigateToSignUp));

        private DelegateCommand<PasswordBox> _signIn;
        public DelegateCommand<PasswordBox> SignIn =>
            _signIn ?? (_signIn = new DelegateCommand<PasswordBox>(ExecuteSignIn, (x) => !string.IsNullOrEmpty(Login)));

        private async void ExecuteSignIn(PasswordBox passwordBox)
        {
            var result = await _authentication.LoginAsync(Login, passwordBox.Password);
            if (result.IsSuccessStatusCode)
            {
                JObject obj = JObject.Parse(await result.Content.ReadAsStringAsync());
                var token = obj["access_token"].ToString();
                _authentication.SetAccessToken(token);
                _regionManager.RequestNavigate("MainContent", "ChatMain");
            }
            else
            {
                var content = await result.Content.ReadAsStringAsync();
                var errors = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(content);
                foreach (var error in errors.Values)
                {
                    ErrorMessage += error[0] + Environment.NewLine;
                }
            }
        }

        void ExecuteNavigateToSignUp()
        {
            _regionManager.RequestNavigate("Auth", "SignUp");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Login = navigationContext.Parameters["login"] as string;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }

        public SignInViewModel(IRegionManager regionManager, Authentication authentication)
        {
            _regionManager = regionManager;
            _authentication = authentication;
        }
    }
}

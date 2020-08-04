using AuthenticationModule.Business;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace AuthenticationModule.ViewModels
{
    public class SignUpViewModel : BindableBase
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
                SignUp.RaiseCanExecuteChanged();
            }
        }
        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set { SetProperty(ref _errorMessage, value); }
        }

        private DelegateCommand _navigateToSignIn;
        public DelegateCommand NavigateToSignIn =>
            _navigateToSignIn ?? (_navigateToSignIn = new DelegateCommand(ExecuteNavigateToSignIn));

        private DelegateCommand<object[]> _signUp;
        public DelegateCommand<object[]> SignUp =>
            _signUp ?? (_signUp = new DelegateCommand<object[]>(ExecuteSignUn,(x)=> !string.IsNullOrEmpty(Login)));

        async void ExecuteSignUn(object[] passBoxs)
        {
            ErrorMessage = "";
            var pass = passBoxs[0] as PasswordBox;
            var conPass = passBoxs[1] as PasswordBox;
            if(pass.Password == conPass.Password)
            {
                var result = await _authentication.RegisterAsync(Login, pass.Password);
                if (result.IsSuccessStatusCode)
                {
                    var parameters = new NavigationParameters();
                    parameters.Add("login", Login);
                    _regionManager.RequestNavigate("Auth", "RegistrationCompletion",parameters);
                }
                else
                {
                    var content = await result.Content.ReadAsStringAsync();
                    var errors = JsonConvert.DeserializeObject<Dictionary<string,string[]>>(content);
                    foreach(var error in errors.Values)
                    {
                        ErrorMessage += error[0] + Environment.NewLine;
                    }
                }
            }
            else
            {
                ErrorMessage = "Пароли не совпадают";
            }
        }

        void ExecuteNavigateToSignIn()
        {
            _regionManager.RequestNavigate("Auth", "SignIn");
        }

        public SignUpViewModel(IRegionManager regionManager, Authentication authentication)
        {
            _regionManager = regionManager;
            _authentication = authentication;
        }
    }
}

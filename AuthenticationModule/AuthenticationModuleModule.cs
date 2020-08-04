using AuthenticationModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AuthenticationModule
{
    public class AuthenticationModuleModule : IModule
    {
        IRegionManager _regionManager;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            var _regionManager = containerProvider.Resolve<IRegionManager>();
            _regionManager.RequestNavigate("MainContent", "AuthenticationView");
            _regionManager.RequestNavigate("Auth", "SignIn");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<AuthenticationView>();
            containerRegistry.RegisterForNavigation<SignIn>();
            containerRegistry.RegisterForNavigation<SignUp>();
            containerRegistry.RegisterForNavigation<RegistrationCompletion>();
        }
    }
}
using ChatModule.Business;
using ChatModule.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Unity;
using ChatModule.ViewModels;
using CommonServiceLocator;

namespace ChatModule
{
    public class ChatModuleModule : IModule
    {
        private IRegionManager _regionManager;
        private IContainerProvider _container;

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager = containerProvider.Resolve<IRegionManager>();
            _container = containerProvider;

            //_regionManager.RegisterViewWithRegion("ContactList", () => _container.Resolve<ContactsList>());
            _regionManager.RegisterViewWithRegion("ChatList", () => _container.Resolve<ChatsList>());
            _regionManager.RegisterViewWithRegion("AddContact", () => _container.Resolve<FindContact>());

            //var contactList = containerProvider.Resolve<ContactsList>();
            //region.Add(contactList);

            //var chatsList = containerProvider.Resolve<ChatsList>();
            //region.Add(chatsList);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<Contacts>();
            containerRegistry.RegisterSingleton<CurrentChat>();
            //var currentChat = ServiceLocator.Current.TryResolve<CurrentChat>();
            //containerRegistry.RegisterInstance<CurrentChat>(currentChat);

            containerRegistry.RegisterForNavigation<ChatMain>();
            containerRegistry.RegisterForNavigation<ContactMenu>();
            containerRegistry.RegisterForNavigation<Views.Chat>();

            //containerRegistry.RegisterForNavigation<ContactsList>();
            //containerRegistry.RegisterForNavigation<InboxList>();
            //containerRegistry.RegisterForNavigation<OutboxList>();
        }
    }
}
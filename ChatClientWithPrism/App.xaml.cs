using ChatClientWithPrism.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Net.Http;
using System.Windows;
using System;
using Prism.Unity;
using Infrastructure;
using System.Configuration;

namespace ChatClientWithPrism
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App:PrismApplication
    {

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var appSettings = ConfigurationManager.AppSettings;
            var serverUrl = appSettings["ServerUrl"];

            var data = new AppData() { ServerUrl = serverUrl };
            containerRegistry.RegisterInstance(data);
            
            var httpClient = new HttpClient() { BaseAddress = new Uri(serverUrl) };
            containerRegistry.RegisterInstance(typeof(HttpClient), httpClient);
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<AuthenticationModule.AuthenticationModuleModule>();
            moduleCatalog.AddModule<ChatModule.ChatModuleModule>();
        }
    }
}

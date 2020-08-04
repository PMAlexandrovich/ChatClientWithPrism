using Prism.Ioc;
using Prism.Regions;
using System.Windows.Controls;

namespace ChatModule.Views
{
    /// <summary>
    /// Interaction logic for ContactMenu
    /// </summary>
    public partial class ContactMenu : UserControl
    {
        IContainerExtension _container;
        IRegionManager _regionManager;
        IRegion _region;

        ContactsList _contactsList;
        InboxList _inboxList;
        OutboxList _outboxList;

        public ContactMenu(IContainerExtension container, IRegionManager regionManager)
        {
            InitializeComponent();
            _container = container;
            _regionManager = regionManager;
            this.Loaded += ContactMenu_Loaded;
        }

        private void ContactMenu_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _contactsList = _container.Resolve<ContactsList>();
            _inboxList = _container.Resolve<InboxList>();
            _outboxList = _container.Resolve<OutboxList>();

            _region = _regionManager.Regions["CurrentMenuContact"];

            _region.Add(_contactsList);
            _region.Add(_inboxList);
            _region.Add(_outboxList);
            //_region.Activate(_contactsList);
        }

        private void RadioButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _region.Activate(_contactsList);
        }

        private void RadioButton_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            _region.Activate(_inboxList);
        }

        private void RadioButton_Click_2(object sender, System.Windows.RoutedEventArgs e)
        {
            _region.Activate(_outboxList);
        }
    }
}

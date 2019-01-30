using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.System.RemoteSystems;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class RemoteDeviceHelperPage : Page
    {
        private RemoteDeviceHelper _remoteDeviceHelper;

        public RemoteDeviceHelperPage()
        {
            this.InitializeComponent();

            var selectionEnum = Enum.GetNames(typeof(RemoteSystemDiscoveryType)).Cast<string>().OrderBy(q => q).ToList();
            discoveryType.ItemsSource = selectionEnum;
            discoveryType.SelectedIndex = 0;

            selectionEnum = Enum.GetNames(typeof(RemoteSystemAuthorizationKind)).Cast<string>().OrderBy(q => q).ToList();
            authorizationType.ItemsSource = selectionEnum;
            authorizationType.SelectedIndex = 0;

            selectionEnum = Enum.GetNames(typeof(RemoteSystemStatusType)).Cast<string>().OrderBy(q => q).ToList();
            statusType.ItemsSource = selectionEnum;
            statusType.SelectedIndex = 0;
        }

        private void Button_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var filters = new List<IRemoteSystemFilter>
            {
                new RemoteSystemDiscoveryTypeFilter((RemoteSystemDiscoveryType)Enum.Parse(typeof(RemoteSystemDiscoveryType), discoveryType?.SelectedValue.ToString())),
                new RemoteSystemAuthorizationKindFilter((RemoteSystemAuthorizationKind)Enum.Parse(typeof(RemoteSystemAuthorizationKind), authorizationType?.SelectedValue.ToString())),
                new RemoteSystemStatusTypeFilter((RemoteSystemStatusType)Enum.Parse(typeof(RemoteSystemStatusType), statusType?.SelectedValue.ToString()))
            };

            

            _remoteDeviceHelper = new RemoteDeviceHelper(filters);
            DevicesList.DataContext = _remoteDeviceHelper;
        }
    }
}

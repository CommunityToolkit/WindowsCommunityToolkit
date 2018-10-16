// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.System.RemoteSystems;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Picker Control to show List of Remote Devices that are accessible
    /// </summary>
    [TemplatePart(Name = "PART_ListDevices", Type = typeof(ListView))]
    [TemplatePart(Name = "PART_ListDeviceTypes", Type = typeof(ComboBox))]
    [TemplatePart(Name = "PART_ProgressRing", Type = typeof(ProgressRing))]
    public sealed class RemoteDevicePicker : ContentDialog
    {
        private ListView _listDevices;
        private ComboBox _listDeviceTypes;
        private ProgressRing _progressRing;
        private Grid _commandSpace;
        private ComboBox _deviceDiscoveryItemsControl;
        private ComboBox _deviceStatusItemsControl;
        private ComboBox _authorizationTypeItemsControl;
        private Grid _advancedFiltersGrid;

        /// <summary>
        /// Gets or sets List of All Remote Systems based on Selection Filter
        /// </summary>
        private ObservableCollection<RemoteSystem> RemoteSystems { get; set; }

        /// <summary>
        /// Gets or sets the DeviceList Selection Mode. Defaults to ListViewSelectionMode.Single
        /// </summary>
        public RemoteDeviceSelectionMode SelectionMode
        {
            get { return (RemoteDeviceSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="SelectionMode"/>.
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            nameof(SelectionMode),
            typeof(RemoteDeviceSelectionMode),
            typeof(RemoteDevicePicker),
            new PropertyMetadata(RemoteDeviceSelectionMode.Single));

        /// <summary>
        /// Gets or sets a value indicating whether Advanced Filters visible or not
        /// </summary>
        public bool ShowAdvancedFilters
        {
            get { return (bool)GetValue(ShowAdvancedFiltersProperty); }
            set { SetValue(ShowAdvancedFiltersProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="ShowAdvancedFilters"/>.
        /// </summary>
        public static readonly DependencyProperty ShowAdvancedFiltersProperty = DependencyProperty.Register(
            nameof(ShowAdvancedFilters),
            typeof(bool),
            typeof(RemoteDevicePicker),
            new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteDevicePicker"/> class.
        /// </summary>
        public RemoteDevicePicker()
        {
            DefaultStyleKey = typeof(RemoteDevicePicker);
            RemoteSystems = new ObservableCollection<RemoteSystem>();
        }

        /// <summary>
        /// Initiate Picker UI
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IEnumerable<RemoteSystem>> PickDeviceAsync()
        {
            await ShowAsync();
            IEnumerable<RemoteSystem> devices = ReturnDevices();
            return devices;
        }

        internal IEnumerable<RemoteSystem> ReturnDevices()
        {
            List<RemoteSystem> returnValue = new List<RemoteSystem>();
            foreach (RemoteSystem item in _listDevices.SelectedItems)
            {
                returnValue.Add(item);
            }

            return returnValue.AsEnumerable();
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            if (_listDeviceTypes != null)
            {
                _listDeviceTypes.SelectionChanged -= ListDeviceTypes_SelectionChanged;
            }

            if (_listDevices != null)
            {
                _listDevices.SelectionChanged -= ListDevices_SelectionChanged;
                _listDevices.DoubleTapped -= ListDevices_DoubleTapped;
                _listDevices.ContainerContentChanging -= ListDevices_ContainerContentChanging;
                _listDeviceTypes.SelectedIndex = -1;
            }

            _listDevices = GetTemplateChild("PART_ListDevices") as ListView;
            _listDeviceTypes = GetTemplateChild("PART_ListDeviceTypes") as ComboBox;
            _progressRing = GetTemplateChild("PART_ProgressRing") as ProgressRing;
            _commandSpace = GetTemplateChild("CommandSpace") as Grid;
            _deviceDiscoveryItemsControl = GetTemplateChild("DiscoveryType") as ComboBox;
            _deviceStatusItemsControl = GetTemplateChild("StatusType") as ComboBox;
            _authorizationTypeItemsControl = GetTemplateChild("AuthorizationType") as ComboBox;
            _advancedFiltersGrid = GetTemplateChild("AdvancedFiltersGrid") as Grid;

            List<string> deviceList = typeof(RemoteSystemKinds).GetProperties().Select(a => a.Name).ToList();
            deviceList.Add("All");
            deviceList.Add("Unknown");

            if (_listDeviceTypes != null)
            {
                _listDeviceTypes.ItemsSource = deviceList.OrderBy(a => a.ToString());
                _listDeviceTypes.SelectionChanged += ListDeviceTypes_SelectionChanged;
            }

            if (_listDevices != null)
            {
                _listDevices.SelectionChanged += ListDevices_SelectionChanged;
                _listDevices.DoubleTapped += ListDevices_DoubleTapped;
                _listDevices.ContainerContentChanging += ListDevices_ContainerContentChanging;

                _commandSpace.Visibility = Visibility.Visible;
                _listDevices.SelectionMode = SelectionMode == RemoteDeviceSelectionMode.Single ? ListViewSelectionMode.Single : ListViewSelectionMode.Multiple;
                _listDevices.IsMultiSelectCheckBoxEnabled = SelectionMode == RemoteDeviceSelectionMode.Multiple ? true : false;
            }

            if (_advancedFiltersGrid != null)
            {
                _advancedFiltersGrid.Visibility = ShowAdvancedFilters ? Visibility.Visible : Visibility.Collapsed;
            }

            RemoteDeviceHelper romeHelper = new RemoteDeviceHelper();
            RemoteSystems = romeHelper.RemoteSystems;

            UpdateProgressRing(true);
            UpdateList();

            Focus(FocusState.Programmatic);

            LoadFilters();

            base.OnApplyTemplate();
        }

        private void LoadFilters()
        {
            List<string> discoveryType = typeof(RemoteSystemDiscoveryType).GetEnumNames().OrderBy(a => a.ToString()).ToList();
            _deviceDiscoveryItemsControl.ItemsSource = discoveryType;

            List<string> statusType = typeof(RemoteSystemStatusType).GetEnumNames().OrderBy(a => a.ToString()).ToList();
            _deviceStatusItemsControl.ItemsSource = statusType;

            List<string> authType = typeof(RemoteSystemAuthorizationKind).GetEnumNames().OrderBy(a => a.ToString()).ToList();
            _authorizationTypeItemsControl.ItemsSource = authType;
        }

        private void ListDevices_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            RemoteSystem model = (RemoteSystem)args.Item;
            switch (model.Status)
            {
                case RemoteSystemStatus.Available:
                    args.ItemContainer.IsEnabled = true;
                    break;

                case RemoteSystemStatus.DiscoveringAvailability:
                case RemoteSystemStatus.Unavailable:
                case RemoteSystemStatus.Unknown:
                    args.ItemContainer.IsEnabled = false;
                    break;
            }
        }

        private void ListDevices_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_listDevices.SelectionMode == ListViewSelectionMode.Single)
            {
                ReturnDevices();
                Hide();
            }
        }

        private void ListDevices_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            ReturnDevices();
            Hide();
        }

        private void ListDeviceTypes_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateList();

        private void UpdateList()
        {
            ObservableCollection<RemoteSystem> bindingList = new ObservableCollection<RemoteSystem>();
            if (RemoteSystems != null)
            {
                var bindinglist = _listDeviceTypes.SelectedValue.ToString().Equals("All")
                    ? RemoteSystems
                    : new ObservableCollection<RemoteSystem>(RemoteSystems.Where(a => a.Kind.Equals(_listDeviceTypes.SelectedValue.ToString(), StringComparison.OrdinalIgnoreCase)).ToList());

                _listDevices.ItemsSource = bindinglist;
                UpdateProgressRing(false);
            }
        }

        private void UpdateProgressRing(bool state)
        {
            if (_progressRing != null)
            {
                _progressRing.IsActive = state;
            }
        }
    }
}
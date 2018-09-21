// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.System.RemoteSystems;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

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

        /// <summary>
        /// Gets or sets List of All Remote Systems based on Selection Filter
        /// </summary>
        private ObservableCollection<RemoteSystem> RemoteSystems { get; set; }

        /// <summary>
        /// Gets or sets the Line Color on control Header. takes **SystemControlBackgroundAccentBrush** by default
        /// </summary>
        public Brush HeaderLineBrush
        {
            get { return (Brush)GetValue(HeaderLineBrushProperty); }
            set { SetValue(HeaderLineBrushProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="HeaderLineBrush"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderLineBrushProperty = DependencyProperty.Register(
            nameof(HeaderLineBrush),
            typeof(Brush),
            typeof(RemoteDevicePicker),
            new PropertyMetadata(Application.Current.Resources["SystemControlBackgroundAccentBrush"]));

        /// <summary>
        /// Gets or sets the DeviceList Selection Mode. Defaults to ListViewSelectionMode.Single
        /// </summary>
        public ListViewSelectionMode SelectionMode
        {
            get { return (ListViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="SelectionMode"/>.
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(
            nameof(SelectionMode),
            typeof(ListViewSelectionMode),
            typeof(RemoteDevicePicker),
            new PropertyMetadata(ListViewSelectionMode.Single));

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
            _listDevices = GetTemplateChild("PART_ListDevices") as ListView;
            _listDeviceTypes = GetTemplateChild("PART_ListDeviceTypes") as ComboBox;
            _progressRing = GetTemplateChild("PART_ProgressRing") as ProgressRing;
            _commandSpace = GetTemplateChild("CommandSpace") as Grid;

            List<string> deviceList = typeof(RemoteSystemKinds).GetProperties().Select(a => a.Name).ToList();
            deviceList.Add("All");
            deviceList.Add("Unknown");

            _listDeviceTypes.ItemsSource = deviceList.OrderBy(a => a.ToString());
            _listDeviceTypes.SelectionChanged += ListDeviceTypes_SelectionChanged;
            _listDevices.SelectionChanged += ListDevices_SelectionChanged;
            _listDevices.DoubleTapped += ListDevices_DoubleTapped;
            _listDeviceTypes.SelectedIndex = 0;

            _commandSpace.Visibility = SelectionMode == ListViewSelectionMode.Single ? Visibility.Collapsed : Visibility.Visible;

            RomeHelper romeHelper = new RomeHelper();
            RemoteSystems = romeHelper.RemoteSystems;

            UpdateProgressRing(true);
            UpdateList();

            base.OnApplyTemplate();
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

        internal void UpdateList()
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

        internal void UpdateProgressRing(bool state)
        {
            if (_progressRing != null)
            {
                _progressRing.IsActive = state;
            }
        }
    }
}
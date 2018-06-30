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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        private RemoteSystemWatcher _remoteSystemWatcher;

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
            SecondaryButtonText = "Done";
            RemoteSystems = new ObservableCollection<RemoteSystem>();
        }

        /// <summary>
        /// Initiate Picker UI
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IEnumerable<RemoteSystem>> PickDeviceAsync()
        {
            List<RemoteSystem> returnValue = new List<RemoteSystem>();
            await ShowAsync();
            _remoteSystemWatcher.Stop();
            foreach (RemoteSystem item in _listDevices.SelectedItems)
            {
                returnValue.Add(item);
            }

            return returnValue.AsEnumerable();
        }

        /// <inheritdoc />
        protected async override void OnApplyTemplate()
        {
            _listDevices = GetTemplateChild("PART_ListDevices") as ListView;
            _listDeviceTypes = GetTemplateChild("PART_ListDeviceTypes") as ComboBox;
            _progressRing = GetTemplateChild("PART_ProgressRing") as ProgressRing;

            List<string> deviceList = typeof(RemoteSystemKinds).GetProperties().Select(a => a.Name).ToList();
            deviceList.Add("All");
            deviceList.Add("Unknown");

            _listDeviceTypes.ItemsSource = deviceList.OrderBy(a => a.ToString());
            _listDeviceTypes.SelectionChanged += ListDeviceTypes_SelectionChanged;
            _listDeviceTypes.SelectedIndex = 0;

            RemoteSystemAccessStatus accessStatus = await RemoteSystem.RequestAccessAsync();
            if (accessStatus == RemoteSystemAccessStatus.Allowed)
            {
                _remoteSystemWatcher = RemoteSystem.CreateWatcher();
                _remoteSystemWatcher.RemoteSystemAdded += RemoteSystemWatcher_RemoteSystemAdded;
                _remoteSystemWatcher.RemoteSystemRemoved += RemoteSystemWatcher_RemoteSystemRemoved;
                _remoteSystemWatcher.RemoteSystemUpdated += RemoteSystemWatcher_RemoteSystemUpdated;
                _remoteSystemWatcher.Start();
            }

            UpdateProgressRing(true);
            UpdateList();

            base.OnApplyTemplate();
        }

        private void ListDeviceTypes_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateList();

        private void RemoteSystemWatcher_RemoteSystemUpdated(RemoteSystemWatcher sender, RemoteSystemUpdatedEventArgs args)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                UpdateProgressRing(true);
                RemoteSystems.Remove(RemoteSystems.First(a => a.Id == args.RemoteSystem.Id));
                RemoteSystems.Add(args.RemoteSystem);
                UpdateList();
            });
        }

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

        private async void RemoteSystemWatcher_RemoteSystemRemoved(RemoteSystemWatcher sender, RemoteSystemRemovedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateProgressRing(true);
                RemoteSystems.Remove(RemoteSystems.First(a => a.Id == args.RemoteSystemId));
                UpdateList();
            });
        }

        private async void RemoteSystemWatcher_RemoteSystemAdded(RemoteSystemWatcher sender, RemoteSystemAddedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateProgressRing(true);
                RemoteSystems.Add(args.RemoteSystem);
                UpdateList();
            });
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
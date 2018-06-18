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
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Picker Control to show List of Remote Devices that are accessible
    /// </summary>
    public sealed class RemoteDevicePicker : ContentDialog
    {
        private Dictionary<string, RemoteSystem> DeviceMap { get; set; }

        private ListView listDevices;
        private ComboBox listDeviceTypes;
        private ProgressRing progressRing;

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
            DeviceMap = new Dictionary<string, RemoteSystem>();
        }

        /// <summary>
        /// Initiate Picker UI
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<List<RemoteSystem>> PickDeviceAsync()
        {
            List<RemoteSystem> remoteSystems = new List<RemoteSystem>();
            await ShowAsync();
            foreach (RemoteSystem system in listDevices.SelectedItems)
            {
                remoteSystems.Add(system);
            }

            return remoteSystems;
        }

        /// <inheritdoc />
        protected async override void OnApplyTemplate()
        {
            listDevices = GetTemplateChild("PART_ListDevices") as ListView;
            listDeviceTypes = GetTemplateChild("PART_ListDeviceTypes") as ComboBox;
            progressRing = GetTemplateChild("PART_ProgressRing") as ProgressRing;

            var enumval = Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>();
            listDeviceTypes.ItemsSource = enumval.ToList();
            listDeviceTypes.SelectionChanged += ListDeviceTypes_SelectionChanged;
            listDeviceTypes.SelectedIndex = 0;

            RemoteSystemAccessStatus accessStatus = await RemoteSystem.RequestAccessAsync();
            if (accessStatus == RemoteSystemAccessStatus.Allowed)
            {
                RemoteSystemWatcher m_remoteSystemWatcher = RemoteSystem.CreateWatcher();
                m_remoteSystemWatcher.RemoteSystemAdded += RemoteSystemWatcher_RemoteSystemAdded;
                m_remoteSystemWatcher.RemoteSystemRemoved += RemoteSystemWatcher_RemoteSystemRemoved;
                m_remoteSystemWatcher.RemoteSystemUpdated += RemoteSystemWatcher_RemoteSystemUpdated;
                m_remoteSystemWatcher.Start();
            }

            progressRing.IsActive = true;
            UpdateList();

            base.OnApplyTemplate();
        }

        private void ListDeviceTypes_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateList();

        private async void RemoteSystemWatcher_RemoteSystemUpdated(RemoteSystemWatcher sender, RemoteSystemUpdatedEventArgs args)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                progressRing.IsActive = true;
                if (DeviceMap.ContainsKey(args.RemoteSystem.Id))
                {
                    RemoteSystems.Remove(DeviceMap[args.RemoteSystem.Id]);
                    DeviceMap.Remove(args.RemoteSystem.Id);
                }
                RemoteSystems.Add(args.RemoteSystem);
                DeviceMap.Add(args.RemoteSystem.Id, args.RemoteSystem);
                UpdateList();
            });
        }

        internal void UpdateList()
        {
            ObservableCollection<RemoteSystem> bindingList = new ObservableCollection<RemoteSystem>();
            if (RemoteSystems != null)
            {
                foreach (RemoteSystem sys in RemoteSystems)
                {
                    if (listDeviceTypes.SelectedValue.Equals(DeviceType.All))
                    {
                        bindingList = RemoteSystems;
                    }
                    else if (listDeviceTypes.SelectedValue.ToString().Equals(sys.Kind))
                    {
                        bindingList.Add(sys);
                    }
                }

                progressRing.IsActive = false;
            }

            listDevices.ItemsSource = bindingList;
        }

        private async void RemoteSystemWatcher_RemoteSystemRemoved(RemoteSystemWatcher sender, RemoteSystemRemovedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                progressRing.IsActive = true;
                if (DeviceMap.ContainsKey(args.RemoteSystemId))
                {
                    RemoteSystems.Remove(DeviceMap[args.RemoteSystemId]);
                    DeviceMap.Remove(args.RemoteSystemId);
                }
            });
        }

        private async void RemoteSystemWatcher_RemoteSystemAdded(RemoteSystemWatcher sender, RemoteSystemAddedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                progressRing.IsActive = true;
                RemoteSystems.Add(args.RemoteSystem);
                DeviceMap.Add(args.RemoteSystem.Id, args.RemoteSystem);
            });
        }
    }
}

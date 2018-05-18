// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        private ListView _listDevices;
        private ComboBox _listDeviceTypes;
        private ProgressRing _progressRing;

        /// <summary>
        /// Gets List of All Remote Systems based on Selection Filter
        /// </summary>
        public ObservableCollection<RemoteSystem> RemoteSystems { get; private set; }

        /// <summary>
        /// Gets or sets the Line Color on control Header. takes **SystemControlBackgroundAccentBrush** by default
        /// </summary>
        public Brush HeaderLineColor
        {
            get { return (Brush)GetValue(HeaderLineColorProperty); }
            set { SetValue(HeaderLineColorProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="HeaderLineColor"/>.
        /// </summary>
        public static readonly DependencyProperty HeaderLineColorProperty = DependencyProperty.Register(
            nameof(HeaderLineColor),
            typeof(Brush),
            typeof(RemoteDevicePicker),
            new PropertyMetadata(Application.Current.Resources["SystemControlBackgroundAccentBrush"]));

        /// <summary>
        /// Gets or sets the DeviceList Selection Mode. Defaults to ListViewSelectionMode.Single
        /// </summary>
        public ListViewSelectionMode DeviceListSelectionMode
        {
            get { return (ListViewSelectionMode)GetValue(DeviceListSelectionModeProperty); }
            set { SetValue(DeviceListSelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets the dependency property for <see cref="DeviceListSelectionMode"/>.
        /// </summary>
        public static readonly DependencyProperty DeviceListSelectionModeProperty = DependencyProperty.Register(
            nameof(DeviceListSelectionMode),
            typeof(ListViewSelectionMode),
            typeof(RemoteDevicePicker),
            new PropertyMetadata(ListViewSelectionMode.Single));

        /// <summary>
        /// Fired when the Remote Device Picker is Closed.
        /// </summary>
        public event EventHandler<RemoteDevicePickerEventArgs> RemoteDevicePickerClosed;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteDevicePicker"/> class.
        /// </summary>
        public RemoteDevicePicker()
        {
            DefaultStyleKey = typeof(RemoteDevicePicker);
            SecondaryButtonText = "Done";
            SecondaryButtonClick += RemoteDevicePicker_SecondaryButtonClick;
            Loading += RemoteDevicePicker_Loading;
            Loaded += RemoteDevicePicker_Loaded;
        }

        private void RemoteDevicePicker_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ObservableCollection<RemoteSystem> selectedItems = new ObservableCollection<RemoteSystem>();
            foreach (RemoteSystem sys in _listDevices.SelectedItems)
            {
                selectedItems.Add(sys);
            }

            RemoteDevicePickerEventArgs eventArgs = new RemoteDevicePickerEventArgs(selectedItems);
            RemoteDevicePickerClosed?.Invoke(this, eventArgs);
            Hide();
        }

        /// <inheritdoc />
        protected override void OnApplyTemplate()
        {
            _listDevices = GetTemplateChild("PART_LISTDEVICES") as ListView;
            _listDeviceTypes = GetTemplateChild("PART_LISTDEVICETYPES") as ComboBox;
            _progressRing = GetTemplateChild("PART_PROGRESS") as ProgressRing;

            var enumval = Enum.GetValues(typeof(DeviceType)).Cast<DeviceType>();
            _listDeviceTypes.ItemsSource = enumval.ToList();
            _listDeviceTypes.SelectedIndex = 0;
            base.OnApplyTemplate();
        }

        private void ListDeviceTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateList();
        }

        private void RemoteDevicePicker_Loaded(object sender, RoutedEventArgs e)
        {
            RemoteSystems = new ObservableCollection<RemoteSystem>();
            DeviceMap = new Dictionary<string, RemoteSystem>();
            _listDeviceTypes.SelectionChanged += ListDeviceTypes_SelectionChanged;
        }

        private async void RemoteDevicePicker_Loading(FrameworkElement sender, object args)
        {
            RemoteSystemAccessStatus accessStatus = await RemoteSystem.RequestAccessAsync();
            if (accessStatus == RemoteSystemAccessStatus.Allowed)
            {
                RemoteSystemWatcher m_remoteSystemWatcher = RemoteSystem.CreateWatcher();
                m_remoteSystemWatcher.RemoteSystemAdded += RemoteSystemWatcher_RemoteSystemAdded;
                m_remoteSystemWatcher.RemoteSystemRemoved += RemoteSystemWatcher_RemoteSystemRemoved;
                m_remoteSystemWatcher.RemoteSystemUpdated += RemoteSystemWatcher_RemoteSystemUpdated;
                m_remoteSystemWatcher.Start();
            }

            _progressRing.IsActive = true;
            UpdateList();
        }

        private async void RemoteSystemWatcher_RemoteSystemUpdated(RemoteSystemWatcher sender, RemoteSystemUpdatedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _progressRing.IsActive = true;
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
                    if (_listDeviceTypes.SelectedValue.ToString().Equals(DeviceType.All.ToString()))
                    {
                        bindingList = RemoteSystems;
                    }
                    else if (_listDeviceTypes.SelectedValue.ToString().Equals(sys.Kind))
                    {
                        bindingList.Add(sys);
                    }
                }

                _progressRing.IsActive = false;
            }

            _listDevices.ItemsSource = bindingList;
        }

        private async void RemoteSystemWatcher_RemoteSystemRemoved(RemoteSystemWatcher sender, RemoteSystemRemovedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                _progressRing.IsActive = true;
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
                _progressRing.IsActive = true;
                RemoteSystems.Add(args.RemoteSystem);
                DeviceMap.Add(args.RemoteSystem.Id, args.RemoteSystem);
            });
        }
    }
}

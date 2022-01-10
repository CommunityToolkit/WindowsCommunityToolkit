// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Networking.Connectivity;
using Windows.System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Triggers
{
    /// <summary>
    /// Trigger for switching when the network availability changes
    /// </summary>
    public class NetworkConnectionStateTrigger : StateTriggerBase
    {
        private readonly WeakEventListener<NetworkConnectionStateTrigger, object, EventArgs> _weakEvent;

        private DispatcherQueue _dispatcherQueue;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkConnectionStateTrigger"/> class.
        /// </summary>
        public NetworkConnectionStateTrigger()
        {
            _dispatcherQueue = DispatcherQueue.GetForCurrentThread();

            _weakEvent = new WeakEventListener<NetworkConnectionStateTrigger, object, EventArgs>(this)
            {
                OnEventAction = (instance, source, eventArgs) => { NetworkInformation_NetworkStatusChanged(source); },
                OnDetachAction = listener => { NetworkInformation.NetworkStatusChanged -= OnNetworkEvent; }
            };

            NetworkInformation.NetworkStatusChanged += OnNetworkEvent;
            UpdateState();
        }

        private void NetworkInformation_NetworkStatusChanged(object sender)
        {
            _ = _dispatcherQueue.EnqueueAsync(UpdateState, DispatcherQueuePriority.Normal);
        }

        private void OnNetworkEvent(object source)
        {
            _weakEvent?.OnEvent(source, EventArgs.Empty);
        }

        private void UpdateState()
        {
            bool isConnected = false;
            var profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile != null)
            {
                isConnected = profile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;
            }

            SetActive((isConnected && ConnectionState == ConnectionState.Connected) || (!isConnected && ConnectionState == ConnectionState.Disconnected));
        }

        /// <summary>
        /// Gets or sets the state of the connection to trigger on.
        /// </summary>
        public ConnectionState ConnectionState
        {
            get { return (ConnectionState)GetValue(ConnectionStateProperty); }
            set { SetValue(ConnectionStateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ConnectionState"/> DependencyProperty
        /// </summary>
        public static readonly DependencyProperty ConnectionStateProperty =
            DependencyProperty.Register(nameof(ConnectionState), typeof(ConnectionState), typeof(NetworkConnectionStateTrigger), new PropertyMetadata(ConnectionState.Connected, OnConnectionStatePropertyChanged));

        private static void OnConnectionStatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (NetworkConnectionStateTrigger)d;
            obj.UpdateState();
        }
    }

    /// <summary>
    /// ConnectionStates
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// Connected
        /// </summary>
        Connected,

        /// <summary>
        /// Disconnected
        /// </summary>
        Disconnected,
    }
}
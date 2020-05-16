// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1512

// This file is inspired from the MvvmLight libray (lbugnion/mvvmlight),
// more info in ThirdPartyNotices.txt in the root of the project.

using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;

namespace Microsoft.Toolkit.Mvvm.ComponentModel
{
    /// <summary>
    /// A base class for viewmodels.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor will produce an instance that will use the <see cref="Messaging.Messenger.Default"/> instance
        /// to perform requested operations. It will also be available locally through the <see cref="Messenger"/> property.
        /// </remarks>
        protected ViewModelBase()
            : this(Messaging.Messenger.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="messenger">The <see cref="IMessenger"/> instance to use to send messages.</param>
        protected ViewModelBase(IMessenger messenger)
        {
            Messenger = messenger;
        }

        /// <summary>
        /// Gets the <see cref="IMessenger"/> instance in use.
        /// </summary>
        protected IMessenger Messenger { get; }

        private bool isActive;

        /// <summary>
        /// Gets or sets a value indicating whether the current view model is currently active.
        /// </summary>
        public bool IsActive
        {
            get => this.isActive;
            set
            {
                if (Set(ref this.isActive, value, true))
                {
                    if (value)
                    {
                        OnActivated();
                    }
                    else
                    {
                        OnDeactivated();
                    }
                }
            }
        }

        /// <summary>
        /// Raised whenever the <see cref="IsActive"/> property is set to <see langword="true"/>.
        /// Use this method to register to messages and do other initialization for this instance.
        /// </summary>
        protected virtual void OnActivated()
        {
        }

        /// <summary>
        /// Raised whenever the <see cref="IsActive"/> property is set to <see langword="false"/>.
        /// Use this method to unregister from messages and do general cleanup for this instance.
        /// </summary>
        /// <remarks>The base implementation unregisters all messages for this recipient (with no token).</remarks>
        protected virtual void OnDeactivated()
        {
            Messenger.Unregister(this);
        }

        /// <inheritdoc/>
        protected override void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            var message = new PropertyChangedMessage<T>(this, propertyName, oldValue, newValue);

            Messenger.Send(message);
        }
    }
}
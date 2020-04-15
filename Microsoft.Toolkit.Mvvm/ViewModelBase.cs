// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1512

// Original file header:
// ****************************************************************************
// <copyright file="ViewModelBase.cs" company="GalaSoft Laurent Bugnion">
// Copyright © GalaSoft Laurent Bugnion 2009-2016
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>22.4.2009</date>
// <project>GalaSoft.MvvmLight</project>
// <web>http://www.mvvmlight.net</web>
// <license>
// See license.txt in this project or http://www.galasoft.ch/license_MIT.txt
// </license>
// <LastBaseLevel>BL0014</LastBaseLevel>
// ****************************************************************************

using System.Runtime.CompilerServices;
using Microsoft.Toolkit.Mvvm.Messaging;
using Microsoft.Toolkit.Mvvm.Messaging.Messages;
using Microsoft.Toolkit.Mvvm.Services;

namespace Microsoft.Toolkit.Mvvm
{
    /// <summary>
    /// A base class for viewmodels.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject
    {
        /// <summary>
        /// The optional <see cref="Messenger"/> instance to use.
        /// </summary>
        private readonly Messenger? messenger;

        /// <summary>
        /// The optional <see cref="Ioc"/> instance to use.
        /// </summary>
        private readonly Ioc? ioc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <remarks>
        /// This constructor will produce an instance that will use the <see cref="Messaging.Messenger.Default"/>
        /// and <see cref="Services.Ioc.Default"/> services to perform operations.
        /// </remarks>
        protected ViewModelBase()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="messenger">The <see cref="Messenger"/> instance to use to send messages.</param>
        protected ViewModelBase(Messenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="ioc">The optional <see cref="Services.Ioc"/> instance to use to retrieve services.</param>
        protected ViewModelBase(Ioc ioc)
        {
            this.ioc = ioc;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="messenger">The <see cref="Messenger"/> instance to use to send messages.</param>
        /// <param name="ioc">The optional <see cref="Services.Ioc"/> instance to use to retrieve services.</param>
        protected ViewModelBase(Messenger messenger, Ioc ioc)
        {
            this.messenger = messenger;
            this.ioc = ioc;
        }

        /// <summary>
        /// Gets the <see cref="Messaging.Messenger"/> instance in use.
        /// </summary>
        protected Messenger Messenger => this.messenger ?? Messenger.Default;

        /// <summary>
        /// Gets the <see cref="Services.Ioc"/> instance in use.
        /// </summary>
        protected Ioc Ioc => this.ioc ?? Ioc.Default;

        private bool isActive;

        /// <summary>
        /// Gets or sets a value indicating whether the current view model is currently active.
        /// </summary>
        public bool IsActive
        {
            get => this.isActive;
            set
            {
                if (Set(ref this.isActive, value))
                {
                    if (value)
                    {
                        OnActivate();
                    }
                    else
                    {
                        OnDeactivate();
                    }
                }
            }
        }

        /// <summary>
        /// Raised whenever the <see cref="IsActive"/> property is set to <see langword="true"/>.
        /// Use this method to register to messages and do other initialization for this instance.
        /// </summary>
        protected virtual void OnActivate()
        {
        }

        /// <summary>
        /// Raised whenever the <see cref="IsActive"/> property is set to <see langword="false"/>.
        /// Use this method to unregister from messages and do general cleanup for this instance.
        /// </summary>
        /// <remarks>The base implementation unregisters all messages for this recipient (with no token).</remarks>
        protected virtual void OnDeactivate()
        {
            Messenger.Unregister(this);
        }

        /// <summary>
        /// Broadcasts a <see cref="PropertyChangedMessage{T}"/> with the specified
        /// parameters, without using any particular token (so using the default channel).
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="oldValue">The value of the property before it changed.</param>
        /// <param name="newValue">The value of the property after it changed.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <remarks>
        /// You should override this method if you wish to customize the channel being
        /// used to send the message (eg. if you need to use a specific token for the channel).
        /// </remarks>
        protected virtual void Broadcast<T>(T oldValue, T newValue, string propertyName)
        {
            var message = new PropertyChangedMessage<T>(this, propertyName, oldValue, newValue);

            Messenger.Send(message);
        }

        /// <summary>
        /// Compares the current and new values for a given property. If the value has changed,
        /// raises the <see cref="ObservableObject.PropertyChanging"/> event, updates the property with
        /// the new value, then raises the <see cref="ObservableObject.PropertyChanged"/> event.
        /// </summary>
        /// <typeparam name="T">The type of the property that changed.</typeparam>
        /// <param name="field">The field storing the property's value.</param>
        /// <param name="newValue">The property's value after the change occurred.</param>
        /// <param name="broadcast">If <see langword="true"/>, <see cref="Broadcast{T}"/> will also be invoked.</param>
        /// <param name="propertyName">(optional) The name of the property that changed.</param>
        /// <returns><see langword="true"/> if the property was changed, <see langword="false"/> otherwise.</returns>
        /// <remarks>
        /// This method is just like <see cref="ObservableObject.Set{T}(ref T, T, string)"/>, just with the addition
        /// of the <paramref name="broadcast"/> parameter. As such, following the behavior of the base method, the
        /// <see cref="ObservableObject.PropertyChanging"/> and <see cref="ObservableObject.PropertyChanged"/> events
        /// are not raised if the current and new value for the target property are the same.
        /// </remarks>
        protected bool Set<T>(ref T field, T newValue, bool broadcast, [CallerMemberName] string propertyName = null!)
        {
            if (!broadcast)
            {
                return Set(ref field, newValue, propertyName);
            }

            T oldValue = field;

            if (Set(ref field, newValue, propertyName))
            {
                Broadcast(oldValue, newValue, propertyName);

                return true;
            }

            return false;
        }
    }
}
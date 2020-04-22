// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#pragma warning disable SA1512

// Original file header:
// ****************************************************************************
// <copyright file="PropertyChangedMessage`1.cs" company="GalaSoft Laurent Bugnion">
// Copyright © GalaSoft Laurent Bugnion 2009-2016
// </copyright>
// ****************************************************************************
// <author>Laurent Bugnion</author>
// <email>laurent@galasoft.ch</email>
// <date>13.4.2009</date>
// <project>GalaSoft.MvvmLight.Messaging</project>
// <web>http://www.mvvmlight.net</web>
// <license>
// See license.txt in this project or http://www.galasoft.ch/license_MIT.txt
// </license>
// ****************************************************************************

namespace Microsoft.Toolkit.Mvvm.Messaging.Messages
{
    /// <summary>
    /// A message used to broadcast property changes in observable objects.
    /// </summary>
    /// <typeparam name="T">The type of the property to broadcast the change for.</typeparam>
    public class PropertyChangedMessage<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyChangedMessage{T}"/> class.
        /// </summary>
        /// <param name="sender">The original sender of the broadcast message.</param>
        /// <param name="propertyName">The name of the property that changed.</param>
        /// <param name="oldValue">The value that the property had before the change.</param>
        /// <param name="newValue">The value that the property has after the change.</param>
        public PropertyChangedMessage(object sender, string propertyName, T oldValue, T newValue)
        {
            Sender = sender;
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Gets the original sender of the broadcast message.
        /// </summary>
        public object Sender { get; }

        /// <summary>
        /// Gets the name of the property that changed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the value that the property had before the change.
        /// </summary>
        public T OldValue { get; }

        /// <summary>
        /// Gets the value that the property has after the change.
        /// </summary>
        public T NewValue { get; }
    }
}
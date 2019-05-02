// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
#if WINDOWS_UWP

#endif

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// Exception returned when invalid notification content is provided.
    /// </summary>
    internal sealed class NotificationContentValidationException : Exception
    {
        public NotificationContentValidationException(string message)
            : base(message)
        {
        }
    }
}
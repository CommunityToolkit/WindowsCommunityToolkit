// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// An interface for a notification XML element with a name.
    /// </summary>
    internal interface IHaveXmlName
    {
        /// <summary>
        /// Gets the name of the current element.
        /// </summary>
        string Name { get; }
    }
}

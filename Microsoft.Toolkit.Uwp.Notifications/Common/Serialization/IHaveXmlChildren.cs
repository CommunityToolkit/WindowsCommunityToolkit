// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

#nullable enable

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// An interface for a notification XML element with additional children.
    /// </summary>
    internal interface IHaveXmlChildren
    {
        /// <summary>
        /// Gets the children of the current element.
        /// </summary>
        IEnumerable<object> Children { get; }
    }
}
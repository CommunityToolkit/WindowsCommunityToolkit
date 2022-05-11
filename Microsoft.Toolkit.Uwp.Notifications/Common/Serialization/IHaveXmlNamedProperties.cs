// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

#nullable enable

namespace Microsoft.Toolkit.Uwp.Notifications
{
    /// <summary>
    /// An interface for a notification XML element with named properties.
    /// </summary>
    internal interface IHaveXmlNamedProperties
    {
        /// <summary>
        /// Enumerates the available named properties for the element.
        /// </summary>
        /// <returns>A sequence of named properties for the element.</returns>
        /// <remarks>The returned values must be valid XML values when <see cref="object.ToString"/> is called on them.</remarks>
        IEnumerable<KeyValuePair<string, object?>> EnumerateNamedProperties();
    }
}

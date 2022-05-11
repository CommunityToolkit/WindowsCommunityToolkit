// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications.Adaptive.Elements
{
    internal sealed class Element_AdaptiveProgressBar : IElement_ToastBindingChild, IHaveXmlName, IHaveXmlNamedProperties
    {
        public string Value { get; set; }

        public string Title { get; set; }

        public string ValueStringOverride { get; set; }

        public string Status { get; set; }

        /// <inheritdoc/>
        string IHaveXmlName.Name => "progress";

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            yield return new("value", Value);
            yield return new("title", Title);
            yield return new("valueStringOverride", ValueStringOverride);
            yield return new("status", Status);
        }
    }
}
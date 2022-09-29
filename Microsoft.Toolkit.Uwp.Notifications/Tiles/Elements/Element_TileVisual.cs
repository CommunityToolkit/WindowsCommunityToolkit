// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal sealed class Element_TileVisual : IHaveXmlName, IHaveXmlNamedProperties, IHaveXmlChildren
    {
        internal const TileBranding DEFAULT_BRANDING = TileBranding.Auto;
        internal const bool DEFAULT_ADD_IMAGE_QUERY = false;

        public bool? AddImageQuery { get; set; }

        public Uri BaseUri { get; set; }

        public TileBranding Branding { get; set; } = DEFAULT_BRANDING;

        public string ContentId { get; set; }

        public string DisplayName { get; set; }

        public string Language { get; set; }

        public string Arguments { get; set; }

        public IList<Element_TileBinding> Bindings { get; private set; } = new List<Element_TileBinding>();

        /// <inheritdoc/>
        string IHaveXmlName.Name => "visual";

        /// <inheritdoc/>
        IEnumerable<object> IHaveXmlChildren.Children => Bindings;

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            yield return new("addImageQuery", AddImageQuery);
            yield return new("baseUri", BaseUri);

            if (Branding != DEFAULT_BRANDING)
            {
                yield return new("branding", Branding.ToPascalCaseString());
            }

            yield return new("contentId", ContentId);
            yield return new("displayName", DisplayName);
            yield return new("lang", Language);
            yield return new("arguments", Arguments);
        }
    }
}
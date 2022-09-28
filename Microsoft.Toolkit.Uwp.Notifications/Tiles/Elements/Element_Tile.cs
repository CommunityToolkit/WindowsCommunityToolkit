// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal sealed class Element_Tile : BaseElement, IHaveXmlName, IHaveXmlChildren
    {
        public Element_TileVisual Visual { get; set; }

        /// <inheritdoc/>
        string IHaveXmlName.Name => "tile";

        /// <inheritdoc/>
        IEnumerable<object> IHaveXmlChildren.Children => new[] { Visual };
    }
}
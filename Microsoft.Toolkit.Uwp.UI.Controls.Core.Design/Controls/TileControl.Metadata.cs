// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Properties;

using Microsoft.VisualStudio.DesignTools.Extensibility;
using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
    internal class TileControlMetadata : AttributeTableBuilder
    {
        public TileControlMetadata()
            : base()
        {
            AddCallback(ControlTypes.TileControl,
                b =>
                {
                    b.AddCustomAttributes(nameof(TileControl.ScrollViewerContainer), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ImageAlignment), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ImageSource), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ScrollOrientation), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.OffsetX), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.OffsetY), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ParallaxSpeedRatio), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.IsAnimated), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.AnimationStepX), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.AnimationStepY), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.AnimationDuration), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}

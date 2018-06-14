// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.PropertyEditing;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
	internal class TileControlMetadata : AttributeTableBuilder
	{
        public TileControlMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.TileControl),
				b =>
				{
                    b.AddCustomAttributes(nameof(TileControl.ScrollViewerContainer), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ImageAlignment), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ImageSource), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ScrollOrientation), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.OffsetX), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.OffsetY), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.ParallaxSpeedRatio), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.IsAnimated), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.AnimationStepX), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.AnimationStepY), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TileControl.AnimationDuration), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

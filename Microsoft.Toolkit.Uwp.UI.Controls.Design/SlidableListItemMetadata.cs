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
	internal class SlidableListItemMetadata : AttributeTableBuilder
	{
        public SlidableListItemMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.SlidableListItem),
				b =>
				{
                    b.AddCustomAttributes(nameof(SlidableListItem.ExtraSwipeThreshold), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.IsOffsetLimited), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.IsRightCommandEnabled), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.IsLeftCommandEnabled), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.ActivationWidth), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.LeftIcon), new CategoryAttribute(Properties.Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(SlidableListItem.RightIcon), new CategoryAttribute(Properties.Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(SlidableListItem.LeftLabel), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.RightLabel), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.LeftForeground), new CategoryAttribute(Properties.Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(SlidableListItem.RightForeground), new CategoryAttribute(Properties.Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(SlidableListItem.LeftBackground), new CategoryAttribute(Properties.Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(SlidableListItem.RightBackground), new CategoryAttribute(Properties.Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(SlidableListItem.MouseSlidingEnabled), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.LeftCommand),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                        new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.RightCommand),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                        new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.SwipeStatus), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(SlidableListItem.IsPointerReleasedOnSwipingHandled), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

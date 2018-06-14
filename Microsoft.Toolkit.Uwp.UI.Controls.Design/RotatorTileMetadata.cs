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
	internal class RotatorTileMetadata : AttributeTableBuilder
	{
        public RotatorTileMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.RotatorTile),
				b =>
				{
                    b.AddCustomAttributes(nameof(RotatorTile.ExtraRandomDuration), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RotatorTile.RotationDelay), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RotatorTile.ItemsSource), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RotatorTile.ItemTemplate), 
                        new CategoryAttribute(Properties.Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                        );
                    b.AddCustomAttributes(nameof(RotatorTile.RotateDirection), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RotatorTile.CurrentItem), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

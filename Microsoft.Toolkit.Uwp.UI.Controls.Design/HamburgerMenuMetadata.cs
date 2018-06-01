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
	internal class HamburgerMenuMetadata : AttributeTableBuilder
	{
        public HamburgerMenuMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.HamburgerMenu),
				b =>
				{
                    b.AddCustomAttributes(nameof(HamburgerMenu.OpenPaneLength), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes("PanePlacement", new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes("DisplayMode", new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(HamburgerMenu.CompactPaneLength), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(HamburgerMenu.PaneForeground), new CategoryAttribute(Properties.Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(HamburgerMenu.PaneBackground), new CategoryAttribute(Properties.Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(HamburgerMenu.IsPaneOpen), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(HamburgerMenu.ItemsSource), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(HamburgerMenu.ItemTemplate), 
                        new CategoryAttribute(Properties.Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(HamburgerMenu.ItemTemplateSelector), 
                        new CategoryAttribute(Properties.Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(HamburgerMenu.SelectedItem), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(HamburgerMenu.SelectedIndex), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

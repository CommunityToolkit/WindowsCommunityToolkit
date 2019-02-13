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
	internal class TabViewMetadata : AttributeTableBuilder
	{
        public TabViewMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.TabView),
				b =>
                {
                    // Layout
                    b.AddCustomAttributes(nameof(TabView.TabWidthBehavior), new CategoryAttribute(Properties.Resources.CategoryLayout));
                    b.AddCustomAttributes(nameof(TabView.SelectedTabWidth), new CategoryAttribute(Properties.Resources.CategoryLayout));
                    b.AddCustomAttributes(nameof(TabView.IsCloseButtonOverlay), new CategoryAttribute(Properties.Resources.CategoryLayout));

                    // Interactions
                    b.AddCustomAttributes(nameof(TabView.CanCloseTabs), new CategoryAttribute(Properties.Resources.CategoryCommon));

                    // Templates
                    b.AddCustomAttributes(nameof(TabView.ItemHeaderTemplate),
                        new CategoryAttribute(Properties.Resources.CategoryCommon),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(TabView.TabActionHeader), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TabView.TabActionHeaderTemplate),
                        new CategoryAttribute(Properties.Resources.CategoryCommon),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(TabView.TabEndHeader), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TabView.TabEndHeaderTemplate),
                        new CategoryAttribute(Properties.Resources.CategoryCommon),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(TabView.TabStartHeader), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(TabView.TabStartHeaderTemplate),
                        new CategoryAttribute(Properties.Resources.CategoryCommon),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

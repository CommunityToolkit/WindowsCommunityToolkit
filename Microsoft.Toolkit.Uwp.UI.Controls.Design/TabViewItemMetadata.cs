// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
using Microsoft.Windows.Design.Model;
using Microsoft.Windows.Design.PropertyEditing;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{

	internal class TabViewItemMetadata : AttributeTableBuilder
	{
		public TabViewItemMetadata()
			: base()
		{
            AddCallback(typeof(TabViewItem),
                b =>
                {
                    b.AddCustomAttributes(nameof(TabViewItem.Header),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                        );
                    b.AddCustomAttributes(nameof(TabViewItem.HeaderTemplate),
                        new CategoryAttribute(Properties.Resources.CategoryCommon),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(TabViewItem.IsClosable),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                        );
                    b.AddCustomAttributes(nameof(TabViewItem.Icon),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                        );

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
		}
	}
}
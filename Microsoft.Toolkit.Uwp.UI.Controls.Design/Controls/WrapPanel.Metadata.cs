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
	internal class WrapPanelMetadata : AttributeTableBuilder
	{
        public WrapPanelMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.WrapPanel),
				b =>
				{   
					b.AddCustomAttributes(nameof(WrapPanel.Orientation),
						new CategoryAttribute(Properties.Resources.CategoryLayout)
					);
                    b.AddCustomAttributes(nameof(WrapPanel.HorizontalSpacing),
                        new CategoryAttribute(Properties.Resources.CategoryLayout)
                    );
                    b.AddCustomAttributes(nameof(WrapPanel.VerticalSpacing),
                        new CategoryAttribute(Properties.Resources.CategoryLayout)
                    );
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

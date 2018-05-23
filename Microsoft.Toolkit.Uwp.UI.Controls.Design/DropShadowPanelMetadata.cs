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
    internal class DropShadowPanelDefaults : DefaultInitializer
    {
        public override void InitializeDefaults(ModelItem item)
        {
            item.Properties[nameof(DropShadowPanel.BlurRadius)].SetValue(5d);
        }
    }

	internal class DropShadowPanelMetadata : AttributeTableBuilder
	{
		public DropShadowPanelMetadata()
			: base()
		{
            AddCallback(typeof(DropShadowPanel),
                b =>
                {
                    b.AddCustomAttributes(new FeatureAttribute(typeof(DropShadowPanelDefaults)));

                    b.AddCustomAttributes(nameof(DropShadowPanel.BlurRadius),
                        new PropertyOrderAttribute(PropertyOrder.Early),
                        new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                        );
                    b.AddCustomAttributes(nameof(DropShadowPanel.ShadowOpacity),
                       new PropertyOrderAttribute(PropertyOrder.Early),
                       new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                       );
                    b.AddCustomAttributes(nameof(DropShadowPanel.Color),
                        new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                        );
                    b.AddCustomAttributes(nameof(DropShadowPanel.OffsetX),
                       new PropertyOrderAttribute(PropertyOrder.Late),
                        new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                        );
                    b.AddCustomAttributes(nameof(DropShadowPanel.OffsetY),
                       new PropertyOrderAttribute(PropertyOrder.Late),
                       new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                       );
                    b.AddCustomAttributes(nameof(DropShadowPanel.OffsetZ),
                       new PropertyOrderAttribute(PropertyOrder.Late),
                       new CategoryAttribute(Properties.Resources.CategoryDropShadow)
                       );

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
		}
	}
}
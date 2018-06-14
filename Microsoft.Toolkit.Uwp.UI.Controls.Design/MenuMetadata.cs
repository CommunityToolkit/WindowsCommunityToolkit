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
	internal class MenuMetadata : AttributeTableBuilder
	{
        public MenuMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.Menu),
				b =>
				{   
					b.AddCustomAttributes(nameof(Menu.Items),
						new PropertyOrderAttribute(PropertyOrder.Early),
						new CategoryAttribute(Properties.Resources.CategoryCommon),
						//The following is necessary because this is a collection of an abstract type, so we help
						//the designer with populating supported types that can be added to the collection
                        new NewItemTypesAttribute(new System.Type[] {
                            typeof(MenuItem),
                        }),
						new AlternateContentPropertyAttribute()
					);
					b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

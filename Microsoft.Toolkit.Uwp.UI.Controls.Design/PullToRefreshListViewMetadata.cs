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
	internal class PullToRefreshListViewMetadata : AttributeTableBuilder
	{
        public PullToRefreshListViewMetadata()
			: base()
		{
			AddCallback(typeof(Microsoft.Toolkit.Uwp.UI.Controls.PullToRefreshListView),
				b =>
				{
                    b.AddCustomAttributes(nameof(PullToRefreshListView.OverscrollLimit), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(PullToRefreshListView.PullThreshold), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(PullToRefreshListView.RefreshCommand),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                        );
                    b.AddCustomAttributes(nameof(PullToRefreshListView.RefreshIntentCanceledCommand),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                        );
                    b.AddCustomAttributes(nameof(PullToRefreshListView.RefreshIndicatorContent), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(PullToRefreshListView.PullToRefreshLabel), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(PullToRefreshListView.ReleaseToRefreshLabel), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(PullToRefreshListView.PullToRefreshContent), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(PullToRefreshListView.ReleaseToRefreshContent), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(PullToRefreshListView.IsPullToRefreshWithMouseEnabled), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
				}
			);
		}
	}
}

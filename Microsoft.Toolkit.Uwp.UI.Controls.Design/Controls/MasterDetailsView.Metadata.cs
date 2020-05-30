// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Properties;

#if VS_DESIGNER_PROCESS_ISOLATION
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;
#else
using Microsoft.VisualStudio.DesignTools.Extensibility;
using Microsoft.VisualStudio.DesignTools.Extensibility.Metadata;
#endif

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
    internal class MasterDetailsViewMetadata : AttributeTableBuilder
    {
        public MasterDetailsViewMetadata()
            : base()
        {
            AddCallback(ControlTypes.MasterDetailsView,
                b =>
                {
                    b.AddCustomAttributes(nameof(MasterDetailsView.SelectedItem), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(MasterDetailsView.DetailsTemplate),
                        new CategoryAttribute(Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(MasterDetailsView.MasterPaneBackground), new CategoryAttribute(Resources.CategoryBrush));
                    b.AddCustomAttributes(nameof(MasterDetailsView.MasterHeader), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(MasterDetailsView.MasterHeaderTemplate),
                        new CategoryAttribute(Resources.CategoryAppearance),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(MasterDetailsView.MasterPaneWidth), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(MasterDetailsView.NoSelectionContent), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(MasterDetailsView.NoSelectionContentTemplate),
                        new CategoryAttribute(Resources.CategoryCommon),
                        new EditorBrowsableAttribute(EditorBrowsableState.Advanced)
                    );
                    b.AddCustomAttributes(nameof(MasterDetailsView.ViewState), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(MasterDetailsView.MasterCommandBar), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(MasterDetailsView.DetailsCommandBar), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}

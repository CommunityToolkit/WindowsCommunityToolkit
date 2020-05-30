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
    internal class RangeSelectorMetadata : AttributeTableBuilder
    {
        public RangeSelectorMetadata()
            : base()
        {
            AddCallback(ControlTypes.RangeSelector,
                b =>
                {
                    b.AddCustomAttributes(nameof(RangeSelector.Minimum), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RangeSelector.Maximum), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RangeSelector.RangeMin), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(RangeSelector.RangeMax), new CategoryAttribute(Resources.CategoryCommon));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}

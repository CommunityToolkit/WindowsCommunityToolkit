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
    internal class ImageExMetadata : AttributeTableBuilder
    {
        public ImageExMetadata()
            : base()
        {
            AddCallback(ControlTypes.ImageEx,
                b =>
                {
                    b.AddCustomAttributes(nameof(ImageEx.NineGrid), new CategoryAttribute(Resources.CategoryAppearance));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}

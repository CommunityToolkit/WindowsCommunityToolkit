// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Common;
using Microsoft.Windows.Design;
using Microsoft.Windows.Design.Metadata;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{

    internal class BladeItemMetadata : AttributeTableBuilder
    {
        public BladeItemMetadata()
            : base()
        {
            AddCallback(typeof(BladeItem),
                b =>
                {
                    b.AddCustomAttributes(nameof(BladeItem.TitleBarVisibility),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                        );
                    b.AddCustomAttributes(nameof(BladeItem.IsOpen),
                        new CategoryAttribute(Properties.Resources.CategoryCommon)
                        );
                    b.AddCustomAttributes(nameof(BladeItem.TitleBarBackground),
                        new CategoryAttribute(Properties.Resources.CategoryBrush)
                        );
                    b.AddCustomAttributes(nameof(BladeItem.CloseButtonBackground),
                        new CategoryAttribute(Properties.Resources.CategoryBrush)
                        );
                    b.AddCustomAttributes(nameof(BladeItem.CloseButtonForeground),
                        new CategoryAttribute(Properties.Resources.CategoryBrush)
                        );

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}
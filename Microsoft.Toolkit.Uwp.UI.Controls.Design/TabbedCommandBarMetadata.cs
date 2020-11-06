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

    internal class TabbedCommandBarMetadata : AttributeTableBuilder
    {
        public TabbedCommandBarMetadata()
            : base()
        {
            AddCallback(typeof(TabbedCommandBar),
                b =>
                {
                    //b.AddCustomAttributes(nameof(TabbedCommandBar.SomePropertyHere),
                    //    new CategoryAttribute(Properties.Resources.CategoryCommon)
                    //    );
                    b.AddCustomAttributes(nameof(TabbedCommandBar.Background),
                        new CategoryAttribute(Properties.Resources.CategoryBrush)
                        );

                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                }
            );
        }
    }
}